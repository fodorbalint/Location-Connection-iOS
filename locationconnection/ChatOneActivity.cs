using CoreGraphics;
using Firebase.CloudMessaging;
using Foundation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using UIKit;
using UserNotifications;

namespace LocationConnection
{
    public partial class ChatOneActivity : BaseActivity
    {
		private static bool foregroundNotificationSet;
		List<MessageItem> messageItems;
		string earlyDelivery;
		CustomTapNoDelay tap;
		public MatchItem currentMatch;

		private string notificationRequestFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "notificationrequest.txt");

		public UILabel No_Messages { get { return NoMessages; } }
		public UIScrollView Chat_MessageWindow { get { return ChatMessageWindow; } }

		public ChatOneActivity (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
			try
			{
				base.ViewDidLoad();

				if (!foregroundNotificationSet)
				{
					UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => {
						c.CW("Entered foreground, registering for notifications");
						c.LogActivity("Entered foreground, registering for notifications");

						var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
						UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
							Console.WriteLine("ChatOne Notification authorization granted: " + granted);
							CommonMethods.LogActivityStatic("ChatOne Notification authorization granted: " + granted);
							if (granted)
							{
								InvokeOnMainThread(() => {
									UIApplication.SharedApplication.RegisterForRemoteNotifications();
									Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
								});
							}
						});

						BaseActivity currentController = CommonMethods.GetCurrentViewController();
						if (currentController is ChatOneActivity)
						{
							((ChatOneActivity)currentController).SetMenu(); //needed in case location updates were running before backgrounding
							((ChatOneActivity)currentController).RefreshPage();
						}
					});

