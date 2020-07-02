using Foundation;
using System;
using System.IO;
using System.Reflection;
using UIKit;

namespace LocationConnection
{
    public partial class MainActivity : BaseActivity, IUITextFieldDelegate
    {
        private string checkFormMessage;
        private bool resetSectionVisible;

        public MainActivity(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                LogoText.Text = LangEnglish.LogoText;
                LoginEmailText.Text = LangEnglish.LoginEmail;
                LoginPasswordText.Text = LangEnglish.LoginPassword;
                LoginDone.SetTitle(LangEnglish.LoginDone, UIControlState.Normal);
                ResetPassword.SetTitle(LangEnglish.ResetPassword, UIControlState.Normal);
                ResetEmailText.Text = LangEnglish.ResetEmail;
                ResetSendButton.SetTitle(LangEnglish.ResetSend, UIControlState.Normal);
                RegisterButton.SetTitle(LangEnglish.RegisterButton, UIControlState.Normal);
                ListButton.SetTitle(LangEnglish.ListButton, UIControlState.Normal);

                LoginDone.Layer.MasksToBounds = true;
                ResetPassword.Layer.MasksToBounds = true;
                ResetSendButton.Layer.MasksToBounds = true;
                RegisterButton.Layer.MasksToBounds = true;
                ListButton.Layer.MasksToBounds = true;

                c.CollapseY(ResetSection);
                resetSectionVisible = false;

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                LoginPassword.Delegate = this;
                LoginDone.TouchUpInside += LoginDone_Click;
                ResetPassword.TouchUpInside += ResetPassword_Click;
                ResetEmail.Delegate = this;
                ResetSendButton.TouchUpInside += ResetSendButton_Click;
                RegisterButton.TouchUpInside += RegisterButton_Click;
                ListButton.TouchUpInside += ListButton_Click;

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackTopConstraint_Base = SnackTopConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;
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

                if (c.snackVisible)
                {
                    c.HideSnack();
                }

                if (!(IntentData.error is null))
                {
                    c.ErrorAlert(IntentData.error + Environment.NewLine + Environment.NewLine + LangEnglish.ErrorNotificationSent);
                    IntentData.error = null;
                    return;
                }
                if (IntentData.logout)
                {
                    c.Log("Logout");
                    IntentData.logout = false;

                    if (!string.IsNullOrEmpty(locationUpdatesTo))
                    {
                        EndLocationShare();
                        locationUpdatesTo = null;
                    }
                    locationUpdatesFrom = null;
                    locationUpdatesFromData = null;

                    var latitude = Session.Latitude;
                    var longitude = Session.Longitude;
                    var locationTime = Session.LocationTime;

                    if (!Constants.SafeLocationMode)
                    {
                        c.ClearCurrentUser();
                    }
                    else
                    {
                        var safeLatitude = Session.SafeLatitude;
                        var safeLongitude = Session.SafeLongitude;
                        var safeLocationTime = Session.SafeLocationTime;

                        var latestLatitude = Session.LatestLatitude;
                        var latestLongitude = Session.LatestLongitude;
                        var latestLocationTime = Session.LatestLocationTime;

                        c.ClearCurrentUser();

                        Session.SafeLatitude = safeLatitude;
                        Session.SafeLongitude = safeLongitude;
                        Session.SafeLocationTime = safeLocationTime;

                        Session.LatestLatitude = latestLatitude;
                        Session.LatestLongitude = latestLongitude;
                        Session.LatestLocationTime = latestLocationTime;
                    }

                    Session.Latitude = latitude;
                    Session.Longitude = longitude;
                    Session.LocationTime = locationTime;

                    if (!string.IsNullOrEmpty(locationUpdatesTo))
                    {
                        locationUpdatesTo = null;
                    }
                    if (IntentData.authError)
                    {
                        IntentData.authError = false;
                        Session.SnackMessage = LangEnglish.LoggedOut;
                        Session.SnackPermanent = true;
                    }
                    LoginEmail.Text = "";
                    LoginPassword.Text = "";
                }
                if (!(ListActivity.listProfiles is null))
                {
                    ListActivity.listProfiles.Clear();
                    ListActivity.totalResultCount = null;
                }
                Session.LastDataRefresh = null;
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            //imm.HideSoftInputFromWindow(SearchTerm.WindowToken, 0);
            if (textField == LoginPassword)
            {
                LoginDone_Click(null, null);
            }
            else if (textField == ResetEmail)
            {
                ResetSendButton_Click(null, null);
            }
            return true;
        }

