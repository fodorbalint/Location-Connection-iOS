//location switches clipped to the right
//pressing cancel button is registeracivity crashes program

using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UIKit;

namespace LocationConnection
{
    public partial class ProfileEditActivity : BaseActivity, IUITextViewDelegate
    {
        public RegisterCommonMethods rc;
        private string checkFormMessage;

        public ImageFrameLayout ImagesUploaded { get { return EditImagesUploaded; } }
        public TouchScrollView ProfileEditScroll { get { return ProfileEdit_Scroll; } }

        public ProfileEditActivity (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                Images.SetTitle(LangEnglish.Images, UIControlState.Normal);
                ImagesProgressText.Text = "";
                DescriptionLabel.Text = LangEnglish.Description;
                SexLabel.Text = LangEnglish.EditSex;
                WomenLabel.Text = LangEnglish.Women;
                MenLabel.Text = LangEnglish.Men;

                AccountData.SetTitle(LangEnglish.EditAccountData, UIControlState.Normal);
                ChangePassword.SetTitle(LangEnglish.EditChangePassword, UIControlState.Normal);
                LocationSettings.SetTitle(LangEnglish.EditLocationSettings, UIControlState.Normal);

                Save.SetTitle(LangEnglish.EditSave, UIControlState.Normal);
                Cancel.SetTitle(LangEnglish.Cancel, UIControlState.Normal);

                MoreOptions.SetTitle(LangEnglish.EditMoreOptions, UIControlState.Normal);

                EmailLabel.Text = LangEnglish.Email;
                UsernameLabel.Text = LangEnglish.Username;
                CheckUsername.SetTitle(LangEnglish.CheckAvailability, UIControlState.Normal);
                NameLabel.Text = LangEnglish.Name;

                OldPasswordLabel.Text = LangEnglish.EditOldPassword;
                NewPasswordLabel.Text = LangEnglish.EditNewPassword;
                ConfirmPasswordLabel.Text = LangEnglish.EditConfirmPassword;

                UseLocationLabel.Text = LangEnglish.UseLocation;
                LocationShareLabel.Text = LangEnglish.LocationShare;
                LocationShareAllLabel.Text = LangEnglish.LocationShareAll;
                LocationShareLikeLabel.Text = LangEnglish.LocationShareLike;
                LocationShareMatchLabel.Text = LangEnglish.LocationShareMatch;
                LocationShareFriendLabel.Text = LangEnglish.LocationShareFriend;
                LocationShareNoneLabel.Text = LangEnglish.LocationShareNone;
                DistanceShareLabel.Text = LangEnglish.DistanceShare;
                DistanceShareAllLabel.Text = LangEnglish.DistanceShareAll;
                DistanceShareLikeLabel.Text = LangEnglish.DistanceShareLike;
                DistanceShareMatchLabel.Text = LangEnglish.DistanceShareMatch;
                DistanceShareFriendLabel.Text = LangEnglish.DistanceShareFriend;
                DistanceShareNoneLabel.Text = LangEnglish.DistanceShareNone;
                ImageEditorLabel.Text = LangEnglish.ImageEditorLabel;

                DeactivateAccount.SetTitle(LangEnglish.DeactivateAccount, UIControlState.Normal);
                DeleteAccount.SetTitle(LangEnglish.DeleteAccount, UIControlState.Normal);

                DescriptionText.Delegate = this;

                Images.Layer.MasksToBounds = true;
                CheckUsername.Layer.MasksToBounds = true;
                Save.Layer.MasksToBounds = true;
                Cancel.Layer.MasksToBounds = true;
                DeactivateAccount.Layer.MasksToBounds = true;
                DeleteAccount.Layer.MasksToBounds = true;

                c.DrawBorder(DescriptionText);

                c.CollapseY(AccountDataSection);
                c.CollapseY(ChangePasswordSection);
                c.CollapseY(LocationSettingsSection);
                c.CollapseY(MoreOptionsSection);
                LoaderCircle.Hidden = true;

                ImageEditorFrameBorder.Layer.BorderColor = UIColor.FromName("PrimaryDark").CGColor;
                ImageEditorFrameBorder.Layer.BorderWidth = 1;

                rc = new RegisterCommonMethods(this, c, ImagesUploaded, Email, Username, Name, DescriptionText, CheckUsername, Images,
                ImagesProgressText, LoaderCircle, ImagesProgress, UseLocationSwitch, LocationShareAll, LocationShareLike, LocationShareMatch, LocationShareFriend, LocationShareNone,
                DistanceShareAll, DistanceShareLike, DistanceShareMatch, DistanceShareFriend, DistanceShareNone, ImageEditorControls, TopSeparator, RippleImageEditor, ImageEditorStatus, ImageEditorCancel, ImageEditorOK, ImageEditor, ImageEditorFrame, ImageEditorFrameBorder);

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);
                ProfileEditScroll.SetContext(this);