					foregroundNotificationSet = true;
				}

				NoMessages.Text = LangEnglish.NoMessages;
				NoMessages.Hidden = true;

				c.DrawBorder(ChatEditMessage);

				c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

				MenuUnmatch.SetTitle(LangEnglish.MenuUnmatch, UIControlState.Normal);
				MenuBlock.SetTitle(LangEnglish.MenuBlock, UIControlState.Normal);
				MenuReport.SetTitle(LangEnglish.MenuReport, UIControlState.Normal);

				c.HideMenu(MenuLayer, MenuContainer, false);

				ChatMessageWindow.RowHeight = UITableView.AutomaticDimension;
				ChatMessageWindow.EstimatedRowHeight = 100;

				ChatOneBack.TouchUpInside += ChatOneBack_Click;
				ChatSendMessage.TouchUpInside += ChatSendMessage_Click;

                MenuIcon.TouchUpInside += MenuIcon_Click;
				MenuLayer.TouchDown += MenuLayer_TouchDown;

				MenuLocationUpdates.TouchUpInside += MenuLocationUpdates_Click;
				MenuFriend.TouchUpInside += MenuFriend_Click;
				MenuUnmatch.TouchUpInside += MenuUnmatch_Click;
                MenuReport.TouchUpInside += MenuReport_Click;
				MenuBlock.TouchUpInside += MenuBlock_Click;

				RoundBottom_Base = RoundBottom;
				Snackbar_Base = Snackbar;

				BottomConstraint_Base = BottomConstraint;
				SnackTopConstraint_Base = SnackTopConstraint;
				SnackBottomConstraint_Base = SnackBottomConstraint;
				ChatOneLeftConstraint_Base = ChatOneLeftConstraint;
				ChatOneRightConstraint_Base = ChatOneRightConstraint;
				ChatMessageWindow_Base = ChatMessageWindow;
			}
			catch (Exception ex)
			{
				c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

			c.SetShadow(MenuContainer, 0, 0, 10);
        }

        public override async void ViewWillAppear(bool animated)
        {
			try
			{
				base.ViewWillAppear(animated);

                if (currentMatch is null && Session.CurrentMatch != null)
                {
					currentMatch = (MatchItem)c.Clone(Session.CurrentMatch);
                }

				SetMenu();

				var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
				UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
					CommonMethods.LogActivityStatic("ChatOne notification authorization granted: " + granted);
					if (granted)
					{
						InvokeOnMainThread(() => {
							UIApplication.SharedApplication.RegisterForRemoteNotifications();
							Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
						});
					}
                    else if (bool.Parse(File.ReadAllText(notificationRequestFile)) == true)
                    {
						InvokeOnMainThread(() => {
							c.ActionAlert("", LangEnglish.ChatOneEnableNotifications, LangEnglish.DialogYes, LangEnglish.DialogNo, LangEnglish.DialogDontAsk, LangEnglish.DialogGoToSettings, (alert) => {
							    UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
						    }, (alert) => {
						    }, (alert) => {
							    File.WriteAllText(notificationRequestFile, "False");
						    }, (alert) => {
							    CommonMethods.OpenPage("SettingsActivity", 1);
						    }, ChatViewProfile);
						});
					}
				});

				string responseString;
				if (!(IntentData.senderID is null))
				{
					responseString = await c.MakeRequest("action=loadmessages&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&TargetID=" + (int)IntentData.senderID);

					IntentData.senderID = null;

					if (responseString.Substring(0, 2) == "OK")
					{
						LoadMessages(responseString, false);
					}
					else if (responseString == "ERROR_MatchNotFound")
					{
						c.CW("Match not found");
						Session.SnackMessage = LangEnglish.MatchNotFound;
						CommonMethods.OpenPage(null, 0);
					}
					else
					{
						c.ReportError(responseString);
					}
				}
				else
				{
					LoadHeader();

					responseString = await c.MakeRequest("action=loadmessages&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&MatchID=" + currentMatch.MatchID);

					if (responseString.Substring(0, 2) == "OK")
					{
						LoadMessages(responseString, true);
					}
					else
					{
						c.ReportError(responseString);
					}
				}
			}
			catch (Exception ex)
			{
				c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

        public async void RefreshPage() //called whenever ChatOneActivity enters foreground, either from a notification tap or not.
		{
            if (IntentData.senderID != null) 
            {
				View.EndEditing(true);
				ChatEditMessage.Text = "";

				string responseString = await c.MakeRequest("action=loadmessages&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&TargetID=" + (int)IntentData.senderID);

				IntentData.senderID = null;

				if (responseString.Substring(0, 2) == "OK")
				{
					LoadMessages(responseString, false);
				}
				else if (responseString == "ERROR_MatchNotFound")
				{
					Session.SnackMessage = LangEnglish.MatchNotFound;
					CommonMethods.OpenPage(null, 0);
				}
				else
				{
					c.ReportError(responseString);
				}
			}
		}

        private void SetMenu()
		{
			int targetID;
			if (IntentData.senderID != null) //click from an in-app or background notification
			{
				targetID = (int)IntentData.senderID;
			}
			else
			{
				targetID = (int)currentMatch.TargetID;
			}

			if ((bool)Session.UseLocation && c.IsLocationEnabled())
			{
				MenuLocationUpdates.Hidden = false;
				if (IsUpdatingTo(targetID))
				{
					MenuLocationUpdates.SetTitle(LangEnglish.MenuStopLocationUpdates, UIControlState.Normal);
				}
				else
				{
					MenuLocationUpdates.SetTitle(LangEnglish.MenuStartLocationUpdates, UIControlState.Normal);
				}
			}
			else
			{
				MenuLocationUpdates.Hidden = true;
			}

			if (!(currentMatch is null))
			{
				if (currentMatch.UnmatchDate is null)
				{
					MenuFriend.Hidden = false;
				}
				else
				{
					MenuFriend.Hidden = true;
				}

                if (currentMatch.TargetID == 0)
                {
					MenuBlock.Hidden = true;
                }
                else
                {
					MenuBlock.Hidden = false;
                }
			}
		}

		private void MenuIcon_Click(object sender, EventArgs e)
		{
			c.ShowMenu(MenuLayer, MenuContainer);
		}

		private void MenuLayer_TouchDown(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);
		}

		private void MenuLocationUpdates_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);
			if (IsUpdatingTo((int)currentMatch.TargetID))
			{
				StopRealTimeLocation();
			}
			else
			{
				if (Session.InAppLocationRate > 60)
				{
					ChangeSettings();
				}
				else
				{
					StartRealTimeLocation();
				}
			}
		}

		private void MenuFriend_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);
			if (!(currentMatch is null) && !(currentMatch.Friend is null))
			{
				AddFriend();
			}
			else
			{
				c.Snack(LangEnglish.ChatOneDataLoading);
			}
		}

		private void MenuUnmatch_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);
			if (!(currentMatch is null) && !(currentMatch.Friend is null))
			{
				Unmatch();
			}
			else
			{
				c.Snack(LangEnglish.ChatOneDataLoading);
			}
		}

		private void MenuReport_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			c.DisplayCustomDialog(LangEnglish.ConfirmAction, LangEnglish.ReportDialogText, LangEnglish.DialogYes, LangEnglish.DialogNo, async alert =>
			{
				string responseString = await c.MakeRequest("action=reportchatone&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&TargetID=" + currentMatch.TargetID + "&MatchID=" + currentMatch.MatchID);
				if (responseString.Substring(0, 2) == "OK")
				{
					c.Snack(LangEnglish.UserReported);
				}
				else
				{
					c.ReportError(responseString);
				}
			}, null);
		}

		private void MenuBlock_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			c.DisplayCustomDialog(LangEnglish.ConfirmAction, LangEnglish.BlockDialogText, LangEnglish.DialogYes, LangEnglish.DialogNo, async alert =>
			{
				if (IsUpdatingTo((int)currentMatch.TargetID)) {
					RemoveUpdatesTo((int)currentMatch.TargetID);
				}

				long unixTimestamp = c.Now();
				string responseString = await c.MakeRequest("action=blockchatone&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&TargetID=" + currentMatch.TargetID + "&time=" + unixTimestamp);
				if (responseString.Substring(0, 2) == "OK")
				{

					if (!(ListActivity.listProfiles is null))
					{
						for (int i = 0; i < ListActivity.listProfiles.Count; i++)
						{
							if (ListActivity.listProfiles[i].ID == currentMatch.TargetID)
							{
								ListActivity.listProfiles.RemoveAt(i);
								break;
							}
						}
					}
					if (!(ListActivity.viewProfiles is null))
					{
						for (int i = 0; i < ListActivity.viewProfiles.Count; i++)
						{
							if (ListActivity.viewProfiles[i].ID == currentMatch.TargetID)
							{
								ListActivity.viewProfiles.RemoveAt(i);
								break;
							}
						}
					}

					CommonMethods.OpenPage("ListActivity", 1);
				}
				else
				{
					c.ReportError(responseString);
				}
			}, null);
		}

		private void LoadHeader()
		{
			TargetName.Text = currentMatch.TargetName;

			ImageCache im = new ImageCache(this);
			im.LoadImage(ChatTargetImage, currentMatch.TargetID.ToString(), currentMatch.TargetPicture);
		}

		private void LoadMessages(string responseString, bool merge)
		{
			responseString = responseString.Substring(3);

			if (!merge)
			{
				ServerParser<MatchItem> parser = new ServerParser<MatchItem>(responseString);
				currentMatch = parser.returnCollection[0];
				LoadHeader();
			}
			else
			{
				//we need to add the new properties to the existing MatchItem.
				MatchItem sessionMatchItem = currentMatch;
				ServerParser<MatchItem> parser = new ServerParser<MatchItem>(responseString);
				MatchItem mergeMatchItem = parser.returnCollection[0];
				Type type = typeof(MatchItem);
				FieldInfo[] fieldInfos = type.GetFields();
				foreach (FieldInfo field in fieldInfos)
				{
					object value = field.GetValue(mergeMatchItem);
					if (value != null)
					{
						field.SetValue(sessionMatchItem, value);
					}
				}
			}

			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((long)currentMatch.MatchDate).ToLocalTime();
			MatchDate.Text = LangEnglish.Matched + ": " + dt.ToString("dd MMMM yyyy HH:mm");
			if (!(currentMatch.UnmatchDate is null))
			{
				dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((long)currentMatch.UnmatchDate).ToLocalTime();
				UnmatchDate.Text = LangEnglish.Unmatched + ": " + dt.ToString("dd MMMM yyyy HH:mm");
				MenuFriend.Hidden = true;
			}
			else
			{
				UnmatchDate.Text = "";
				MenuFriend.Hidden = false;
			}

			if ((bool)currentMatch.Active)
			{
				if ((bool)Session.UseLocation && c.IsLocationEnabled())
				{
					MenuLocationUpdates.Hidden = false;
				}
				else
				{
					MenuLocationUpdates.Hidden = true;
				}
				ChatEditMessage.UserInteractionEnabled = true;
				ChatSendMessage.Enabled = true;
				ChatSendMessage.Alpha = 1;
			}
			else
			{
				MenuLocationUpdates.Hidden = true;
				ChatEditMessage.UserInteractionEnabled = false;
				ChatSendMessage.Enabled = false;
				ChatSendMessage.Alpha = 0.5f;
			}

            if (tap is null)
            {
				tap = new CustomTapNoDelay(this, ChatViewProfile); //will work even if the acoount is inactive.
				ChatViewProfile.AddGestureRecognizer(tap);
			}			

			if (!(bool)currentMatch.Friend)
			{
				MenuFriend.SetTitle(LangEnglish.MenuAddFriend, UIControlState.Normal);
			}
			else
			{
				MenuFriend.SetTitle(LangEnglish.MenuRemoveFriend, UIControlState.Normal);
			}

			messageItems = new List<MessageItem>();
			if (currentMatch.Chat.Length != 0)
			{
				NoMessages.Hidden = true;
				foreach (string messageItem in currentMatch.Chat)
				{
					AddMessageItem(messageItem);
				}

				ChatMessageWindowAdapter adapter = new ChatMessageWindowAdapter(messageItems);
				ChatMessageWindow.Source = adapter;
				ChatMessageWindow.ReloadData();

				ScrollToBottom(false);
			}
			else
			{
				NoMessages.Hidden = false;
				ChatMessageWindowAdapter adapter = new ChatMessageWindowAdapter(messageItems);
				ChatMessageWindow.Source = adapter;
				ChatMessageWindow.ReloadData();
			}
		}

		private void  AddMessageItem(string messageItem)
        {
			int sep1Pos = messageItem.IndexOf('|');
			int sep2Pos = messageItem.IndexOf('|', sep1Pos + 1);
			int sep3Pos = messageItem.IndexOf('|', sep2Pos + 1);
			int sep4Pos = messageItem.IndexOf('|', sep3Pos + 1);
			int sep5Pos = messageItem.IndexOf('|', sep4Pos + 1);

			MessageItem item = new MessageItem
			{
				MessageID = int.Parse(messageItem.Substring(0, sep1Pos)),
				SenderID = int.Parse(messageItem.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1)),
				SentTime = long.Parse(messageItem.Substring(sep2Pos + 1, sep3Pos - sep2Pos - 1)),
				SeenTime = long.Parse(messageItem.Substring(sep3Pos + 1, sep4Pos - sep3Pos - 1)),
				ReadTime = long.Parse(messageItem.Substring(sep4Pos + 1, sep5Pos - sep4Pos - 1)),
				Content = CommonMethods.UnescapeBraces(messageItem.Substring(sep5Pos + 1))
			};
			messageItems.Add(item);
		}

		public void AddMessageItemOne(string messageItem)
		{
			NoMessages.Hidden = true;
			AddMessageItem(messageItem);

			ChatMessageWindowAdapter adapter = new ChatMessageWindowAdapter(messageItems);
			ChatMessageWindow.Source = adapter;
			ChatMessageWindow.ReloadData();

			ScrollToBottom(true);
		}

		public void UpdateMessageItem(string meta) // MessageID|SentTime|SeenTime|ReadTime 
		{
			//situation: sending two chats at the same time.
			//both parties will be their message first (it is faster to get a response from a server than the server sending a cloud message to the recipient)
			//but for one person their message is actually the second.
			//if someone sends 2 messages within 2 seconds, the tags may be the same. What are the consequences? In practice it is not a situation we have to deal with.

			int sep1Pos = meta.IndexOf('|');
			int sep2Pos = meta.IndexOf('|', sep1Pos + 1);
			int sep3Pos = meta.IndexOf('|', sep2Pos + 1);

			int messageID = int.Parse(meta.Substring(0, sep1Pos));
			long sentTime = long.Parse(meta.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1));
			long seenTime = long.Parse(meta.Substring(sep2Pos + 1, sep3Pos - sep2Pos - 1));
			long readTime = long.Parse(meta.Substring(sep3Pos + 1));

			int messageIndex = messageID - 1;

			if (messageIndex >= messageItems.Count) //message exists
			{
				earlyDelivery = meta;
				return;
			}
			MessageItem item = messageItems[messageIndex];

			if (item.MessageID == messageID) //normal case
			{
				item.SentTime = sentTime;
				item.SeenTime = seenTime;
				item.ReadTime = readTime;
			}
			else //two messages were sent at the same time from both parties, and for one, the order of the two messages may be the other way, if the server response was faster than google cloud.
			{
				messageIndex = messageIndex - 1;
				item = messageItems[messageIndex];
				if (item.MessageID == messageID)
				{
					item.SentTime = sentTime;
					item.SeenTime = seenTime;
					item.ReadTime = readTime;
				}
			}

			ChatMessageWindowAdapter adapter = new ChatMessageWindowAdapter(messageItems);
			ChatMessageWindow.Source = adapter;
			ChatMessageWindow.ReloadData();
		}

		public void UpdateStatus(int senderID, bool active, long? unmatchDate)
		{
			if (senderID == currentMatch.TargetID)
			{
				currentMatch.Active = active;
				currentMatch.UnmatchDate = unmatchDate;

				if (!(unmatchDate is null))
				{
					DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((long)currentMatch.UnmatchDate).ToLocalTime();
					UnmatchDate.Text = LangEnglish.Unmatched + ": " + dt.ToString("dd MMMM yyyy HH:mm");
					MenuFriend.Hidden = true;
				}
				else
				{
					UnmatchDate.Text = "";
					MenuFriend.Hidden = false;
				}

				if (active)
				{
					MenuLocationUpdates.Hidden = false;
					ChatEditMessage.UserInteractionEnabled = true;
					ChatSendMessage.Enabled = true;
					ChatSendMessage.Alpha = 1;
				}
				else
				{
					MenuLocationUpdates.Hidden = true;
					ChatEditMessage.UserInteractionEnabled = false;
					ChatSendMessage.Enabled = false;
					ChatSendMessage.Alpha = 0.5f;
				}
			}
		}

		private async void ChatSendMessage_Click(object sender, EventArgs e)
		{
			string message = ChatEditMessage.Text;
			View.EndEditing(true);

			if (message.Length != 0)
			{
				ChatSendMessage.Enabled = false; //to prevent mulitple clicks
				ChatSendMessage.Alpha = 0.5f;
				string responseString = await c.MakeRequest("action=sendmessage&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&MatchID=" + currentMatch.MatchID + "&message=" + c.UrlEncode(message));
				if (responseString.Substring(0, 2) == "OK")
				{
					ChatEditMessage.Text = "";

					string messageItem;
					if (earlyDelivery is null)
					{
						responseString = responseString.Substring(3);
						int sep1Pos = responseString.IndexOf("|");
						int sep2Pos = responseString.IndexOf("|", sep1Pos + 1);
						int messageID = int.Parse(responseString.Substring(0, sep1Pos));
						long sentTime = long.Parse(responseString.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1));
						string newRate = responseString.Substring(sep2Pos + 1);
						messageItem = messageID + "|" + Session.ID + "|" + sentTime + "|0|0|" + message;
						if (newRate != "")
						{
							Session.ResponseRate = float.Parse(newRate, CultureInfo.InvariantCulture);
						}
					}
					else
					{
						messageItem = earlyDelivery + "|" + message;
						earlyDelivery = null;
					}

                    AddMessageItemOne(messageItem);
				}
				else if (responseString.Substring(0, 6) == "ERROR_")
				{
					c.Snack(c.GetLang(responseString.Substring(6)));
				}
				else
				{
					c.ReportError(responseString);
				}
				ChatSendMessage.Enabled = true;
				ChatSendMessage.Alpha = 1f;
			}
		}

		private void ChatOneBack_Click(object sender, EventArgs e)
		{
			CommonMethods.OpenPage(null, 0); 
		}

        public void ScrollToBottom(bool animated)
        {
			ChatMessageWindow.ScrollToRow(NSIndexPath.FromRowSection(messageItems.Count - 1, 0), UITableViewScrollPosition.Bottom, animated);
		}

		private void ChangeSettings()
		{
			c.DisplayCustomDialog("", LangEnglish.ChangeUpdateCriteria, LangEnglish.DialogYes, LangEnglish.DialogNo, async alert =>
			{
				string responseString = await c.MakeRequest("action=updatesettings&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&InAppLocationRate=60");
				if (responseString.Substring(0, 2) == "OK")
				{
					if (responseString.Length > 2) //a change happened
					{
						c.LogActivity("ChatOne changed settings: " + responseString);
						c.LoadCurrentUser(responseString);
						StartRealTimeLocation();
					}
				}
				else
				{
					c.ReportError(responseString);
				}
			}, null);
		}

		private void StartRealTimeLocation()
		{
			if ((bool)currentMatch.Friend)
			{
				if (Session.LocationShare < 1)
				{
					c.Snack(LangEnglish.EnableLocationLevelFriend.Replace("[name]", currentMatch.TargetName));
					return;
				}
			}
			else
			{
				if (Session.LocationShare < 2)
				{
					c.Snack(LangEnglish.EnableLocationLevelMatch.Replace("[name]", currentMatch.TargetName)
						.Replace("[sex]", (currentMatch.Sex == 0) ? LangEnglish.SexHer : LangEnglish.SexHim));
					return;
				}
			}
			AddUpdatesTo((int)currentMatch.TargetID);
			MenuLocationUpdates.SetTitle(LangEnglish.MenuStopLocationUpdates, UIControlState.Normal);

            if (locMgr is null) //should not happen, but for security
            {
				locMgr = new LocationManager(this);
				locMgr.StartLocationUpdates();
			}
			c.Snack(LangEnglish.LocationUpdatesToStart);
		}

		private void StopRealTimeLocation()
		{
			RemoveUpdatesTo((int)currentMatch.TargetID);
			MenuLocationUpdates.SetTitle(LangEnglish.MenuStartLocationUpdates, UIControlState.Normal);
			c.Snack(LangEnglish.LocationUpdatesToEnd);
			EndLocationShare((int)currentMatch.TargetID);
		}

		private async void AddFriend()
		{
			long unixTimestamp = c.Now();
			if (!(bool)currentMatch.Friend)
			{
				string responseString = await c.MakeRequest("action=addfriend&ID=" + Session.ID + "&target=" + currentMatch.TargetID
		+ "&time=" + unixTimestamp + "&SessionID=" + Session.SessionID);
				if (responseString == "OK")
				{
					currentMatch.Friend = true;
					c.Snack(LangEnglish.FriendAdded);
					MenuFriend.SetTitle(LangEnglish.MenuRemoveFriend, UIControlState.Normal);
				}
				else
				{
					c.ReportError(responseString);
				}
			}
			else
			{
				string responseString = await c.MakeRequest("action=removefriend&ID=" + Session.ID + "&target=" + currentMatch.TargetID
		+ "&time=" + unixTimestamp + "&SessionID=" + Session.SessionID);
				if (responseString == "OK")
				{
					currentMatch.Friend = false;
					c.Snack(LangEnglish.FriendRemoved);
					MenuFriend.SetTitle(LangEnglish.MenuAddFriend, UIControlState.Normal);
				}
				else
				{
					c.ReportError(responseString);
				}
			}
		}

		private void Unmatch()
		{
			string displayText;
			if (currentMatch.TargetID == 0)
			{
				displayText = LangEnglish.DialogUnmatchDeleted;
			}
			else
			{
				displayText = (currentMatch.UnmatchDate is null) ? LangEnglish.DialogUnmatchMatched : LangEnglish.DialogUnmatchUnmatched;
				displayText = displayText.Replace("[name]", currentMatch.TargetName);
				displayText = displayText.Replace("[sex]", (currentMatch.Sex == 0) ? LangEnglish.SexShe : LangEnglish.SexHe);
			}

			c.DisplayCustomDialog(LangEnglish.ConfirmAction, displayText, LangEnglish.DialogOK, LangEnglish.DialogCancel, async alert => {
				if (IsUpdatingTo((int)currentMatch.TargetID))
				{
					RemoveUpdatesTo((int)currentMatch.TargetID);
				}
				if (IsUpdatingFrom((int)currentMatch.TargetID))
				{
					RemoveUpdatesFrom((int)currentMatch.TargetID);
				}

				long unixTimestamp = c.Now();
				string responseString = await c.MakeRequest("action=unmatch&ID=" + Session.ID + "&target=" + currentMatch.TargetID
					+ "&time=" + unixTimestamp + "&SessionID=" + Session.SessionID);
				if (responseString == "OK")
				{
					if (!(ListActivity.listProfiles is null))
					{
						foreach (Profile item in ListActivity.listProfiles)
						{
							if (item.ID == currentMatch.TargetID)
							{
								item.UserRelation = 0;
							}
						}
					}
					if (!(ListActivity.viewProfiles is null))
					{
						foreach (Profile item in ListActivity.viewProfiles)
						{
							if (item.ID == currentMatch.TargetID)
							{
								item.UserRelation = 0;
							}
						}
					}
					currentMatch = null;
					CommonMethods.OpenPage("ChatListActivity", 1);
				}
				else
				{
					c.ReportError(responseString);
				}
			}, null);
		}
	}
}