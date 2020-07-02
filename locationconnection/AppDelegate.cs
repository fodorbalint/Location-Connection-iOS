using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CoreLocation;
using Foundation;
using UIKit;
using UserNotifications;

namespace LocationConnection
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register ("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate, IUNUserNotificationCenterDelegate {
    
        [Export("window")]
        public UIWindow Window { get; set; }

        private string deviceTokenFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "devicetoken.txt");
		private string notificationRequestFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "notificationrequest.txt");

		[Export ("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {
            if (!File.Exists(notificationRequestFile))
            {
				File.WriteAllText(notificationRequestFile, "True");
			}

			UNUserNotificationCenter.Current.Delegate = this;

            return true;
        }

        [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {

			byte[] bytes = deviceToken.ToArray<byte>();
			string[] hexArray = bytes.Select(b => b.ToString("x2")).ToArray();
			string tokenString = string.Join(string.Empty, hexArray);

			CommonMethods.LogStatic("RegisteredForRemoteNotifications");

			File.WriteAllText(deviceTokenFile, tokenString);

			if (Session.Token != tokenString && !string.IsNullOrEmpty(Session.SessionID)) //when logging out, SessionID will be empty
            {
				CommonMethods.LogStatic("Token is new.");
				string url = "action=updatetoken&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&token=" + CommonMethods.UrlEncode(tokenString) + "&ios=1";
				string responseString = CommonMethods.MakeRequestSyncStatic(url);
				if (responseString.Substring(0, 2) == "OK")
				{
					Session.Token = tokenString;
				}
				else
				{
					CommonMethods.ReportErrorSilentStatic(responseString);	
				}
			}
			else
            {
				CommonMethods.LogStatic("Token is up to date.");
			}
		}

		[Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")] //does it come after autologin?
		public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action //called when app is in background, and user taps on notification
			completionHandler)
		{
			completionHandler();
			NSDictionary userInfo = response.Notification.Request.Content.UserInfo;

			CommonMethods c = new CommonMethods(null);
			BaseActivity context = CommonMethods.GetCurrentViewController();

			CommonMethods.LogStatic("DidReceiveNotificationResponse " + userInfo.ToString().Replace(Environment.NewLine, " ") + " logged in " + c.IsLoggedIn() + " context " + context);

            if (userInfo != null && userInfo.ContainsKey(new NSString("aps")))
			{
				int senderID = ((NSNumber)userInfo.ObjectForKey(new NSString("fromuser"))).Int32Value;
				int targetID = ((NSNumber)userInfo.ObjectForKey(new NSString("touser"))).Int32Value;

                if (targetID != Session.ID)
                {
					return;
                }

				IntentData.senderID = senderID;
                if (!(context is ChatOneActivity))
                {
					CommonMethods.OpenPage("ChatOneActivity", 1);
				}
				//otherwise foreground notification will refresh the page
			}
		}		

		[Export("application:didReceiveRemoteNotification:")]
		public void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo) //notification sent to the device while the notification permission was off are not received when the user turn permission on.
		{
			/*
            userInfo {
                aps =     {
					alert =         {
						body = S;
						title = "New message from b";
					};
				};
				fromuser = 123;
				inapp = 1;
				meta = "71|123|1591561106|0|0";
				touser = 10;
				type = sendMessage;
            }
            */
			CommonMethods.LogStatic("ReceivedRemoteNotification userInfo " + userInfo.ToString().Replace(Environment.NewLine, " "));

			try
			{
				string title = "";
				string body = "";

				int senderID = ((NSNumber)userInfo.ObjectForKey(new NSString("fromuser"))).Int32Value;
				int targetID = ((NSNumber)userInfo.ObjectForKey(new NSString("touser"))).Int32Value;
				string type = userInfo.ObjectForKey(new NSString("type")) as NSString;
				string meta = userInfo.ObjectForKey(new NSString("meta")) as NSString;
				bool inApp = (((NSNumber)userInfo.ObjectForKey(new NSString("inapp"))).Int32Value == 0) ? false : true;

				NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

				if (userInfo != null && aps.ContainsKey(new NSString("alert")))
				{
					NSDictionary alert = aps.ObjectForKey(new NSString("alert")) as NSDictionary;
					title = alert.ObjectForKey(new NSString("title")) as NSString;
					body = alert.ObjectForKey(new NSString("body")) as NSString;
				}
				else if (userInfo.ContainsKey(new NSString("title")))
				{
					title = userInfo.ObjectForKey(new NSString("title")) as NSString;
					body = userInfo.ObjectForKey(new NSString("body")) as NSString; // \\ already converted to \
				}
				HandleNotification(senderID, targetID, type, meta, inApp, title, body);
			}
			catch (Exception ex)
			{
				CommonMethods c = new CommonMethods(null);
				c.ReportErrorSilent(ex.Message + " " + ex.StackTrace);
			}
		}

		private void HandleNotification(int senderID, int targetID, string type, string meta, bool inApp, string title, string body)
        {
			int sep1Pos;
			int sep2Pos;
			int sep3Pos;
			int sep4Pos;
			int matchID;
			string senderName;
			string text;

			BaseActivity context = CommonMethods.GetCurrentViewController();
			if (targetID != Session.ID)
			{
				return;
			}

			try
			{
				switch (type)
				{
					case "sendMessage":


						if (context is ChatOneActivity)
						{
							long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

							//we need to update the Read time locally for display purposes before 
							sep1Pos = meta.IndexOf('|');
							sep2Pos = meta.IndexOf('|', sep1Pos + 1);
							sep3Pos = meta.IndexOf('|', sep2Pos + 1);

							int messageID = int.Parse(meta.Substring(0, sep1Pos));
							long sentTime = long.Parse(meta.Substring(sep2Pos + 1, sep3Pos - sep2Pos - 1));
							long seenTime = unixTimestamp;
							long readTime = unixTimestamp;

							meta = messageID + "|" + senderID + "|" + sentTime + "|" + seenTime + "|" + readTime + "|";

							if (senderID != Session.ID && senderID == ((ChatOneActivity)context).currentMatch.TargetID) //for tests, you can use 2 accounts from the same device, and a sent message would appear duplicate.
							{
								((ChatOneActivity)context).AddMessageItemOne(meta + body);
								((ChatOneActivity)context).c.MakeRequest("action=messagedelivered&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&MatchID=" + ((ChatOneActivity)context).currentMatch.MatchID + "&MessageID=" + messageID + "&Status=Read");
							}
							else if (inApp && senderID != Session.ID)
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChatNoOpen(senderID); }));
							}
						}
						else
						{
							if (inApp)
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChat(senderID); }));
							}

							//update message list
							if (context is ChatListActivity)
							{
								((ChatListActivity)context).InsertMessage(meta, body);
							}
						}
						break;

					case "messageDelivered":
					case "loadMessages":
					case "loadMessageList":
						if (context is ChatOneActivity && senderID == ((ChatOneActivity)context).currentMatch.TargetID)
						{
							string[] updateItems = meta.Substring(1, meta.Length - 2).Split("}{");
							foreach (string item in updateItems)
							{
								((ChatOneActivity)context).UpdateMessageItem(item);
							}
						}
						break;

					case "matchProfile":
						if (inApp) ////it is impossible to stand in that chat if wasn't previously a match
						{
							if (context is ChatOneActivity)
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChatNoOpen(senderID); }));
							}
							else
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChat(senderID); }));
							}
						}

						if (context is ChatListActivity)
						{
							string matchItem = meta;
							ServerParser<MatchItem> parser = new ServerParser<MatchItem>(matchItem);
							((ChatListActivity)context).AddMatchItem(parser.returnCollection[0]);
						}

						AddUpdateMatch(senderID, true);
						if (context is ProfileViewActivity)
						{
							((ProfileViewActivity)context).UpdateStatus(senderID, true);
						}
						break;

					case "rematchProfile":
						sep1Pos = meta.IndexOf('|');

						matchID = int.Parse(meta.Substring(0, sep1Pos));
						bool active = bool.Parse(meta.Substring(sep1Pos + 1));

						if (inApp)
						{
							if (context is ChatOneActivity && ((ChatOneActivity)context).currentMatch.TargetID == senderID)
							{
								context.c.Snack(title);
							}
							else if (context is ChatOneActivity)
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChatNoOpen(senderID); }));
							}
							else
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChat(senderID); }));
							}
						}

						AddUpdateMatch(senderID, true);
						if (context is ChatListActivity)
						{
							((ChatListActivity)context).UpdateMatchItem(matchID, active, null);
						}
						else if (context is ChatOneActivity)
						{
							((ChatOneActivity)context).UpdateStatus(senderID, active, null);
						}
						else if (context is ProfileViewActivity)
						{
							((ProfileViewActivity)context).UpdateStatus(senderID, true);
						}

						break;

					case "unmatchProfile":
						sep1Pos = meta.IndexOf('|');

						matchID = int.Parse(meta.Substring(0, sep1Pos));
						long unmatchDate = long.Parse(meta.Substring(sep1Pos + 1));

						if (context.IsUpdatingFrom(senderID))
						{
							context.RemoveUpdatesFrom(senderID);
						}
						if (context.IsUpdatingTo(senderID))
						{
							context.RemoveUpdatesTo(senderID);
						}

						if (inApp)
						{
							if (context is ChatOneActivity && ((ChatOneActivity)context).currentMatch.TargetID == senderID)
							{
								context.c.Snack(title);
							}
							else if (context is ChatOneActivity)
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChatNoOpen(senderID); }));
							}
							else
							{
								context.c.SnackAction(title, LangEnglish.ShowReceived, new Action(delegate () { GoToChat(senderID); }));
							}
						}

						AddUpdateMatch(senderID, false);
						if (context is ChatListActivity)
						{
							((ChatListActivity)context).UpdateMatchItem(matchID, false, unmatchDate);
						}
						else if (context is ChatOneActivity)
						{
							((ChatOneActivity)context).UpdateStatus(senderID, false, unmatchDate);
						}
						else if (context is ProfileViewActivity)
						{
							((ProfileViewActivity)context).UpdateStatus(senderID, false);
						}

						break;

					case "locationUpdate":
						sep1Pos = meta.IndexOf('|');
						sep2Pos = meta.IndexOf('|', sep1Pos + 1);

						senderName = meta.Substring(0, sep1Pos);
						int frequency = int.Parse(meta.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1));

						if (!context.IsUpdatingFrom(senderID))
						{
							context.AddUpdatesFrom(senderID);

							text = senderName + " " + LangEnglish.LocationUpdatesFromStart + " " + frequency + " s.";
							if (context is ProfileViewActivity)
							{
								((ProfileViewActivity)context).UpdateLocationStart(senderID, text);
							}
							else
							{
								context.c.SnackAction(text, LangEnglish.ShowReceived, new Action(delegate () { GoToProfile(senderID); }));
							}
						}

						sep3Pos = meta.IndexOf('|', sep2Pos + 1);
						sep4Pos = meta.IndexOf('|', sep3Pos + 1);

						long time = long.Parse(meta.Substring(sep2Pos + 1, sep3Pos - sep2Pos - 1));
						double latitude = double.Parse(meta.Substring(sep3Pos + 1, sep4Pos - sep3Pos - 1), CultureInfo.InvariantCulture);
						double longitude = double.Parse(meta.Substring(sep4Pos + 1), CultureInfo.InvariantCulture);

						context.AddLocationData(senderID, latitude, longitude, time);

						if (!(ListActivity.listProfiles is null))
						{
							foreach (Profile user in ListActivity.listProfiles)
							{
								if (user.ID == senderID)
								{
									user.LastActiveDate = time;
									user.Latitude = latitude;
									user.Longitude = longitude;
									user.LocationTime = time;
								}
							}
						}

						if (context is ListActivity && (bool)Settings.IsMapView)
						{
							foreach (ProfileAnnotation annotation in ListActivity.profileAnnotations)
							{
								if (annotation.Title == senderID.ToString())
								{
									annotation.SetCoordinate(new CLLocationCoordinate2D(latitude, longitude));
								}
							}
						}
						else if (context is ProfileViewActivity)
						{
							((ProfileViewActivity)context).UpdateLocation(senderID, time, latitude, longitude);
						}
						break;

					case "locationUpdateEnd":
						senderName = meta;

						if (context.IsUpdatingFrom(senderID)) //user could have gone to the background, clearing out the list of people to receive updates from.
						{
							context.RemoveUpdatesFrom(senderID);

							text = senderName + " " + LangEnglish.LocationUpdatesFromEnd;
							context.c.Snack(text);
						}
						break;

				}
			}
			catch (Exception ex)
			{
				CommonMethods c = new CommonMethods(null);
				c.ReportErrorSilent(ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine + " Error in DidReceiveMessage");
			}
		}

		private void GoToChat(int senderID)
		{
			IntentData.senderID = senderID;
			CommonMethods.OpenPage("ChatOneActivity", 1);
		}

		private void GoToChatNoOpen(int senderID)
		{
			IntentData.senderID = senderID;
			((ChatOneActivity)CommonMethods.GetCurrentViewController()).RefreshPage();
		}

		private void GoToProfile(int targetID)
		{
			if (!(CommonMethods.GetCurrentViewController() is ChatOneActivity)) //this condition is not necessary in iOS where the ChatOneActivity retains its currentMatch when clicking on a profile and navigating back
			{
				Session.CurrentMatch = null; //It must be set to null, otherwise when clicking the chat button, we are going back to the current activity if a chat was open before
											 //currentmatch should be kept even if standing another chat, because pressing the back button from profile view should take us back to the current chat.
			}

			IntentData.profileViewPageType = Constants.ProfileViewType_Standalone;
			IntentData.targetID = targetID;
			CommonMethods.OpenPage("ProfileViewActivity", 1);			
		}

		public static void AddUpdateMatch(int senderID, bool isMatch)
		{
            if (!(ListActivity.viewProfiles is null))
            {
				for (int i = 0; i < ListActivity.viewProfiles.Count; i++)
				{
					if (ListActivity.viewProfiles[i].ID == senderID)
					{
						if (isMatch)
						{
							ListActivity.viewProfiles[i].UserRelation = 3;
						}
						else
						{
							ListActivity.viewProfiles[i].UserRelation = 2;
						}
						break;
					}
				}
			}

			if (!(ListActivity.listProfiles is null))
			{
				for (int i = 0; i < ListActivity.listProfiles.Count; i++)
				{
					if (ListActivity.listProfiles[i].ID == senderID)
					{
						if (isMatch)
						{
							ListActivity.listProfiles[i].UserRelation = 3;
						}
						else
						{
							ListActivity.listProfiles[i].UserRelation = 2;
						}
						break;
					}
				}
			}
		}               

        [Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
        public void FailedToRegisterForRemoteNotifications(UIKit.UIApplication application, Foundation.NSError error)
        {
			CommonMethods.LogStatic("FailedToRegisterForRemoteNotifications, error: " + error.Description);
        }   

        // UISceneSession Lifecycle

        [Export ("application:configurationForConnectingSceneSession:options:")]
        public UISceneConfiguration GetConfiguration (UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            // Called when a new scene session is being created.
            // Use this method to select a configuration to create the new scene with.
            return UISceneConfiguration.Create ("Default Configuration", connectingSceneSession.Role);
        }

        [Export ("application:didDiscardSceneSessions:")]
        public void DidDiscardSceneSessions (UIApplication application, NSSet<UISceneSession> sceneSessions)
        {
            // Called when the user discards a scene session.
            // If any sessions were discarded while the application was not running, this will be called shortly after `FinishedLaunching`.
            // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        }
	}
}