        private async void LoginDone_Click(object sender, EventArgs e)
        {
            //View.EndEditing(true);
            if (CheckFields())
            {
                View.EndEditing(true);

                LoginDone.Enabled = false;
                LoginDone.Alpha = 0.5f;

                string url = "action=login&User=" + CommonMethods.UrlEncode(LoginEmail.Text.Trim()) + "&Password=" + CommonMethods.UrlEncode(LoginPassword.Text.Trim());

                string responseString = await c.MakeRequest(url);
                if (responseString.Substring(0, 2) == "OK")
                {
                    if (File.Exists(regSessionFile))
                    {
                        File.Delete(regSessionFile);
                    }
                    if (File.Exists(regSaveFile))
                    {
                        File.Delete(regSaveFile);
                    }

                    c.LoadCurrentUser(responseString);

                    if ((bool)Session.UseLocation && c.IsLocationEnabled() && (bool)Session.BackgroundLocation && !(locMgr is null))
                    {
                        locMgr.RestartLocationUpdates();
                    }

                    CommonMethods.OpenPage("ListActivity", 1);
                }
                else if (responseString.Substring(0, 6) == "ERROR_")
                {
                    c.Snack(c.GetLang(responseString.Substring(6)));
                }
                else
                {
                    c.ReportError(responseString);
                }
                LoginDone.Enabled = true;
                LoginDone.Alpha = 1;
            }
            else
            {
                c.Snack(checkFormMessage);
            }

        }

        private bool CheckFields()
        {
            if (LoginEmail.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.LoginEmailEmpty;
                //LoginEmail.RequestFocus();
                return false;
            }
            if (LoginPassword.Text.Trim().Length < 6)
            {
                checkFormMessage = LangEnglish.LoginPasswordShort;
                //LoginPassword.RequestFocus();
                return false;
            }
            return true;
        }

        private void ResetPassword_Click(object sender, EventArgs e)
        {
            if (!resetSectionVisible)
            {
                c.ExpandY(ResetSection);
                resetSectionVisible = true;
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
            }
            else
            {
                c.CollapseY(ResetSection);
                resetSectionVisible = false;
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
            }
        }

        private async void ResetSendButton_Click(object sender, EventArgs e)
        {
            View.EndEditing(true);

            if (CheckFieldsReset())
            {
                ResetSendButton.Enabled = false;
                ResetSendButton.Alpha = 0.5f;

                string url = "action=resetpassword&Email=" + CommonMethods.UrlEncode(ResetEmail.Text.Trim());

                string responseString = await c.MakeRequest(url);
                if (responseString.Substring(0, 2) == "OK")
                {
                    c.Alert(LangEnglish.ResetEmailSent);
                    ResetEmail.Text = "";
                    c.CollapseY(ResetSection);
                    resetSectionVisible = false;
                }
                else
                {
                    c.ReportError(responseString);
                }                
                ResetSendButton.Enabled = true;
                ResetSendButton.Alpha = 1f;
            }
            else
            {
                c.Snack(checkFormMessage);
            }
        }

        private bool CheckFieldsReset()
        {
            if (ResetEmail.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.LoginEmailEmpty;
                //ResetEmail.RequestFocus();
                return false;
            }
            return true;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            CommonMethods.OpenPage("RegisterActivity", 1);
        }

        private void ListButton_Click(object sender, EventArgs e)
        {
            CommonMethods.OpenPage("ListActivity", 1);
        }
    }
}