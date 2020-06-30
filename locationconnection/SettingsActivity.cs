using Foundation;
using System;
using System.IO;
using UIKit;

namespace LocationConnection
{
    public partial class SettingsActivity : BaseActivity
    {
        public UIScrollView SettingsScroll { get { return Settings_Scroll; } }

        public SettingsActivity (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackTopConstraint_Base = SnackTopConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;
                ScrollBottomConstraint_Base = ScrollBottomConstraint;
                ScrollBottomOuterConstraint_Base = ScrollBottomOuterConstraint;

                SettingsHeaderText.Text = LangEnglish.MenuSettings;
                NotificationsLabel.Text = LangEnglish.NotificationsText;
                NotificationsInApp.Text = LangEnglish.NotificationsInApp;
                NotificationsBackground.Text = LangEnglish.NotificationsBackground;
                NotificationsNewMatchLabel.Text = LangEnglish.NotificationsNewMatchText;
                NotificationsNewMessageLabel.Text = LangEnglish.NotificationsNewMessageText;
                NotificationsUnmatchLabel.Text = LangEnglish.NotificationsUnmatchText;
                NotificationsRematchLabel.Text = LangEnglish.NotificationsRematchText;

                DisplaySection.Text = LangEnglish.DisplaySection;
                MapIconSizeLabel.Text = LangEnglish.MapIconSizeText;
                MapRatioLabel.Text = LangEnglish.MapRatioText;

                LocationHistoryButton.Layer.MasksToBounds = true;
                MessageSend.Layer.MasksToBounds = true;
                ProgramLogButton.Layer.MasksToBounds = true;

                MapIconSize.MinValue = Constants.MapIconSizeMin;
                MapIconSize.MaxValue = Constants.MapIconSizeMax;

                MapRatio.MinValue = Constants.MapRatioMin;
                MapRatio.MaxValue = Constants.MapRatioMax;

                LocationLabel.Text = LangEnglish.LocationText;
                
                InAppLocationRateLabel.Text = LangEnglish.InAppLocationRate;
                InAppLocationRate.MinValue = Constants.InAppLocationRateMin;
                InAppLocationRate.MaxValue = Constants.InAppLocationRateMax;

                if (Constants.SafeLocationMode)
                {
                    /*c.CollapseY(BackgroundLocationLabel);
                    c.CollapseY(BackgroundLocation);
                    BackgroundLocation.Hidden = true;
                    c.CollapseY(BackgroundLocationRateLabel);
                    c.CollapseY(BackgroundLocationRate);
                    BackgroundLocationRate.Hidden = true;   
                    c.CollapseY(BackgroundLocationRateValue);

                    InAppLocationRateLabelConstraint.Constant = 0;
                    LocationHistoryButtonConstraint.Constant = -5;*/

                    InAppLocationRateExplanation.Text = LangEnglish.InAppLocationRateExplanation;
                }
                else
                {
                    c.CollapseY(InAppLocationRateExplanation);
                    /*BackgroundLocationRateLabelConstraint.Constant = 0;

                    BackgroundLocationLabel.Text = LangEnglish.BackgroundLocation;
                    BackgroundLocationRateLabel.Text = LangEnglish.BackgroundLocationRate;*/
                }

                //BackgroundLocationRate.MinValue = Constants.BackgroundLocationRateMin;
                //BackgroundLocationRate.MaxValue = Constants.BackgroundLocationRateMax;

                LocationHistoryButton.SetTitle(LangEnglish.LocationHistory, UIControlState.Normal);
                SettingsFormCaption.SetTitle(LangEnglish.SettingsFormCaption, UIControlState.Normal);

                MessageSend.SetTitle(LangEnglish.SettingsFormSend, UIControlState.Normal);
                ProgramLogIncluded.Text = LangEnglish.ProgramLogIncluded;
                ProgramLogButton.SetTitle(LangEnglish.SeeProgramLog, UIControlState.Normal);

                c.DrawBorder(MessageEdit);
                c.CollapseY(MessageContainer);

                SettingsBack.TouchDown += SettingsBack_TouchDown;
                SettingsBack.TouchUpInside += SettingsBack_TouchUpInside;
                MapIconSize.ValueChanged += MapIconSize_ValueChanged;
                MapRatio.ValueChanged += MapRatio_ValueChanged;
                //BackgroundLocation.ValueChanged += BackgroundLocation_ValueChanged;
                InAppLocationRate.ValueChanged += InAppLocationRate_ValueChanged;
                //BackgroundLocationRate.ValueChanged += BackgroundLocationRate_ValueChanged;
                LocationHistoryButton.TouchUpInside += LocationHistoryButton_TouchUpInside;
                SettingsFormCaption.TouchUpInside += SettingsFormCaption_TouchUpInside;
                MessageSend.TouchUpInside += SettingsMessageSend_TouchUpInside;
                ProgramLogButton.TouchUpInside += ProgramLogButton_TouchUpInside;
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                MapIconSize.Value = (int)Settings.MapIconSize;
                MapIconSizeValue.Text = Settings.MapIconSize.ToString();

                MapRatio.Value = (float)Settings.MapRatio;
                MapRatioValue.Text = Settings.MapRatio.ToString();

                if (!c.IsLocationEnabled() || !(bool)Session.UseLocation)
                {
                    //BackgroundLocation.Enabled = false;
                    InAppLocationRate.Enabled = false;
                    //BackgroundLocationRate.Enabled = false;
                }
                else
                {
                    InAppLocationRate.Enabled = true;
                    if (c.IsLoggedIn())
                    {
                        //BackgroundLocation.Enabled = true;
                        //BackgroundLocationRate.Enabled = true;
                    }
                    else
                    {
                        //BackgroundLocation.Enabled = false;
                        //BackgroundLocationRate.Enabled = false;
                    }
                }

                if (c.IsLoggedIn())
                {
                    CheckMatchInApp.Enabled = true;
                    CheckMessageInApp.Enabled = true;
                    CheckUnmatchInApp.Enabled = true;
                    CheckRematchInApp.Enabled = true;
                    CheckMatchBackground.Enabled = true;
                    CheckMessageBackground.Enabled = true;
                    CheckUnmatchBackground.Enabled = true;
                    CheckRematchBackground.Enabled = true;

                    CheckMatchInApp.Checked = (bool)Session.MatchInApp;
                    CheckMessageInApp.Checked = (bool)Session.MessageInApp;
                    CheckUnmatchInApp.Checked = (bool)Session.UnmatchInApp;
                    CheckRematchInApp.Checked = (bool)Session.RematchInApp;
                    CheckMatchBackground.Checked = (bool)Session.MatchBackground;
                    CheckMessageBackground.Checked = (bool)Session.MessageBackground;
                    CheckUnmatchBackground.Checked = (bool)Session.UnmatchBackground;
                    CheckRematchBackground.Checked = (bool)Session.RematchBackground;

                    /*BackgroundLocation.On = (bool)Session.BackgroundLocation;
                    if (BackgroundLocation.On)
                    {
                        BackgroundLocationRate.Enabled = true;
                        BackgroundLocationRateValue.TextColor = UIColor.FromName("PrimaryDark");
                    }
                    else
                    {
                        BackgroundLocationRate.Enabled = false;
                        BackgroundLocationRateValue.TextColor = UIColor.FromName("ImageEditorBackground");
                    }*/
                    InAppLocationRate.Value = (int)Session.InAppLocationRate;
                    InAppLocationRateValue.Text = GetTimeString((int)Session.InAppLocationRate);
                    //BackgroundLocationRate.Value = (int)Session.BackgroundLocationRate;
                    //BackgroundLocationRateValue.Text = GetTimeString((int)Session.BackgroundLocationRate);
                }
                else
                {
                    CheckMatchInApp.Enabled = false;
                    CheckMessageInApp.Enabled = false;
                    CheckUnmatchInApp.Enabled = false;
                    CheckRematchInApp.Enabled = false;
                    CheckMatchBackground.Enabled = false;
                    CheckMessageBackground.Enabled = false;
                    CheckUnmatchBackground.Enabled = false;
                    CheckRematchBackground.Enabled = false;

                    CheckMatchInApp.Checked = false;
                    CheckMessageInApp.Checked = false;
                    CheckUnmatchInApp.Checked = false;
                    CheckRematchInApp.Checked = false;
                    CheckMatchBackground.Checked = false;
                    CheckMessageBackground.Checked = false;
                    CheckUnmatchBackground.Checked = false;
                    CheckRematchBackground.Checked = false;

                    //BackgroundLocation.On = false;
                    InAppLocationRate.Value = (int)Settings.InAppLocationRate;
                    InAppLocationRateValue.Text = GetTimeString((int)Settings.InAppLocationRate);
                    //BackgroundLocationRate.Value = 0;
                    //BackgroundLocationRateValue.Text = "";
                }
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public async override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            bool changed = false;

            if (Settings.MapIconSize != Math.Round(MapIconSize.Value))
            {
                Session.LastDataRefresh = null;
                Settings.MapIconSize = (int)Math.Round(MapIconSize.Value);
                changed = true;
            }
            if (Settings.MapRatio != Math.Round(MapRatio.Value, 2))
            {
                Settings.MapRatio = (float)Math.Round(MapRatio.Value, 2);
                changed = true;
            }

            if (c.IsLoggedIn())
            {
                //bool backgroundLocationChanged = false;

                string requestStringBase = "action=updatesettings&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                string requestStringAdd = "";

                if (CheckMatchInApp.Checked != Session.MatchInApp)
                {
                    requestStringAdd += "&MatchInApp=" + CheckMatchInApp.Checked;
                }
                if (CheckMessageInApp.Checked != Session.MessageInApp)
                {
                    requestStringAdd += "&MessageInApp=" + CheckMessageInApp.Checked;
                }
                if (CheckUnmatchInApp.Checked != Session.UnmatchInApp)
                {
                    requestStringAdd += "&UnmatchInApp=" + CheckUnmatchInApp.Checked;
                }
                if (CheckRematchInApp.Checked != Session.RematchInApp)
                {
                    requestStringAdd += "&RematchInApp=" + CheckRematchInApp.Checked;
                }
                if (CheckMatchBackground.Checked != Session.MatchBackground)
                {
                    requestStringAdd += "&MatchBackground=" + CheckMatchBackground.Checked;
                }
                if (CheckMessageBackground.Checked != Session.MessageBackground)
                {
                    requestStringAdd += "&MessageBackground=" + CheckMessageBackground.Checked;
                }
                if (CheckUnmatchBackground.Checked != Session.UnmatchBackground)
                {
                    requestStringAdd += "&UnmatchBackground=" + CheckUnmatchBackground.Checked;
                }
                if (CheckRematchBackground.Checked != Session.RematchBackground)
                {
                    requestStringAdd += "&RematchBackground=" + CheckRematchBackground.Checked;
                }

                /*if (BackgroundLocation.On != Session.BackgroundLocation)
                {
                    requestStringAdd += "&BackgroundLocation=" + BackgroundLocation.On;
                    backgroundLocationChanged = true;
                }*/

                if (Math.Round(InAppLocationRate.Value) != Session.InAppLocationRate)
                {
                    requestStringAdd += "&InAppLocationRate=" + Math.Round(InAppLocationRate.Value);
                }
                /*if (Math.Round(BackgroundLocationRate.Value) != Session.BackgroundLocationRate)
                {
                    requestStringAdd += "&BackgroundLocationRate=" + Math.Round(BackgroundLocationRate.Value);
                }*/

                if (requestStringAdd != "") //if the form was changed
                {
                    string responseString = await c.MakeRequest(requestStringBase + requestStringAdd);
                    if (responseString.Substring(0, 2) == "OK")
                    {
                        if (responseString.Length > 2) //a change happened
                        {
                            c.LoadCurrentUser(responseString);

                            /*if (backgroundLocationChanged && !(locMgr is null))
                            {
                                locMgr.RestartLocationUpdates();
                            }*/
                        }
                    }
                    else
                    {
                        c.ReportErrorSnackNext(responseString);
                    }
                }
            }
            else
            {
                if (Math.Round(InAppLocationRate.Value) != Settings.InAppLocationRate)
                {
                    Settings.InAppLocationRate = (int)Math.Round(InAppLocationRate.Value);
                    changed = true;
                }
            }

            if (changed)
            {
                c.SaveSettings();
            }
        }