                ImagesUploaded.SetContext(this);
                ImagesUploaded.numColumns = 3; //it does not get passed in the layout file
                ImagesUploaded.tileSpacing = 2;

                AccountData.TouchUpInside += AccountData_Click;
                ChangePassword.TouchUpInside += ChangePassword_Click;
                LocationSettings.TouchUpInside += LocationSettings_Click;

                Save.TouchUpInside += Save_Click;
                Cancel.TouchUpInside += Cancel_Click;
                MoreOptions.TouchUpInside += MoreOptions_Click;
                DeactivateAccount.TouchUpInside += DeactivateAccount_Click;
                DeleteAccount.TouchUpInside += DeleteAccount_Click;

                ImageEditorCancel.TouchUpInside += rc.CancelImageEditing;
                ImageEditorOK.TouchUpInside += rc.OKImageEditing;

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackTopConstraint_Base = SnackTopConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;
                ScrollBottomConstraint_Base = ScrollBottomConstraint;
                ScrollBottomOuterConstraint_Base = ScrollBottomOuterConstraint;
                ViewportConstraint_Base = ViewportConstraint;
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

                SetSexChoice();
                Email.Text = Session.Email;
                Username.Text = Session.Username;
                Name.Text = Session.Name;
                rc.uploadedImages = new List<string>(Session.Pictures);

                int i;
                for (i = ImagesUploaded.Subviews.Length - 1; i >= 0; i--)
                {
                    ImagesUploaded.Subviews[i].RemoveFromSuperview();
                }
                ImagesUploaded.RefitImagesContainer();

                i = 0;
                foreach (string image in rc.uploadedImages)
                {
                    ImagesUploaded.AddPicture(image, i);
                    i++;
                }

                if (rc.uploadedImages.Count > 1)
                {
                    ImagesProgressText.Text = LangEnglish.ImagesRearrange;
                }
                else
                {
                    ImagesProgressText.Text = "";
                }

                //works only if activity is resuming. It is recreated when pressing cancel and coming here again. Can it happen?
                if (rc.imagesUploading)
                {
                    rc.StartAnim();
                }

                DescriptionText.Text = Session.Description;

                UseLocationSwitch.On = (bool)Session.UseLocation;
                rc.EnableLocationSwitches((bool)Session.UseLocation);
                rc.SetLocationShareLevel((byte)Session.LocationShare);
                rc.SetDistanceShareLevel((byte)Session.DistanceShare);

                if ((bool)Session.ActiveAccount)
                {
                    DeactivateAccount.SetTitle(LangEnglish.DeactivateAccount, UIControlState.Normal);
                }
                else
                {
                    DeactivateAccount.SetTitle(LangEnglish.ActivateAccount, UIControlState.Normal);
                }
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void ViewDidLayoutSubviews() //called after ViewWillTransitionToSize
        {
            dpWidth = UIScreen.MainScreen.Bounds.Width;
            dpHeight = UIScreen.MainScreen.Bounds.Height;

            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            safeAreaLeft = window.SafeAreaInsets.Left;
            safeAreaRight = window.SafeAreaInsets.Right;

            ImagesUploaded.SetTileSize();
            ImagesUploaded.Reposition();
            ImagesUploaded.RefitImagesContainer();

            base.ViewDidLayoutSubviews();
        }
        
        [Foundation.Export("textViewDidBeginEditing:")]
        public virtual void EditingStarted(UITextView textView)
        {
            ProfileEditScroll.ScrollRectToVisible(DescriptionText.Frame, true);
        }

