using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using CoreAnimation;
using CoreLocation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
	public class CommonMethods
	{
		public string errorFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "error.txt");
		public static string logFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "systemlog.txt");
		public string locationLogFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "locationlog.txt");
		private string settingsFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "settings.txt");
		private string defaultSettingsFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "defaultsettings.txt");
		private string loginSessionFile = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "loginsession.txt");

		//-----Snackbar snack;
		BaseActivity context;
		public string snackPermanentText;
		private Action snackButtonAction;
		public bool snackVisible;
		private UIView MainLayout;
		private UIView SnackBar;
		private UILabel SnackText;
		private UIButton SnackButton;
		private Timer snackTimer;
		private int snackDuration = 4000;
        	
		public CommonMethods(BaseActivity context)
		{
			this.context = context;
		}

        public void AddViews(UIView SnackBar, UILabel SnackText, UIButton SnackButton)
        {
			MainLayout = context.View;
			this.SnackBar = SnackBar;
			this.SnackText = SnackText;
			this.SnackButton = SnackButton;
            this.SnackButton.TouchUpInside += SnackButton_Click;
			this.SnackBar.Hidden = true;
			snackVisible = false;
        }

		public void LoadSettings(bool defaultSettings)
		{
			if (!defaultSettings)
			{
				//Load not empty values from file, the rest will be loaded from SettingsDefault
				if (File.Exists(settingsFile))
				{
					Type type = typeof(Settings);
					string[] settingLines = (defaultSettings) ? File.ReadAllLines(defaultSettingsFile) : File.ReadAllLines(settingsFile);
					foreach (string line in settingLines)
					{
						if (line != "" && line[0] != '\'')
						{
							int pos = line.IndexOf(":");
							string key = line.Substring(0, pos);
							string value = line.Substring(pos + 1).Trim();
							if (value != "")
							{
								FieldInfo fieldInfo = type.GetField(key);
								if (!(fieldInfo is null)) //if that setting still exists in this version of the app
								{
									Type type1 = Nullable.GetUnderlyingType(fieldInfo.FieldType) ?? fieldInfo.FieldType;
									fieldInfo.SetValue(null, Convert.ChangeType(value, type1));
								}
							}
						}
					}
				}

				Type typeS = typeof(Settings);
				Type typeSDef = typeof(SettingsDefault);

				string str = "";
				FieldInfo[] fields = typeS.GetFields();
				foreach (FieldInfo field in fields)
				{
					if (field.GetValue(null) is null)
					{
						FieldInfo defField = typeSDef.GetField(field.Name);
						if (!(defField is null)) //other address data has no default.
						{
							field.SetValue(null, defField.GetValue(null));
						}
					}
					str += Environment.NewLine;
				}
			}
			else //load default settings
			{
				Type typeS = typeof(Settings);
				Type typeSDef = typeof(SettingsDefault);

				FieldInfo[] fields = typeS.GetFields();
				foreach (FieldInfo field in fields)
				{
					FieldInfo defField = typeSDef.GetField(field.Name);
					if (!(defField is null)) //other address data has no default.
					{
						field.SetValue(null, defField.GetValue(null));
					}
				}
			}
		}

		public void SaveSettings()
		{
			List<string> settingLines = new List<string>();
			Type type = typeof(Settings);
			FieldInfo[] fieldInfo = type.GetFields();
			foreach (FieldInfo field in fieldInfo)
			{
				settingLines.Add(field.Name + ": " + field.GetValue(null));
			}
			File.WriteAllLines(settingsFile, settingLines);
		}

		public Task<string> MakeRequest(string query, string method = "GET", string postData = null)
		{
			return Task.Run(() =>
			{
				Stopwatch stw = new Stopwatch();
				stw.Start();
				try
				{
					string url = Constants.HostName + "?" + query;
					//url += Constants.TestDB;
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
					request.Timeout = Constants.RequestTimeout;

					if (method == "GET")
					{
						request.Method = "GET";
					}
					else
					{
						request.Method = "POST";
						byte[] byteArray = Encoding.UTF8.GetBytes(postData);
						request.ContentType = "application/x-www-form-urlencoded";
						request.ContentLength = byteArray.Length;
						Stream dataStream = request.GetRequestStream();
						dataStream.Write(byteArray, 0, byteArray.Length);
						dataStream.Close();
					}

					var response = request.GetResponse();
					stw.Stop();
					CW(stw.ElapsedMilliseconds + " " + url);

					string data = new StreamReader(response.GetResponseStream()).ReadToEnd();
					response.Close();
					if ((url.IndexOf("ID=") != -1) && IsLoggedIn())
					{
						long unixTimestamp = Now();
						Session.LastActiveDate = unixTimestamp;
					}

                    if (snackPermanentText == LangEnglish.NoNetwork || snackPermanentText == LangEnglish.NetworkTimeout)
                    {
						context.InvokeOnMainThread(() => {
							HideSnack();
						});
					}
					return data;

				}
				catch (Exception ex)
				{
					stw.Stop();
					if (ex is WebException)
					{
						switch (((WebException)ex).Status)
						{
							case WebExceptionStatus.ConnectFailure:
							case WebExceptionStatus.NameResolutionFailure:
							case WebExceptionStatus.SecureChannelFailure:
								return "NoNetwork";
							case WebExceptionStatus.Timeout:
								if (stw.ElapsedMilliseconds > Constants.RequestTimeout)
								{
									return "NetworkTimeout";
								}
								else //app crashed, and underlying activity was called
								{
									return ex.Message;
								}
							default:
								return ex.Message;
						}
					}
					else
					{
						return ex.Message;
					}
				}
			});
		}

		public string MakeRequestSync(string query, string method = "GET", string postData = null)
		{
			Stopwatch stw = new Stopwatch();
			stw.Start();
			try
			{
				string url = Constants.HostName + "?" + query;
				//url += Constants.TestDB;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Timeout = Constants.RequestTimeout;

				if (method == "GET")
				{
					request.Method = "GET";
				}
				else
				{
					request.Method = "POST";
					byte[] byteArray = Encoding.UTF8.GetBytes(postData);
					request.ContentType = "application/x-www-form-urlencoded";
					request.ContentLength = byteArray.Length;
					Stream dataStream = request.GetRequestStream();
					dataStream.Write(byteArray, 0, byteArray.Length);
					dataStream.Close();
				}
				var response = request.GetResponse();

				stw.Stop();
				CW(stw.ElapsedMilliseconds + " " + url);

				string data = new StreamReader(response.GetResponseStream()).ReadToEnd();
				response.Close();
				if ((url.IndexOf("ID=") != -1) && IsLoggedIn())
				{
					long unixTimestamp = Now();
					Session.LastActiveDate = unixTimestamp;
				}
				if (snackPermanentText == LangEnglish.NoNetwork || snackPermanentText == LangEnglish.NetworkTimeout)
				{
					context.InvokeOnMainThread(() => {
						HideSnack();
					});
				}
				return data;
			}
			catch (Exception ex)
			{
				stw.Stop();
				if (ex is WebException)
				{
					switch (((WebException)ex).Status)
					{
						case WebExceptionStatus.ConnectFailure:
						case WebExceptionStatus.NameResolutionFailure:
						case WebExceptionStatus.SecureChannelFailure:
							return "NoNetwork";
						case WebExceptionStatus.Timeout:
							if (stw.ElapsedMilliseconds > Constants.RequestTimeout)
							{
								return "NetworkTimeout";
							}
							else //app crashed, and underlying activity was called
							{
								return ex.Message;
							}
						default:
							return ex.Message;
					}
				}
				else
				{
					return ex.Message;
				}
			}
		}

		public void LoadCurrentUser(string responseString)
		{
			responseString = responseString.Substring(3);
			ServerParser<Session> parser = new ServerParser<Session>(responseString);
			File.WriteAllText(loginSessionFile, Session.ID + ";" + Session.SessionID);
		}

		public void ClearCurrentUser()
		{
			Type type = typeof(Session);
			FieldInfo[] fieldInfo = type.GetFields();
			foreach (FieldInfo field in fieldInfo)
			{
				field.SetValue(null, null);
			}

			if (File.Exists(loginSessionFile))
			{
				File.Delete(loginSessionFile);
			}
		}

		public bool IsLoggedIn()
		{
			return !string.IsNullOrEmpty(Session.SessionID);
		}

		public bool IsLocationEnabled()
		{
			if (CLLocationManager.LocationServicesEnabled)
			{
				switch(CLLocationManager.Status) {
					case CLAuthorizationStatus.AuthorizedAlways:
					case CLAuthorizationStatus.AuthorizedWhenInUse:
						return true;
				}
				return false;
			}
			return false;
		}

		public bool IsOwnLocationAvailable()
		{
			return Session.Latitude != null && Session.Longitude != null;
		}

		public bool IsOtherLocationAvailable()
		{
			return Session.OtherLatitude != null && Session.OtherLongitude != null;
		}

		public async Task<bool> UpdateLocationSync()
		{
			string url = "action=updatelocation&ID=" + Session.ID + "&SessionID=" + Session.SessionID
						+ "&Latitude=" + ((double)Session.Latitude).ToString(CultureInfo.InvariantCulture) + "&Longitude=" + ((double)Session.Longitude).ToString(CultureInfo.InvariantCulture) + "&LocationTime=" + Session.LocationTime + "&Background=" + !BaseActivity.isAppForeground;
			if (!string.IsNullOrEmpty(BaseActivity.locationUpdatesTo))
			{
				url += "&LocationUpdates=" + BaseActivity.locationUpdatesTo + "&Frequency=" + Session.InAppLocationRate;
				/*if (Session.InAppLocationRate == 0)
				{
					LogActivity("Error: location update rate is 0.");
				}*/
			}

			string responseString = await MakeRequest(url);
			if (responseString == "OK")
			{
				return true;
			}
			else
			{
				if (BaseActivity.isAppForeground)
				{
					//When logging in from another device, program crashes:
					// 'Attempt to invoke virtual method 'android.content.res.Resources android.view.View.getResources()' on a null object reference'
					if (responseString == "AUTHORIZATION_ERROR")
					{
						IntentData.logout = true;
						IntentData.authError = true;
						OpenPage("MainActivity", 1);
					}
					else
					{
						Console.WriteLine("-----------location could not be updated, responsestring-----------" + responseString);
						/*context.RunOnUiThread(() => { //When updating from the location provider, only the caller activity can display a snack until it is paused.
							Snack(Resource.String.LocationNoUpdate, null);
						});*/
					}
				}
				else if (responseString == "AUTHORIZATION_ERROR")
				{
					BaseActivity.locMgr.StopLocationUpdates();
				}
				return false;
			}
		}

		public string GetTimeDiffStr(long? pastTime, bool isShort)
		{
			long unixTimestamp = Now();
			if (pastTime == unixTimestamp)
			{
				if (isShort)
				{
					return LangEnglish.Now;
				}
				else
				{
					return LangEnglish.NowSmall;
				}
			}
			StackTrace stackTrace = new StackTrace();
			TimeSpan ts = TimeSpan.FromSeconds((long)(unixTimestamp - pastTime));

			string day = LangEnglish.Day;
			string days = LangEnglish.Days;
			string hour = LangEnglish.Hour;
			string hours = LangEnglish.Hours;
			string min, mins, sec, secs;
			if (isShort)
			{
				min = LangEnglish.ShortMinute;
				mins = LangEnglish.ShortMinutes;
				sec = LangEnglish.ShortSecond;
				secs = LangEnglish.ShortSeconds;
			}
			else
			{
				min = LangEnglish.Minute;
				mins = LangEnglish.Minutes;
				sec = LangEnglish.Second;
				secs = LangEnglish.Seconds;
			}

			string str = "";
			bool showHours = true;
			bool showMinutes = true;
			bool showSeconds = true;
			if (ts.Days > 1)
			{
				str += ts.Days + " " + days + " ";
				showHours = false;
				showMinutes = false;
				showSeconds = false;
			}
			else if (ts.Days > 0)
			{
				str += ts.Days + " " + day + " ";
				showMinutes = false;
				showSeconds = false;
			}

			if (showHours)
			{
				if (ts.Hours > 1)
				{
					str += ts.Hours + " " + hours + " ";
					showMinutes = false;
					showSeconds = false;
				}
				else if (ts.Hours > 0)
				{
					str += ts.Hours + " " + hour + " ";
					showSeconds = false;
				}
			}
			else
			{
				showMinutes = false;
				showSeconds = false;
			}

			if (showMinutes)
			{
				if (ts.Minutes > 1)
				{
					str += ts.Minutes + " " + mins + " ";
					showSeconds = false;
				}
				else if (ts.Minutes > 0)
				{
					str += ts.Minutes + " " + min + " ";
				}
			}
			else
			{
				showSeconds = false;
			}

			if (showSeconds)
			{
				if (ts.Seconds > 1)
				{
					str += ts.Seconds + " " + secs + " ";
				}
				else if (ts.Seconds > 0)
				{
					str += ts.Seconds + " " + sec + " ";
				}
			}
			if (str == "") //pastTime can be +1 compared to now, resulting in ts.Seconds = -1
			{
				if (isShort)
				{
					return LangEnglish.Now;
				}
				else
				{
					return LangEnglish.NowSmall;
				}
			}
			str += LangEnglish.Ago;
			return str.Replace(" ", "\u00A0"); //non-breaking space
		}

		public void Snack(string message)
		{
			SnackText.Text = message;
			CollapseButton();
			MainLayout.LayoutIfNeeded();

            if (snackVisible)
            {
                if (!(snackTimer is null))
                {
					snackTimer.Stop();
					snackTimer.Start();
				}
                else
                {
					snackTimer = new Timer();
					snackTimer.Elapsed += Timer_Elapsed;
					snackTimer.Interval += snackDuration;
					snackTimer.Start();
				}
			}
            else
            {
				ShowSnack();

				snackTimer = new Timer();
				snackTimer.Elapsed += Timer_Elapsed;
				snackTimer.Interval += snackDuration;
				snackTimer.Start();
			}			
		}

		public void SnackAction(string message, string actionText, Action action)
		{
			SnackText.Text = message;
			ExpandButton();
			SnackButton.SetTitle(actionText, UIControlState.Normal);
            MainLayout.LayoutIfNeeded();
			
			snackButtonAction = action;

			if (snackVisible)
			{
				if (!(snackTimer is null))
				{
					snackTimer.Stop();
					snackTimer.Start();
				}
				else
				{
					snackTimer = new Timer();
					snackTimer.Elapsed += Timer_Elapsed;
					snackTimer.Interval += snackDuration;
					snackTimer.Start();
				}
			}
			else
			{
				ShowSnack();

				snackTimer = new Timer();
				snackTimer.Elapsed += Timer_Elapsed;
				snackTimer.Interval += snackDuration;
				snackTimer.Start();
			}
		}

		public void SnackIndef(string message)
		{
			snackPermanentText = message;
			SnackText.Text = message;
			ExpandButton();
			SnackButton.SetTitle(LangEnglish.SnackOK, UIControlState.Normal);
			MainLayout.LayoutIfNeeded();

            if (snackVisible)
            {
				if(!(snackTimer is null))
				{
					snackTimer.Stop();
				}
			}
            else
            {
				ShowSnack();
			}
		}

		private void SnackButton_Click(object sender, EventArgs e)
		{
			HideSnack();

            if (snackButtonAction != null)
            {
				snackButtonAction.Invoke();
				snackButtonAction = null;
            }
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			snackTimer.Stop();
			context.InvokeOnMainThread(() =>
			{
				HideSnack();
			});
		}

		public void ShowSnack()
		{
			this.SnackBar.Hidden = false;
			snackVisible = true;

			UIView.Animate(duration: context.tweenTime * 2, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
			{
				foreach (NSLayoutConstraint constraint in MainLayout.Constraints)
				{
					if (constraint.SecondItem == SnackBar && constraint.SecondAttribute == NSLayoutAttribute.Top)
					{
						constraint.Priority = 198;
					}
				}
				MainLayout.LayoutIfNeeded();
			}, completion: () => { });
		}

		public void HideSnack()
		{
			snackVisible = false;
			snackPermanentText = "";

			UIView.Animate(duration: context.tweenTime * 2, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
			{
				foreach (NSLayoutConstraint constraint in MainLayout.Constraints)
				{
					if (constraint.SecondItem == SnackBar && constraint.SecondAttribute == NSLayoutAttribute.Top)
					{
						constraint.Priority = 200;
					}
				}
				MainLayout.LayoutIfNeeded();
			}, completion: () => { if (!snackVisible) this.SnackBar.Hidden = true; }); //a new snackbar could have appeared during animation
		}

		public void DisplayCustomDialog(string dialogTitle, string dialogMessage, string dialogPositiveBtnLabel, string dialogNegativeBtnLabel, Action<UIAlertAction> actionPositive, Action<UIAlertAction> actionNegative)
		{
			UIAlertController dialog = UIAlertController.Create(dialogTitle, dialogMessage, UIAlertControllerStyle.Alert);

			var paragraphStyle = new NSMutableParagraphStyle();
			paragraphStyle.Alignment = UITextAlignment.Left;

			var messageText = new NSAttributedString(dialogMessage, paragraphStyle: paragraphStyle, foregroundColor: UIColor.Black, font: UIFont.SystemFontOfSize(14));

			dialog.SetValueForKey(messageText, new NSString("attributedMessage"));
			dialog.AddAction(UIAlertAction.Create(dialogNegativeBtnLabel, UIAlertActionStyle.Default, actionNegative));
			dialog.AddAction(UIAlertAction.Create(dialogPositiveBtnLabel, UIAlertActionStyle.Default, actionPositive));
			context.PresentViewController(dialog, true, null);
		}

		public void ActionAlert(string dialogTitle, string dialogMessage, string action1label, string action2label, string action3label, string action4label, Action<UIAlertAction> action1, Action<UIAlertAction> action2, Action<UIAlertAction> action3, Action<UIAlertAction> action4, UIView sourceView)
		{
			UIAlertController dialog = UIAlertController.Create(dialogTitle, dialogMessage, UIAlertControllerStyle.ActionSheet);

			var paragraphStyle = new NSMutableParagraphStyle();
			paragraphStyle.Alignment = UITextAlignment.Left;

			var messageText = new NSAttributedString(dialogMessage, paragraphStyle: paragraphStyle, foregroundColor: UIColor.Black, font: UIFont.SystemFontOfSize(14));
				
			dialog.SetValueForKey(messageText, new NSString("attributedMessage"));

			dialog.AddAction(UIAlertAction.Create(action1label, UIAlertActionStyle.Default, action1));
            dialog.AddAction(UIAlertAction.Create(action2label, UIAlertActionStyle.Default, action2));
			dialog.AddAction(UIAlertAction.Create(action3label, UIAlertActionStyle.Default, action3));
			dialog.AddAction(UIAlertAction.Create(action4label, UIAlertActionStyle.Default, action4));

			UIPopoverPresentationController presentationPopover = dialog.PopoverPresentationController;
			if (presentationPopover != null)
			{
				presentationPopover.SourceView = sourceView;
				presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Any;
			}

			context.PresentViewController(dialog,true, null);
		}

		public void Alert(string message)
		{
			UIAlertController dialog = UIAlertController.Create("", message, UIAlertControllerStyle.Alert);
			dialog.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
			context.PresentViewController(dialog, true, null);
		}

		public void AlertLinks(string message) //dialog layout may change in a new software version
		{
			UIAlertController dialog = UIAlertController.Create("", message, UIAlertControllerStyle.Alert);
			dialog.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

			try
            {
				UILabel label = (UILabel)dialog.View.Subviews[0].Subviews[0].Subviews[0].Subviews[0].Subviews[0].Subviews[2];
				label.Hidden = true;				

				UITextView textView = new UITextView();
				dialog.View.Subviews[0].Subviews[0].Subviews[0].Subviews[0].Subviews[0].InsertSubviewAbove(textView, label);

				textView.TranslatesAutoresizingMaskIntoConstraints = false;
				textView.TopAnchor.ConstraintEqualTo(label.TopAnchor, -7).Active = true;
                textView.LeadingAnchor.ConstraintEqualTo(label.LeadingAnchor).Active = true;
				textView.TrailingAnchor.ConstraintEqualTo(label.TrailingAnchor).Active = true;

				textView.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
				textView.ScrollEnabled = false;
				textView.Editable = false;
				textView.DataDetectorTypes = UIDataDetectorType.Link;
				textView.Selectable = true;
				textView.Font = UIFont.SystemFontOfSize(13);
				textView.Text = message;
				
			}
            catch (Exception ex)
            {
				ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
			}

			context.PresentViewController(dialog, true, null);
		}

		public void LogAlert(string message)
		{
			message = message.Replace(DateTime.UtcNow.ToString(@"yyyy-MM-dd "), "");

			UIAlertController dialog = UIAlertController.Create("", message, UIAlertControllerStyle.Alert);
			dialog.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

			try
            {
				UIView container = dialog.View.Subviews[0].Subviews[0].Subviews[0];
				((UILabel)container.Subviews[0].Subviews[0].Subviews[2]).TextAlignment = UITextAlignment.Left;

				dialog.View.WidthAnchor.ConstraintEqualTo((nfloat)(context.View.Frame.Width - 44)).Active = true;
				dialog.View.Subviews[0].WidthAnchor.ConstraintEqualTo((nfloat)(context.View.Frame.Width - 44)).Active = true;
				container.WidthAnchor.ConstraintEqualTo((nfloat)(context.View.Frame.Width - 44)).Active = true;

				dialog.View.Subviews[0].LeftAnchor.ConstraintEqualTo(dialog.View.LeftAnchor).Active = true;
			}
            catch
            {
            }
			context.PresentViewController(dialog, true, null);
		}

        public void LogLocationAlert()
        {
			string alertText = "";
			string[] fileLines = File.ReadAllLines(locationLogFile);
			int counter = 0;
			for (int i = fileLines.Length - 1; i >= 0; i--)
			//for (int i = 0; i < fileLines.Length; i++)
			{
				counter++;
				string line = fileLines[i];
				int sep1Pos = line.IndexOf('|');

				long time = long.Parse(line.Substring(0, sep1Pos));
				DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(time).ToLocalTime();
				alertText += counter + " --- " + dt.ToString("HH:mm:ss") + " --- " + line.Substring(sep1Pos + 1) + Environment.NewLine;
			}
			LogAlert(alertText);
		}

		public void ErrorAlert(string message)
		{
			UIAlertController dialog = UIAlertController.Create(LangEnglish.ErrorEncountered, message, UIAlertControllerStyle.Alert);
			dialog.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

			context.PresentViewController(dialog, true, null);
		}

		public void LogActivity(string message)
		{
			try
			{
				File.AppendAllLines(logFile, new string[] { DateTime.UtcNow.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff") + "  " + message });
			}
			catch
			{
			}
		}

		public static void LogActivityStatic(string message)
		{
			try
			{
				File.AppendAllLines(logFile, new string[] { DateTime.UtcNow.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff") + "  " + message });
			}
			catch
			{
			}
		}

		public void LogLocation(string message)
		{
			try
			{
				File.AppendAllLines(locationLogFile, new string[] { message });
			}
			catch
			{
			}
		}

		public void LogError(string message) //only the last record is kept, to prevent the file from growing if there is a problem with the server.
		{
			try
			{
				int ID = (Session.ID is null) ? 0 : (int)Session.ID;
				File.WriteAllLines(errorFile, new string[] { DateTime.UtcNow.ToString(@"yyyy.MM.dd. HH\:mm\:ss") + " " + message + ", ID: " + ID });
			}
			catch
			{
			}
		}

		public void ReportError(string error)
		{
			if (error == "AUTHORIZATION_ERROR")
			{
				IntentData.logout = true;
				IntentData.authError = true;
				OpenPage("MainActivity", 1);
			}
			else if (error == "NoNetwork")
			{
				SnackIndef(LangEnglish.NoNetwork);
			}
			else if (error == "NetworkTimeout")
			{
			    SnackIndef(LangEnglish.NetworkTimeout);
			}
			else
			{
				string url = "action=reporterror&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
				string content = "Content=" + UrlEncode(error + Environment.NewLine
					+ "Version: " + UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion + " " + Environment.NewLine + UIDevice.CurrentDevice.Model + Environment.NewLine + File.ReadAllText(logFile));
				string responseString = MakeRequestSync(url, "POST", content);
				if (responseString == "OK")
				{
					ErrorAlert(error + Environment.NewLine + Environment.NewLine + LangEnglish.ErrorNotificationSent);
				}
				else
				{
					LogError(error);
					ErrorAlert(error + Environment.NewLine + Environment.NewLine + LangEnglish.ErrorNotificationToSend);
				}
			}
		}

		public void ReportErrorSilent(string error)
		{
			if (error == "AUTHORIZATION_ERROR")
			{
				IntentData.logout = true;
				IntentData.authError = true;
				OpenPage("MainActivity", 1);
			}
            else if (error != "NoNetwork" && error != "NetworkTimeout")
			{
				string url = "action=reporterror&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
				string content = "Content=" + UrlEncode(error + Environment.NewLine
					+ "Version: " + UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion + " " + Environment.NewLine + UIDevice.CurrentDevice.Model + Environment.NewLine + File.ReadAllText(logFile));
				MakeRequestSync(url, "POST", content);
			}
		}

		public void ReportErrorSnackNext(string error)
		{
			if (error == "AUTHORIZATION_ERROR")
			{
				IntentData.logout = true;
				IntentData.authError = true;
				OpenPage("MainActivity", 1);
			}
			else if (error == "NoNetwork")
			{
				Session.SnackMessage = LangEnglish.NoNetwork;
			}
			else if (error == "NetworkTimeout")
			{
				Session.SnackMessage = LangEnglish.NetworkTimeout;
			}
			else
			{
				string url = "action=reporterror&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
				string content = "Content=" + UrlEncode(error + Environment.NewLine
					+ "Version: " + UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion + " " + Environment.NewLine + UIDevice.CurrentDevice.Model + Environment.NewLine + File.ReadAllText(logFile));
				MakeRequestSync(url, "POST", content);
			}
		}

		public void CW(object message)
		{
			Console.WriteLine(Environment.NewLine + "-------------------------------------------------------------- " + message + Environment.NewLine);
		}

        public static void ShowConstraint(NSLayoutConstraint constraint)
        {
		    Console.WriteLine("Constraint: " + constraint.FirstItem + " --- " + constraint.FirstAttribute + " --- " + constraint.SecondItem + " --- " + constraint.SecondAttribute + " --- " + constraint.Constant + " --- " + constraint.Priority);
		}

		public string ShowClass<T>()
		{
			string str = "";
			Type type = typeof(T);
			FieldInfo[] fieldInfo = type.GetFields();
			foreach (FieldInfo field in fieldInfo)
			{
				str += field.Name + ": " + field.GetValue(null) + "\n";
			}
			return str;
		}

		public long Now()
		{
			return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		public string UrlEncode(string input)
		{
			if (!string.IsNullOrEmpty(input))
			{
				return input.Replace("#", "%23").Replace("&", "%26").Replace("+", "%2B");
			}
			else
			{
				return "";
			}
		}

		public string UnescapeBraces(string input)
		{
			return input.Replace(@"\{", "{").Replace(@"\}", "}").Replace(@"\""", @"""");
		}

		private void CollapseButton()
		{
			foreach (NSLayoutConstraint constraint in SnackBar.Constraints)
			{
				if (constraint.FirstAttribute == NSLayoutAttribute.Leading && constraint.SecondAttribute == NSLayoutAttribute.Trailing)
				{
					constraint.Constant = 0;
				}
			}
			Collapse(SnackButton);
		}

		private void ExpandButton()
		{
			foreach (NSLayoutConstraint constraint in SnackBar.Constraints)
			{
				if (constraint.FirstAttribute == NSLayoutAttribute.Leading && constraint.SecondAttribute == NSLayoutAttribute.Trailing)
				{
					constraint.Constant = 10;
				}
			}
			Expand(SnackButton);
		}

		public void CollapseX(UIView view)
		{
			view.WidthAnchor.ConstraintEqualTo(0).Active = true;
		}

		public void ExpandX(UIView view)
		{
			foreach (NSLayoutConstraint constraint in view.Constraints)
			{
				if (constraint.FirstAttribute == NSLayoutAttribute.Width && constraint.Constant == 0)
				{
					view.RemoveConstraint(constraint);
				}
			}
		}

		public void CollapseY(UIView view)
		{
			view.HeightAnchor.ConstraintEqualTo(0).Active = true;
		}

		public void ExpandY(UIView view) {
			foreach (NSLayoutConstraint constraint in view.Constraints)
			{
				if (constraint.FirstAttribute == NSLayoutAttribute.Height && constraint.Constant == 0)
				{
					view.RemoveConstraint(constraint);
				}
			}
		}

		public void Collapse(UIView view)
		{
			view.HeightAnchor.ConstraintEqualTo(0).Active = true;
			view.WidthAnchor.ConstraintEqualTo(0).Active = true;
		}

		public void Expand(UIView view)
		{
			foreach (NSLayoutConstraint constraint in view.Constraints)
			{
				if (constraint.FirstAttribute == NSLayoutAttribute.Height && constraint.Constant == 0)
				{
					view.RemoveConstraint(constraint);
				}
				if (constraint.FirstAttribute == NSLayoutAttribute.Width && constraint.Constant == 0)
				{
					view.RemoveConstraint(constraint);
				}
			}
		}

        public void SetHeight(UIView view, nfloat height)
        {
			foreach (NSLayoutConstraint constraint in view.Constraints)
            {
				if (constraint.FirstAttribute == NSLayoutAttribute.Height && constraint.SecondItem is null)
				{
					constraint.Constant = height;
				}
			}
		}

		public void SetWidth(UIView view, nfloat width)
		{
			foreach (NSLayoutConstraint constraint in view.Constraints)
			{
				if (constraint.FirstAttribute == NSLayoutAttribute.Width && constraint.SecondItem is null)
				{
					constraint.Constant = width;
				}
			}

		}

        public void SetLeftMargin(UIView view, nfloat value)
        {
			foreach (NSLayoutConstraint constraint in view.Superview.Constraints)
			{
                if (constraint.FirstItem == view && constraint.FirstAttribute == NSLayoutAttribute.Leading)
                {
					constraint.Constant = value;
                }
                else if (constraint.SecondItem == view && constraint.SecondAttribute == NSLayoutAttribute.Leading)
                {
					constraint.Constant = -value;
                }
			}
		}

		public static UIImage LoadFromUrl(string uri)
		{
			//Console.WriteLine(DateTime.UtcNow.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff") + " ----- LoadFromUrl start -----------" + uri);
			try
            {
				using (var url = new NSUrl(uri))
				using (var data = NSData.FromUrl(url))
                {
					var image = UIImage.LoadFromData(data);
                    //Console.WriteLine(DateTime.UtcNow.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff") + " ------ LoadFromUrl end --------");
					return image;
				}
                    

			}
            catch
            {
				try
				{
					File.AppendAllLines(logFile, new string[] { DateTime.UtcNow.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff") + " Error loading image: " + uri });
					Console.WriteLine("Error loading image: " + uri);
				}
				catch
				{
				}
				return null;
			}			
		}

		public static Task<NSData> LoadFromUrlAsyncData(string uri)
		{
			return Task.Run(() => {
				//Console.WriteLine(DateTime.UtcNow.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff") + " ----- LoadFromUrl start -----------" + uri);
				try
			    {
				    using (var url = new NSUrl(uri))
					    return NSData.FromUrl(url);

			    }
			    catch
			    {
				    try
				    {
					    File.AppendAllLines(logFile, new string[] { DateTime.UtcNow.ToString(@"yyyy-MM-dd HH\:mm\:ss.fff") + " Error loading image: " + uri });
					    Console.WriteLine("Error loading image: " + uri);
				    }
				    catch
				    {
				    }
				    return null;
			    }
			});
		}

        public static bool transitionRunning;
		public static string transitionTarget;
		public static byte transitionAnim;

		public static void OpenPage(string target, byte anim)
        {
			BaseActivity currentContext =  GetCurrentViewController(); //LocationManager's context is either ListActivity or ProfileEditActivity, but in case of authorization error, we need to open a new viewcontroller.
			currentContext.active = false;
			LogActivityStatic("OpenPage currentContext " + currentContext + " target " + target + " anim " + anim + " transitionRunning " + transitionRunning);
			Console.WriteLine("OpenPage currentContext " + currentContext + " target " + target + " anim " + anim + " transitionRunning " + transitionRunning);
			if (transitionRunning)
            {
				transitionTarget = target;
				transitionAnim = anim;
				return;
			}

			transitionTarget = "empty";
			transitionRunning = true; //In ProfileView, transition will start after a http request is made, due to it is being called sync. Unless I figure out how to cancel presentation from ViewWillAppear, completing the transition is necessary.

            if (anim == 0)
            {
				currentContext.DismissViewController(true, null);
			}
            else
            {
				UIViewController activity = currentContext.Storyboard.InstantiateViewController(target) as UIViewController;
				activity.ModalPresentationStyle = UIModalPresentationStyle.FullScreen; //fullscreen in: CurrentContext, BlurOverFullScreen, custom, fullscreen, OverCurrentContext OverFullScreen; popover in: automatic , formsheet, pagesheet, Popover

				var transitioningDelegate = new TransitioningDelegate(anim);
				activity.TransitioningDelegate = transitioningDelegate;

				currentContext.PresentViewController(activity, true, null);
			}            
		}

		public static BaseActivity GetCurrentViewController()
		{
			UIViewController root = UIApplication.SharedApplication.KeyWindow.RootViewController;
			UIViewController prevController;
			UIViewController currentController = root;
			do
			{
				prevController = currentController;
				currentController = prevController.PresentedViewController;

			} while (currentController != null);

            if (prevController is BaseActivity) //else UIAlertController
            {
				return (BaseActivity)prevController;
			}
            else
            {
				return (BaseActivity)prevController.PresentingViewController;
            }			
		}

		public string GetLang(string id)
        {
			Type type = typeof(LangEnglish);
			FieldInfo fieldInfo = type.GetField(id);
			return (string)fieldInfo.GetValue(null);
		}

		public void SetShadow(UIView view, float x, float y, float radius)
		{
			view.Layer.MasksToBounds = false;
			view.Layer.ShadowColor = UIColor.Gray.CGColor;
			view.Layer.ShadowOffset = new CoreGraphics.CGSize(x, y);
			view.Layer.ShadowOpacity = .5f;
			view.Layer.ShadowRadius = radius;
		}

		public void SetRoundShadow(UIView view, float x, float y, float shadowRadius, float cornerRadius, bool isLeft)
		{
			UIBezierPath path;
            if(isLeft)
            {
				path = UIBezierPath.FromRoundedRect(view.Bounds, UIRectCorner.TopLeft | UIRectCorner.BottomLeft, new CGSize(cornerRadius, cornerRadius));
			}
            else
            {
				path = UIBezierPath.FromRoundedRect(view.Bounds, UIRectCorner.TopRight | UIRectCorner.BottomRight, new CGSize(cornerRadius, cornerRadius));
			}
			
			view.Layer.MasksToBounds = false;
			view.Layer.ShadowColor = UIColor.Gray.CGColor;
			view.Layer.ShadowOffset = new CoreGraphics.CGSize(x, y);
			view.Layer.ShadowOpacity = 0.35f;
			view.Layer.ShadowRadius = shadowRadius;
			view.Layer.ShadowPath = path.CGPath;
		}

		public void DrawBorder(UITextView view)
        {
			view.Layer.BorderColor = UIColor.FromWhiteAlpha(0.804f, 1).CGColor; //d3 = 211
			view.Layer.BorderWidth = 1;
			view.Layer.CornerRadius = 5;
			view.TextContainerInset = new UIEdgeInsets(7, 2, 7, 2);
		}

        public void RemoveSubviews(UIView view)
        {
            foreach (UIView subview in view.Subviews)
            {
				subview.RemoveFromSuperview();
            }
        }

		public void AnimateRipple(UIView view, byte zoom)
		{
			if (!context.rippleRunning)
			{
				view.Alpha = 1;
				context.rippleRunning = true;

				UIView.Animate(context.tweenTime, () => {

					foreach (NSLayoutConstraint constraint in view.Constraints)
					{
						if (constraint.FirstAttribute == NSLayoutAttribute.Width || constraint.FirstAttribute == NSLayoutAttribute.Height)
						{
							constraint.Constant = constraint.Constant * zoom;
						}
					}
					context.View.LayoutIfNeeded();

				}, () => {
					UIView.Animate(context.tweenTime, () => {

						view.Alpha = 0;

					}, () => {
						foreach (NSLayoutConstraint constraint in view.Constraints)
						{
							if (constraint.FirstAttribute == NSLayoutAttribute.Width || constraint.FirstAttribute == NSLayoutAttribute.Height)
							{
								constraint.Constant = constraint.Constant / zoom;
							}
						}
						context.rippleRunning = false;
					});
				});
			}
		}

        public void ScrollToBottom(UIScrollView scroll)
        {
			context.View.LayoutIfNeeded();

			if (scroll.ContentSize.Height + context.uselessHeight > scroll.Bounds.Size.Height)
			{
				CGPoint bottomOffset = new CGPoint(0, scroll.ContentSize.Height - scroll.Bounds.Size.Height);

				if ((context is RegisterActivity || context is ProfileEditActivity || context is SettingsActivity) && context.roundBottomHeight > 0 && context.keyboardHeight == 0)
                {
					bottomOffset = new CGPoint(0, scroll.ContentSize.Height - scroll.Bounds.Size.Height + context.roundBottomHeight);
				}
				UIView.Animate(context.tweenTime, () => { scroll.ContentOffset = bottomOffset; }, () => { });
				
			}
		}
	}
}