        private void SettingsBack_TouchDown(object sender, EventArgs e)
        {
            c.AnimateRipple(RippleSettings, 2);
        }

        private void SettingsBack_TouchUpInside(object sender, EventArgs e)
        {
            CommonMethods.OpenPage(null, 0);
        }

        private void MapIconSize_ValueChanged(object sender, EventArgs e)
        {
            MapIconSizeValue.Text = Math.Round(MapIconSize.Value).ToString();
        }

        private void MapRatio_ValueChanged(object sender, EventArgs e)
        {
            MapRatioValue.Text = Math.Round(MapRatio.Value,2).ToString();
        }

        /*private void BackgroundLocation_ValueChanged(object sender, EventArgs e)
        {
            if (BackgroundLocation.On)
            {
                BackgroundLocationRate.Enabled = true;
                BackgroundLocationRateValue.TextColor = UIColor.FromName("PrimaryDark");
            }
            else
            {
                BackgroundLocationRate.Enabled = false;
                BackgroundLocationRateValue.TextColor = UIColor.FromName("ImageEditorBackground");
            }
        }*/

        private void InAppLocationRate_ValueChanged(object sender, EventArgs e)
        {
            InAppLocationRate.Value = (int)Math.Round(InAppLocationRate.Value / 5) * 5;
            InAppLocationRateValue.Text = GetTimeString((int)Math.Round(InAppLocationRate.Value));
        }

