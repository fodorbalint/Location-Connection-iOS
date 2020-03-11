using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
	public class BaseActivity : UIViewController
	{
		public string deviceTokenFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "devicetoken.txt");
		public string tokenUptoDateFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "tokenuptodate.txt");

		public CommonMethods c;

		public static bool isAppForeground;
		public static bool locationUpdating; //used only for logging
		public static LocationManager locMgr;

		public static bool firstLocationAcquired;
		public static string locationUpdatesTo;
		public static string locationUpdatesFrom;

		public static float screenWidth;
		public static float screenHeight;
		public static float pixelDensity;
		protected static float XPxPerIn;
		protected static float XDpPerIn;
		public static float DpWidth;
		public static float DpHeight;
		public nfloat roundBottomHeight;
		public nfloat uselessHeight;
		public bool layoutSet;

		NSObject hideNotification, frameChangeNotification;
        public nfloat keyboardHeight = 0;

		public UIView RoundBottom_Base { get; set; }
		public Snackbar Snackbar_Base { get; set; }
		public NSLayoutConstraint BottomConstraint_Base { get; set; }
		public NSLayoutConstraint SnackTopConstraint_Base { get; set; } //used when ResizeWindowOnKeyboard() is used
		public NSLayoutConstraint SnackBottomConstraint_Base { get; set; }
		public NSLayoutConstraint ScrollBottomConstraint_Base { get; set; }
		public NSLayoutConstraint LoaderCircleLeftConstraint_Base { get; set; }
		public NSLayoutConstraint ChatOneLeftConstraint_Base { get; set; }
		public NSLayoutConstraint ChatOneRightConstraint_Base { get; set; }
        public UIScrollView ChatMessageWindow_Base { get; set; }

		private nfloat ScrollBottomConstraintConstant;
		private nfloat BottomConstraintConstant;
		private nfloat SnackBottomConstraintConstant;

		public bool rippleRunning;
		public float tweenTime = 0.2f;

		public BaseActivity(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			c = new CommonMethods(this);

			if (c.IsLoggedIn())
			{
				CheckIntent();
			}

            if (!(this is ProfileViewActivity) && !(this is LocationActivity) && !(this is ChatListActivity))
            {
				var tap0 = new UITapGestureRecognizer(); //If CancelsTouchesInView is set to false, the buttons won't work at first click 
				tap0.AddTarget(() => DismissKeyboard(tap0));
				View.AddGestureRecognizer(tap0);
			}			

			c.LogActivity("ViewDidLoad " + Class.Name);
			c.CW("ViewDidLoad " + Class.Name);

		}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

			c.CW("ViewWillAppear " + this.Class.Name);
			ResizeWindowOnKeyboard();

			/* On ProfileView, Snackbar is shown and hid instantly. No solution found.
            
			For testing in ShowSnack:
            Stopwatch stw = new Stopwatch();
			stw.Start();
			CW("ShowSnack start tweentime " + context.tweenTime);

            stw.Stop(); CW("ShowSnack end " + stw.ElapsedMilliseconds);
            */
			if (!(Session.SnackMessage is null))
			{
				if (this is ChatOneActivity)
				{
					c.Snack(Session.SnackMessage.Replace("[name]", Session.CurrentMatch.TargetName));
				}
				else
				{
					c.Snack(Session.SnackMessage);
				}
				Session.SnackMessage = null;
			}
		}

		public override void ViewWillDisappear(bool animated)
		{
			c.CW("ViewWillDisappear " + this.Class.Name);
			base.ViewWillDisappear(animated);

			if (frameChangeNotification != null)
			{
				frameChangeNotification.Dispose();
				hideNotification.Dispose();
			}
		}

		public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            //c.LogActivity("ViewDidLayoutSubviews " + Class.Name);
			//c.CW("ViewDidLayoutSubviews " + Class.Name);

			if (!layoutSet)
			{
				roundBottomHeight = View.Frame.Height - (View.SafeAreaLayoutGuide.LayoutFrame.Y + View.SafeAreaLayoutGuide.LayoutFrame.Height);
				uselessHeight = 13;

				if (roundBottomHeight > 0)
				{
					if (this is ProfileViewActivity)
					{
						BottomConstraintConstant = 10 + uselessHeight;
						ScrollBottomConstraintConstant = 10 + uselessHeight - roundBottomHeight;
					}
                    else if (this is ChatOneActivity) //12 dp insets
					{
						BottomConstraintConstant = 2 + uselessHeight;
						ChatOneLeftConstraint_Base.Constant = -12;
						ChatOneRightConstraint_Base.Constant = 8; 
					}
                    else
                    {
						BottomConstraintConstant = uselessHeight;
                        if (this is RegisterActivity || this is ProfileEditActivity || this is SettingsActivity)
                        {
							ScrollBottomConstraintConstant = 10 + uselessHeight - roundBottomHeight; //scrollbar will scroll more, but full scroll is truncated.
						}
                        else if (this is ListActivity)
                        {
							LoaderCircleLeftConstraint_Base.Constant = -10;
						}
					}
					SnackBottomConstraintConstant = 0;
				}
                else
                {
                    if (this is ProfileViewActivity)
                    {
						ScrollBottomConstraintConstant = 0;
					}
					else if (this is RegisterActivity || this is ProfileEditActivity || this is SettingsActivity)
                    {
						ScrollBottomConstraintConstant = 10;
					}
					BottomConstraintConstant = 0;
					SnackBottomConstraintConstant = -uselessHeight; //Snackbar is extra padded at the bottom
				}

				//c.CW("ViewDidLayoutSubviews roundBottomHeight " + BottomConstraint_Base + " " + roundBottomHeight + " " + BottomConstraintConstant + " " + SnackBottomConstraintConstant + " " + ScrollBottomConstraintConstant);

				BottomConstraint_Base.Constant = BottomConstraintConstant;
				SnackBottomConstraint_Base.Constant = SnackBottomConstraintConstant;
                if (ScrollBottomConstraint_Base != null)
                {
					ScrollBottomConstraint_Base.Constant = ScrollBottomConstraintConstant;
				}
			}

			layoutSet = true;
		}

        public void ResizeWindowOnKeyboard()
        {
			frameChangeNotification = UIKeyboard.Notifications.ObserveWillChangeFrame((sender, args) => {
				if (SnackBottomConstraint_Base != null && SnackTopConstraint_Base != null)
				{
					nfloat newKeyboardHeight = ((NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey))).RectangleFValue.Size.Height; //changes from 320 to 265 when in Settings, I click on See log while keyboard is open. Framechangenotification is also called when keyboard is closed, having its height for opened state.

					if (keyboardHeight != newKeyboardHeight)
                    {
						nfloat diff = 0;
						nfloat chatWindowNewHeight = 0;

						if (this is ChatOneActivity)
                        {
							diff = newKeyboardHeight - keyboardHeight;
							chatWindowNewHeight = ChatMessageWindow_Base.Bounds.Size.Height - diff;
						}

						BottomConstraint_Base.Constant = newKeyboardHeight;
						SnackBottomConstraint_Base.Constant = newKeyboardHeight - uselessHeight;
						SnackTopConstraint_Base.Constant = newKeyboardHeight;
						if (ScrollBottomConstraint_Base != null)
						{
							ScrollBottomConstraint_Base.Constant = 10;
						}

						NSNumber duration = (NSNumber)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.AnimationDurationUserInfoKey));
						UIView.Animate(duration.DoubleValue, () => { View.LayoutIfNeeded(); }, null);

						if (this is ChatOneActivity)
						{
							CGPoint offset = ChatMessageWindow_Base.ContentOffset;

							if (ChatMessageWindow_Base.ContentSize.Height > ChatMessageWindow_Base.Bounds.Size.Height) //if already scrollable
							{
								offset.Y += diff;
							}
							else if (ChatMessageWindow_Base.ContentSize.Height <= ChatMessageWindow_Base.Bounds.Size.Height && ChatMessageWindow_Base.ContentSize.Height > chatWindowNewHeight) //if becomes scrollable
							{
								offset.Y = ChatMessageWindow_Base.ContentSize.Height - ChatMessageWindow_Base.Bounds.Size.Height + diff;
							}

							ChatMessageWindow_Base.SetContentOffset(offset, false);
						}
						else if (this is SettingsActivity)
						{
							c.ScrollToBottom(((SettingsActivity)this).SettingsScroll);
						}

						keyboardHeight = newKeyboardHeight;
					}					
				}
			});

			hideNotification = UIKeyboard.Notifications.ObserveWillHide((sender, args) => {
                if (SnackBottomConstraint_Base != null && SnackTopConstraint_Base != null) //can become null when opening a new page while keyboard is open
				{
					keyboardHeight = 0;

					BottomConstraint_Base.Constant = BottomConstraintConstant;
					SnackBottomConstraint_Base.Constant = SnackBottomConstraintConstant;
					SnackTopConstraint_Base.Constant = 0;
					if (ScrollBottomConstraint_Base != null)
					{
						ScrollBottomConstraint_Base.Constant = ScrollBottomConstraintConstant;
					}

					NSNumber duration = (NSNumber)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.AnimationDurationUserInfoKey));
					UIView.Animate(duration.DoubleValue, () => { View.LayoutIfNeeded(); }, null);
				}				
			});
		}

		public void DismissKeyboard(UITapGestureRecognizer tap)
		{
			c.CW(" DismissKeyboard ");
			View.EndEditing(true);
		}

        protected void CheckIntent()
		{
			/*
			Key: google.delivered_priority, Value: high
			Key: google.sent_time, Value: 
			Key: google.ttl, Value: 
			Key: google.original_priority, Value: high
			Key: from, Value: 205197408276
			Key: google.message_id, Value: 0:1575318929834966%e37d5f25e37d5f25
			Key: content, Value: 33|6|1575318929|0|0|
			Key: collapse_key, Value: balintfodor.locationconnection
			*/
			/*-----
			if (c.IsLoggedIn())
			{
				if (!(Intent.Extras is null) && !(Intent.Extras.GetString("google.message_id") is null))
				{
					int sep1Pos;
					int senderID = 0;

					string type = Intent.Extras.GetString("type");
					string content = Intent.Extras.GetString("content");
					c.LogActivity("Intent received: " + type);
					switch (type)
					{
						case "sendMessage":
							sep1Pos = content.IndexOf('|');
							int sep2Pos = content.IndexOf('|', sep1Pos + 1);
							senderID = int.Parse(content.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1));
							break;
						case "matchProfile":
						case "rematchProfile":
						case "unmatchProfile":
							sep1Pos = content.IndexOf('|');
							senderID = int.Parse(content.Substring(0, sep1Pos));
							break;
					}

					Intent i = new Intent(this, typeof(ChatOneActivity));
					i.SetFlags(ActivityFlags.ReorderToFront);
					IntentData.senderID = senderID;
					StartActivity(i);
				}
			}
			*/
		}
		public void GetScreenMetrics()
		{
			screenWidth = (float)UIScreen.MainScreen.NativeBounds.Width;
			screenHeight = (float)UIScreen.MainScreen.NativeBounds.Height;
			DpWidth = (float)UIScreen.MainScreen.Bounds.Width;
			DpHeight = (float)UIScreen.MainScreen.Bounds.Height;
			pixelDensity = (float)UIScreen.MainScreen.Scale;

			c.LogActivity("ScreenWidth " + screenWidth + " ScreenHeight " + screenHeight + " PixelDensity " + pixelDensity + " DpWidth " + DpWidth + " DpHeight " + DpHeight);
		}

		public void TruncateLocationLog()
		{
            try
            {
				long unixTimestamp = c.Now();
				string[] lines = File.ReadAllLines(c.locationLogFile);
				string firstLine = lines[0];
				int sep1Pos = firstLine.IndexOf("|");
				long locationTime = long.Parse(firstLine.Substring(0, sep1Pos));
				if (locationTime < unixTimestamp - Constants.LocationKeepTime)
				{
					List<string> newLines = new List<string>();
					for (int i = 1; i < lines.Length; i++)
					{
						string line = lines[i];
						sep1Pos = line.IndexOf("|");
						locationTime = long.Parse(line.Substring(0, sep1Pos));
						if (locationTime >= unixTimestamp - Constants.LocationKeepTime)
						{
							newLines.Add(line);
						}
					}
					File.WriteAllLines(c.locationLogFile, newLines);
				}
			}
            catch (Exception ex)
            {
				c.ReportErrorSilent(ex.Message + " " + ex.StackTrace + " " + File.ReadAllText(c.locationLogFile));
            }			
		}

		public void TruncateSystemLog()
		{
			CultureInfo provider = CultureInfo.InvariantCulture;
			string format = @"yyyy-MM-dd HH\:mm\:ss.fff";
			DateTime dt = DateTime.UtcNow;

			string[] lines = File.ReadAllLines(CommonMethods.logFile);
			string firstLine = lines[0];
			int sep1Pos = firstLine.IndexOf(" ");
			int sep2Pos = firstLine.IndexOf(" ", sep1Pos + 1);
			DateTime logTime = DateTime.ParseExact(firstLine.Substring(0, sep2Pos), format, provider);

			if (dt.Subtract(logTime).TotalSeconds > Constants.SystemLogKeepTime)
			{
				List<string> newLines = new List<string>();
				for (int i = 1; i < lines.Length; i++)
				{
					string line = lines[i];
					sep1Pos = line.IndexOf(" ");
					sep2Pos = line.IndexOf(" ", sep1Pos + 1);
					logTime = DateTime.ParseExact(line.Substring(0, sep2Pos), format, provider);

					if (dt.Subtract(logTime).TotalSeconds <= Constants.SystemLogKeepTime)
					{
						newLines.Add(line);
					}
				}
				File.WriteAllLines(CommonMethods.logFile, newLines);
			}
		}

		protected void EndLocationShare(int? targetID = null)
		{
			c.CW("EndLocationShare");
			string url = "action=updatelocationend&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&LocationUpdates=";
			if (targetID is null) //stop all
			{
				url += locationUpdatesTo;
			}
			else
			{
				url += targetID;
			}
			string responseString = c.MakeRequestSync(url);
			if (responseString == "OK")
			{
			}
			else
			{
				c.ReportErrorSilent(responseString);
			}
		}

		protected bool IsUpdatingTo(int targetID)
		{
			if (string.IsNullOrEmpty(locationUpdatesTo))
			{
				return false;
			}
			string[] arr = locationUpdatesTo.Split("|");
			foreach (string ID in arr)
			{
				if (ID == targetID.ToString())
				{
					return true;
				}
			}
			return false;
		}

		protected void AddUpdatesTo(int targetID)
		{
			c.LogActivity("AddUpdatesTo locationUpdatesTo:" + locationUpdatesTo);
			if (string.IsNullOrEmpty(locationUpdatesTo))
			{
				locationUpdatesTo = targetID.ToString();
			}
			else
			{
				locationUpdatesTo += "|" + targetID;
			}
		}

		protected void RemoveUpdatesTo(int targetID)
		{
			string[] arr = locationUpdatesTo.Split("|");
			string returnStr = "";
			foreach (string ID in arr)
			{
				if (ID != targetID.ToString())
				{
					returnStr += ID + "|";
				}
			}
			if (returnStr.Length > 0)
			{
				returnStr = returnStr.Substring(0, returnStr.Length - 1);
			}
			locationUpdatesTo = returnStr;
		}

		public bool IsUpdatingFrom(int targetID)
		{
			c.LogActivity("Location update from " + targetID + ", existing: " + locationUpdatesFrom);
			if (string.IsNullOrEmpty(locationUpdatesFrom))
			{
				return false;
			}
			string[] arr = locationUpdatesFrom.Split("|");
			foreach (string ID in arr)
			{
				if (ID == targetID.ToString())
				{
					return true;
				}
			}
			return false;
		}

		public void AddUpdatesFrom(int targetID)
		{
			if (string.IsNullOrEmpty(locationUpdatesFrom))
			{
				locationUpdatesFrom = targetID.ToString();
			}
			else
			{
				locationUpdatesFrom += "|" + targetID;
			}
		}

		public void RemoveUpdatesFrom(int targetID)
		{
			string[] arr = locationUpdatesFrom.Split("|");
			string returnStr = "";
			foreach (string ID in arr)
			{
				if (ID != targetID.ToString())
				{
					returnStr += ID + "|";
				}
			}
			if (returnStr.Length > 0)
			{
				returnStr = returnStr.Substring(0, returnStr.Length - 1);
			}
			locationUpdatesFrom = returnStr;
		}
	}
}