using System;
using System.Globalization;
using System.IO;
using CoreLocation;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;

namespace LocationConnection
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register ("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate {
    
        [Export("window")]
        public UIWindow Window { get; set; }

        private string deviceTokenFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "devicetoken.txt");
        private string tokenUptoDateFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tokenuptodate.txt");
		private string notificationRequestFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "notificationrequest.txt");

		[Export ("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {
            Firebase.Core.App.Configure();

            //if (!File.Exists(notificationRequestFile))
            {
				File.WriteAllText(notificationRequestFile, "True");
			}

			var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                Console.WriteLine("Notification authorization granted: " + granted);
				CommonMethods.LogActivityStatic("Notification authorization granted: " + granted);
				if (granted)
                {
                    InvokeOnMainThread(() => {
                        UIApplication.SharedApplication.RegisterForRemoteNotifications();
                        Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
                    });
                }
            });

            UNUserNotificationCenter.Current.Delegate = this;
            Messaging.SharedInstance.Delegate = this;    

            var token = Messaging.SharedInstance.FcmToken ?? "";
            Console.WriteLine($"Existing FCM token: {token}");


            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            return true;
        }

        [Export ("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"DidReceiveRegistrationToken: token: {fcmToken}");

            File.WriteAllText(deviceTokenFile, fcmToken);
            File.WriteAllText(tokenUptoDateFile, "False");
            // DO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }

        [Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
			Console.WriteLine("RegisteredForRemoteNotifications");
        }

        [Export("messaging:didReceiveMessage:")]
        public void DidReceiveMessage(Firebase.CloudMessaging.Messaging messaging, Firebase.CloudMessaging.RemoteMessage remoteMessage)
        {
			BaseActivity context = CommonMethods.GetCurrentViewController();

			//fires in-app, or when app entered foreground.
			Console.WriteLine("DidReceiveMessage " + remoteMessage.AppData.ToString().Replace(Environment.NewLine, "") + " current window: " + context);
			context.c.LogActivity("DidReceiveMessage " + remoteMessage.AppData.ToString().Replace(Environment.NewLine, "") + " current window: " + context); //DidReceiveMessage is called after the viewcontrollers's ViewDidLoad, ViewWillAppear, ViewDidLayoutSubviews, entering foreground sequence, so c cannot be null.

			int sep1Pos;
            int sep2Pos;
            int sep3Pos;
			int sep4Pos;
            int matchID;
            string senderName;
            string text;

			int senderID = int.Parse(remoteMessage.AppData["fromuser"].ToString());
			string type = remoteMessage.AppData["type"].ToString();
            string meta = remoteMessage.AppData["meta"].ToString();
            bool inApp = (remoteMessage.AppData["inapp"].ToString() == "0") ? false : true;

            try
            {
                //context.c.LogActivity("DidReceiveMessage " + type);

				switch (type)
				{
					case "sendMessage":
						string title = remoteMessage.AppData["title"].ToString();
						string body = remoteMessage.AppData["body"].ToString();

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

							if (senderID != Session.ID && senderID == Session.CurrentMatch.TargetID) //for tests, you can use 2 accounts from the same device, and a sent message would appear duplicate.
							{
								((ChatOneActivity)context).No_Messages.Hidden = true;
								((ChatOneActivity)context).AddMessageItemOne(meta + body);
								((ChatOneActivity)context).ScrollToBottom(true);
								((ChatOneActivity)context).c.MakeRequest("action=messagedelivered&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&MatchID=" + Session.CurrentMatch.MatchID + "&MessageID=" + messageID + "&Status=Read");
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
						if (context is ChatOneActivity)
						{
							string[] updateItems = meta.Substring(1, meta.Length - 2).Split("}{");
							foreach (string item in updateItems)
							{
								sep1Pos = meta.IndexOf('|');
								sep2Pos = meta.IndexOf('|', sep1Pos + 1);

								if (senderID == Session.CurrentMatch.TargetID)
								{
									((ChatOneActivity)context).UpdateMessageItem(item);
								}								
							}
						}
						break;

					case "matchProfile":
						if (inApp)
						{
							title = remoteMessage.AppData["title"].ToString();

                            if (context is ChatOneActivity && Session.CurrentMatch.TargetID == senderID)
                            {
								context.c.SnackAction(title, null, null);
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

						if (context is ChatListActivity)
						{
							string matchItem = meta;
							ServerParser<MatchItem> parser = new ServerParser<MatchItem>(matchItem);
							((ChatListActivity)context).AddMatchItem(parser.returnCollection[0]);
						}

						if (context is ProfileViewActivity)
						{
							string matchItem = meta;
							ServerParser<MatchItem> parser = new ServerParser<MatchItem>(matchItem);
							((ProfileViewActivity)context).AddNewMatch(senderID, parser.returnCollection[0]);
						}
						else
						{
							AddUpdateMatch(senderID, true);
						}
						break;

					case "rematchProfile":
						sep1Pos = meta.IndexOf('|');

						matchID = int.Parse(meta.Substring(0, sep1Pos));
						bool active = bool.Parse(meta.Substring(sep1Pos + 1));

						if (inApp)
						{
							title = remoteMessage.AppData["title"].ToString();
							if (context is ChatOneActivity && Session.CurrentMatch.TargetID == senderID)
							{
								context.c.SnackAction(title, null, null);
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

						if (context is ChatListActivity)
						{
							((ChatListActivity)context).UpdateMatchItem(matchID, active, null);
						}
						else if (context is ChatOneActivity)
						{
							((ChatOneActivity)context).UpdateStatus(senderID, active, null);
						}

						if (context is ProfileViewActivity)
						{
							((ProfileViewActivity)context).UpdateStatus(senderID, true, matchID);
						}
						else
						{
							AddUpdateMatch(senderID, true);
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

						if (inApp)
						{
							title = remoteMessage.AppData["title"].ToString();
							if (context is ChatOneActivity && Session.CurrentMatch.TargetID == senderID)
							{
								context.c.SnackAction(title, null, null);
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

						if (context is ChatListActivity)
						{
							((ChatListActivity)context).UpdateMatchItem(matchID, false, unmatchDate);
						}
						else if (context is ChatOneActivity)
						{
							((ChatOneActivity)context).UpdateStatus(senderID, false, unmatchDate);
						}

						if (context is ProfileViewActivity)
						{
							((ProfileViewActivity)context).UpdateStatus(senderID, false, null);
						}
						else
						{
							AddUpdateMatch(senderID, false);
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
			CommonMethods.GetCurrentViewController().c.LogActivity("GoToProfile");
			IntentData.profileViewPageType = "standalone";
			IntentData.targetID = targetID;
			CommonMethods.OpenPage("ProfileViewActivity", 1);			
		}

		public void AddUpdateMatch(int senderID, bool isMatch)
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
					}
				}
			}			
		}

        [Export("application:didReceiveRemoteNotification:")]
        public void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            Console.WriteLine("ReceivedRemoteNotification " + userInfo);
        }

        [Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
        public void FailedToRegisterForRemoteNotifications(UIKit.UIApplication application, Foundation.NSError error)
        {
            Console.WriteLine("FailedToRegisterForRemoteNotifications, error: " + error.Description);
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
/*
using System;
using Foundation;
using UIKit;

namespace LocationConnection
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIResponder, IUIApplicationDelegate
    {

        [Export("window")]
        public UIWindow Window { get; set; }

        [Export("application:didFinishLaunchingWithOptions:")]
        public bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            return true;
        }

        // UISceneSession Lifecycle

        [Export("application:configurationForConnectingSceneSession:options:")]
        public UISceneConfiguration GetConfiguration(UIApplication application, UISceneSession connectingSceneSession, UISceneConnectionOptions options)
        {
            // Called when a new scene session is being created.
            // Use this method to select a configuration to create the new scene with.
            return UISceneConfiguration.Create("Default Configuration", connectingSceneSession.Role);
        }

        [Export("application:didDiscardSceneSessions:")]
        public void DidDiscardSceneSessions(UIApplication application, NSSet<UISceneSession> sceneSessions)
        {
            // Called when the user discards a scene session.
            // If any sessions were discarded while the application was not running, this will be called shortly after `FinishedLaunching`.
            // Use this method to release any resources that were specific to the discarded scenes, as they will not return.
        }

        [Export("applicationDidEnterBackground:")]
        public void DidEnterBackground(UIApplication application)
        {
            Console.WriteLine(Environment.NewLine + "-------------------------------------------------------------- DidEnterBackground");
            BaseActivity.isAppForeground = false;
        }

        [Export("applicationWillEnterForeground:")]
        public void WillEnterForeground(UIApplication application)
        {
            CommonMethods c = new CommonMethods(null);
            Console.WriteLine(Environment.NewLine + "-------------------------------------------------------------- WillEnterForeground IsLocationEnabled " + c.IsLocationEnabled() + " IsLoggedIn " + c.IsLoggedIn());
            BaseActivity.isAppForeground = true;
        }
    }
}*/