        private void SetSexChoice()
        {
            switch (Session.SexChoice)
            {
                case 0:
                    Women.On = true;
                    Men.On = false;
                    break;
                case 1:
                    Women.On = false;
                    Men.On = true;
                    break;
                case 2:
                    Women.On = true;
                    Men.On = true;
                    break;
            }
        }

        private int GetSexChoice()
        {
            if (Women.On && !Men.On)
            {
                return 0;
            }
            else if (!Women.On && Men.On)
            {
                return 1;
            }
            return 2;
        }

        private void AccountData_Click(object sender, EventArgs e)
        {
            if (AccountDataSection.Frame.Height == 0)
            {
                c.ExpandY(AccountDataSection);
                c.CollapseY(ChangePasswordSection);
                c.CollapseY(LocationSettingsSection);
                c.CollapseY(MoreOptionsSection);
            }
            else
            {
                c.CollapseY(AccountDataSection);
            }            
        }

        private void ChangePassword_Click(object sender, EventArgs e)
        {
            if (ChangePasswordSection.Frame.Height == 0)
            {
                c.ExpandY(ChangePasswordSection);
                c.CollapseY(AccountDataSection);
                c.CollapseY(LocationSettingsSection);
                c.CollapseY(MoreOptionsSection);
            }
            else
            {
                c.CollapseY(ChangePasswordSection);
            }
        }

        private void LocationSettings_Click(object sender, EventArgs e)
        {
            if (LocationSettingsSection.Frame.Height == 0)
            {
                c.ExpandY(LocationSettingsSection);
                c.CollapseY(AccountDataSection);
                c.CollapseY(ChangePasswordSection);
                c.CollapseY(MoreOptionsSection);
            }
            else
            {
                c.CollapseY(LocationSettingsSection);
            }
        }

        private void MoreOptions_Click(object sender, EventArgs e)
        {
            if (MoreOptionsSection.Frame.Height == 0)
            {
                c.ExpandY(MoreOptionsSection);
                c.CollapseY(AccountDataSection);
                c.CollapseY(ChangePasswordSection);
                c.CollapseY(LocationSettingsSection);

                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });

                c.ScrollToBottom(ProfileEditScroll);
            }
            else
            {
                c.CollapseY(MoreOptionsSection);
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
            }
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                Save.Enabled = false;
                Save.Alpha = 0.5f;
                
                //not visible form fields do not get saved, but there is no need to reload the form, since we are exiting the activity on successful save.
                string requestStringBase = "action=profileedit&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                string requestStringAdd = "";
                if (DescriptionText.Text != Session.Description)
                {
                    requestStringAdd += "&Description=" + CommonMethods.UrlEncode(DescriptionText.Text);
                }
                if (GetSexChoice() != Session.SexChoice)
                {
                    requestStringAdd += "&SexChoice=" + GetSexChoice();
                }
                if (AccountDataSection.Frame.Height != 0)
                {
                    if (Email.Text.Trim() != Session.Email)
                    {
                        requestStringAdd += "&Email=" + CommonMethods.UrlEncode(Email.Text.Trim());
                    }
                    if (Username.Text.Trim() != Session.Username)
                    {
                        requestStringAdd += "&Username=" + CommonMethods.UrlEncode(Username.Text.Trim());
                    }
                    if (Name.Text.Trim() != Session.Name)
                    {
                        requestStringAdd += "&Name=" + CommonMethods.UrlEncode(Name.Text.Trim());
                    }
                }
                if (ChangePasswordSection.Frame.Height != 0)
                {
                    requestStringAdd += "&OldPassword=" + CommonMethods.UrlEncode(OldPassword.Text.Trim()) + "&Password=" + CommonMethods.UrlEncode(NewPassword.Text.Trim());
                }
                if (LocationSettingsSection.Frame.Height != 0)
                {
                    if (UseLocationSwitch.On != Session.UseLocation)
                    {
                        requestStringAdd += "&UseLocation=" + UseLocationSwitch.On;
                    }
                    int locationShare = rc.GetLocationShareLevel();
                    int distanceShare = rc.GetDistanceShareLevel();
                    if (locationShare != Session.LocationShare)
                    {
                        requestStringAdd += "&LocationShare=" + locationShare;
                    }
                    if (distanceShare != Session.DistanceShare)
                    {
                        requestStringAdd += "&DistanceShare=" + distanceShare;
                    }
                }