        /*private void BackgroundLocationRate_ValueChanged(object sender, EventArgs e)
        {
            BackgroundLocationRate.Value = (int)Math.Round(BackgroundLocationRate.Value / 60) * 60;
            BackgroundLocationRateValue.Text = GetTimeString((int)Math.Round(BackgroundLocationRate.Value));
        }*/

        private void LocationHistoryButton_TouchUpInside(object sender, EventArgs e)
        {
            View.EndEditing(true); //Program freezes on debug / crashes, when in Settings page I opened the keyboard, opened Location Activity, and stepped back. "XPC connection interrupted" 
            CommonMethods.OpenPage("LocationActivity", 1);
        }

        private void SettingsFormCaption_TouchUpInside(object sender, EventArgs e)
        {
            if (MessageContainer.Frame.Height == 0)
            {
                c.ExpandY(MessageContainer);              
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });

                //page will scroll to bottom automatically as the TextView gets focus
                MessageEdit.BecomeFirstResponder();
            }
            else
            {
                c.CollapseY(MessageContainer);
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                View.EndEditing(true);
            }
        }

        private async void SettingsMessageSend_TouchUpInside(object sender, EventArgs e)
        {
            View.EndEditing(true);

            if (MessageEdit.Text != "")
            {
                MessageSend.Enabled = false;
                MessageSend.Alpha = 0.5f;

                string url = "action=reporterror&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                string content = "Content=" + CommonMethods.UrlEncode(MessageEdit.Text + Environment.NewLine
                    + "Version: " + UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion + " " + Environment.NewLine + UIDevice.CurrentDevice.Model + Environment.NewLine + File.ReadAllText(CommonMethods.logFile));
                string responseString = await c.MakeRequest(url, "POST", content);

                if (responseString == "OK")
                {
                    MessageEdit.Text = "";
                    c.CollapseY(MessageContainer);
                    c.Snack(LangEnglish.SettingsSent);
                }
                else
                {
                    c.ReportError(responseString);
                }
                MessageSend.Enabled = true;
                MessageSend.Alpha = 1;
            }
        }

        private void ProgramLogButton_TouchUpInside(object sender, EventArgs e)
        {
            c.LogAlert(File.ReadAllText(CommonMethods.logFile));            
        }        

        private string GetTimeString(int seconds)
        {
            if (seconds == 0)
            {
                return "";
            }

            string hour = LangEnglish.Hour;
            string min = LangEnglish.ShortMinute;
            string mins = LangEnglish.ShortMinutes;
            string sec = "s";
            string secs = "s";

            string str = "";

            TimeSpan ts = TimeSpan.FromSeconds(seconds);
            bool showSeconds = true;

            if (ts.Hours > 0)
            {
                str += ts.Hours + " " + hour + " ";
            }

            if (ts.Minutes > 1)
            {
                str += ts.Minutes + " " + mins + " ";
                if (ts.Minutes >= 5)
                {
                    showSeconds = false;
                }
            }
            else if (ts.Minutes > 0)
            {
                str += ts.Minutes + " " + min + " ";
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

            return str.Substring(0, str.Length - 1);
        }
    }
}