                if (requestStringAdd != "") //if the form was changed
                {
                    string responseString = await c.MakeRequest(requestStringBase + requestStringAdd);
                    if (responseString.Substring(0, 2) == "OK")
                    {
                        if (responseString.Length > 2) //a change happened
                        {
                            bool locationEnabled = false;
                            bool locationDisabled = false;

                            if (!(bool)Session.UseLocation && UseLocationSwitch.On)
                            {
                                locationEnabled = true;
                            }
                            else if ((bool)Session.UseLocation && !UseLocationSwitch.On)
                            {
                                locationDisabled = true;
                            }

                            if (GetSexChoice() != Session.SexChoice)
                            {
                                ListActivity.listProfiles.Clear();
                                Session.LastDataRefresh = null;
                            }
                            c.LoadCurrentUser(responseString);

                            if (locationEnabled)
                            {
                                if (locMgr is null)
                                {
                                    locMgr = new LocationManager(this); //first location update will load the list
                                }
                                locMgr.StartLocationUpdates();
                            }
                            else if (locationDisabled)
                            {
                                if (!(locMgr is null))
                                {
                                    locMgr.StopLocationUpdates();
                                }
                                if (!string.IsNullOrEmpty(locationUpdatesTo))
                                {
                                    EndLocationShare();
                                    locationUpdatesTo = null;
                                }
                                Session.Latitude = null;
                                Session.Longitude = null;
                                Session.LocationTime = null;

                                if (Constants.SafeLocationMode)
                                {
                                    Session.SafeLatitude = null;
                                    Session.SafeLongitude = null;
                                    Session.SafeLocationTime = null;

                                    Session.LatestLatitude = null;
                                    Session.LatestLongitude = null;
                                    Session.LatestLocationTime = null;
                                }

                                Session.LastDataRefresh = null; //For the situation that a logged in user is filtering by current location, and now turns off uselocation. SetDistanceSourceAddress will be called in ListActivity.OnResume
                            }
                        }
                        Session.SnackMessage = LangEnglish.SettingsUpdated;

                        Save.Enabled = true;
                        Save.Alpha = 1;
                        CommonMethods.OpenPage(null, 0);
                    }
                    else if (responseString.Substring(0, 6) == "ERROR_")
                    {
                        c.Snack(c.GetLang(responseString.Substring(6)));
                    }
                    else
                    {
                        c.ReportError(responseString);
                    }
                }
                else
                {
                    Save.Enabled = true;
                    Save.Alpha = 1;
                    CommonMethods.OpenPage(null, 0);
                }
                Save.Enabled = true;
                Save.Alpha = 1;
            }
            else
            {
                c.Snack(checkFormMessage);
            }
        }

        private bool CheckFields()
        {
            if (DescriptionText.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.DescriptionEmpty;
                DescriptionText.BecomeFirstResponder();
                return false;
            }
            if (DescriptionText.Text.Substring(DescriptionText.Text.Length-1) == "\\")
			{
				checkFormMessage = LangEnglish.DescriptionBackslash;
                DescriptionText.BecomeFirstResponder();
                return false;
			}

            if (AccountDataSection.Frame.Height != 0)
            {
                if (Email.Text.Trim() == "")
                {
                    checkFormMessage = LangEnglish.EmailEmpty;
                    Email.BecomeFirstResponder();
                    return false;
                }
                //If the extension is long, the regex will freeze the app.
                int lastDotPos = Email.Text.LastIndexOf(".");
                if (lastDotPos < Email.Text.Length - 5)
                {
                    checkFormMessage = LangEnglish.EmailWrong;
                    return false;
                }
                Regex regex = new Regex(Constants.EmailFormat); //when the email extension is long, it will take ages for the regex to finish
                if (!regex.IsMatch(Email.Text))
                {
                    checkFormMessage = LangEnglish.EmailWrong;
                    Email.BecomeFirstResponder();
                    return false;
                }
                if (Username.Text.Trim() == "")
                {
                    checkFormMessage = LangEnglish.UsernameEmpty;
                    Username.BecomeFirstResponder();
                    return false;
                }
                if (Username.Text.Trim().Substring(Username.Text.Trim().Length - 1) == "\\")
				{
					checkFormMessage = LangEnglish.UsernameBackslash;
                    Username.BecomeFirstResponder();
                    return false;
				}
                if (Name.Text.Trim() == "")
                {
                    checkFormMessage = LangEnglish.NameEmpty;
                    Name.BecomeFirstResponder();
                    return false;
                }
                if (Name.Text.Trim().Substring(Name.Text.Trim().Length - 1) == "\\")
				{
					checkFormMessage = LangEnglish.NameBackslash;
                    Name.BecomeFirstResponder();
                    return false;
				}
            }

            if (ChangePasswordSection.Frame.Height != 0)
            {
                if (OldPassword.Text.Trim().Length < 6)
                {
                    checkFormMessage = LangEnglish.PasswordShort;
                    OldPassword.BecomeFirstResponder();
                    return false;
                }
                if (NewPassword.Text.Trim().Length < 6)
                {
                    checkFormMessage = LangEnglish.PasswordShort;
                    NewPassword.BecomeFirstResponder();
                    return false;
                }
                if (OldPassword.Text.Trim() == NewPassword.Text.Trim())
                {
                    checkFormMessage = LangEnglish.PasswordNotChanged;
                    NewPassword.BecomeFirstResponder();
                    return false;
                }
                if (NewPassword.Text.Trim() != ConfirmPassword.Text.Trim())
                {
                    checkFormMessage = LangEnglish.ConfirmPasswordNoMatch;
                    ConfirmPassword.BecomeFirstResponder();
                    return false;
                }
            }
            return true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            CommonMethods.OpenPage(null, 0);
        }

        private async void DeactivateAccount_Click(object sender, EventArgs e)
        {
            if ((bool)Session.ActiveAccount)
            {
                c.DisplayCustomDialog(LangEnglish.ConfirmAction, LangEnglish.DialogDeactivate, LangEnglish.DialogOK, LangEnglish.DialogCancel, async alert =>
                {
                    DeactivateAccount.Enabled = false;
                    DeactivateAccount.Alpha = 0.5f;

                    string url = "action=deactivateaccount&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                    if (!string.IsNullOrEmpty(locationUpdatesTo))
                    {
                        url += "&LocationUpdates=" + locationUpdatesTo;
                        locationUpdatesTo = null;
                    }
                    string responseString = await c.MakeRequest(url);
                    if (responseString == "OK")
                    {
                        Session.ActiveAccount = false;
                        c.Snack(LangEnglish.Deactivated);
                        DeactivateAccount.SetTitle(LangEnglish.ActivateAccount, UIControlState.Normal);
                    }
                    else
                    {
                        c.ReportError(responseString);
                    }
                    DeactivateAccount.Enabled = true;
                    DeactivateAccount.Alpha = 1;
                }, null);
            }
            else
            {
                DeactivateAccount.Enabled = false;
                DeactivateAccount.Alpha = 0.5f;

                string responseString = await c.MakeRequest("action=activateaccount&ID=" + Session.ID + "&SessionID=" + Session.SessionID);
                if (responseString == "OK")
                {
                    Session.ActiveAccount = true;
                    c.Snack(LangEnglish.Activated);
                    DeactivateAccount.SetTitle(LangEnglish.DeactivateAccount, UIControlState.Normal);
                }
                else
                {
                    c.ReportError(responseString);
                }
                DeactivateAccount.Enabled = true;
                DeactivateAccount.Alpha = 1;
            }
        }

        private void DeleteAccount_Click(object sender, EventArgs e)
        {
            c.DisplayCustomDialog(LangEnglish.ConfirmAction, LangEnglish.DialogDelete, LangEnglish.DialogOK, LangEnglish.DialogCancel, async alert =>
            {
                DeleteAccount.Enabled = true;
                DeleteAccount.Alpha = 0.5f;

                string url = "action=deleteaccount&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                if (!string.IsNullOrEmpty(locationUpdatesTo))
                {
                    url += "&LocationUpdates=" + locationUpdatesTo;
                    locationUpdatesTo = null;
                }
                string responseString = await c.MakeRequest(url);
                if (responseString == "OK")
                {
                    IntentData.logout = true;
                    CommonMethods.OpenPage("MainActivity", 1);
                }
                else
                {
                    c.ReportError(responseString);
                }
                DeleteAccount.Enabled = true;
                DeleteAccount.Alpha = 1f;
            }, null);
        }
    }
}