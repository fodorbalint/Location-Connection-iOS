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

                GetScreenMetrics();

                EditImages.SetTitle(LangEnglish.Images, UIControlState.Normal);
                EditImagesProgressText.Text = "";
                EditDescriptionLabel.Text = LangEnglish.Description;
                EditSexLabel.Text = LangEnglish.EditSex;
                WomenLabel.Text = LangEnglish.Women;
                MenLabel.Text = LangEnglish.Men;

                EditAccountData.SetTitle(LangEnglish.EditAccountData, UIControlState.Normal);
                EditChangePassword.SetTitle(LangEnglish.EditChangePassword, UIControlState.Normal);
                EditLocationSettings.SetTitle(LangEnglish.EditLocationSettings, UIControlState.Normal);

                EditSave.SetTitle(LangEnglish.EditSave, UIControlState.Normal);
                EditCancel.SetTitle(LangEnglish.Cancel, UIControlState.Normal);

                EditMoreOptions.SetTitle(LangEnglish.EditMoreOptions, UIControlState.Normal);

                EditEmailLabel.Text = LangEnglish.Email;
                EditUsernameLabel.Text = LangEnglish.Username;
                EditCheckUsername.SetTitle(LangEnglish.CheckAvailability, UIControlState.Normal);
                EditNameLabel.Text = LangEnglish.Name;

                EditOldPasswordLabel.Text = LangEnglish.EditOldPassword;
                EditNewPasswordLabel.Text = LangEnglish.EditNewPassword;
                EditConfirmPasswordLabel.Text = LangEnglish.EditConfirmPassword;

                EditUseLocationLabel.Text = LangEnglish.UseLocation;
                EditLocationShareLabel.Text = LangEnglish.LocationShare;
                EditLocationShareAllLabel.Text = LangEnglish.LocationShareAll;
                EditLocationShareLikeLabel.Text = LangEnglish.LocationShareLike;
                EditLocationShareMatchLabel.Text = LangEnglish.LocationShareMatch;
                EditLocationShareFriendLabel.Text = LangEnglish.LocationShareFriend;
                EditLocationShareNoneLabel.Text = LangEnglish.LocationShareNone;
                EditDistanceShareLabel.Text = LangEnglish.DistanceShare;
                EditDistanceShareAllLabel.Text = LangEnglish.DistanceShareAll;
                EditDistanceShareLikeLabel.Text = LangEnglish.DistanceShareLike;
                EditDistanceShareMatchLabel.Text = LangEnglish.DistanceShareMatch;
                EditDistanceShareFriendLabel.Text = LangEnglish.DistanceShareFriend;
                EditDistanceShareNoneLabel.Text = LangEnglish.DistanceShareNone;

                DeactivateAccount.SetTitle(LangEnglish.DeactivateAccount, UIControlState.Normal);
                DeleteAccount.SetTitle(LangEnglish.DeleteAccount, UIControlState.Normal);

                EditDescriptionText.Delegate = this;

                EditImages.Layer.MasksToBounds = true;
                EditSave.Layer.MasksToBounds = true;
                EditCancel.Layer.MasksToBounds = true;
                DeactivateAccount.Layer.MasksToBounds = true;
                DeleteAccount.Layer.MasksToBounds = true;

                c.DrawBorder(EditDescriptionText);

                c.CollapseY(EditAccountDataSection);
                c.CollapseY(EditChangePasswordSection);
                c.CollapseY(EditLocationSettingsSection);
                c.CollapseY(EditMoreOptionsSection);
                EditLoaderCircle.Hidden = true;

                rc = new RegisterCommonMethods(this, c, EditImagesUploaded, EditEmail, EditUsername, EditName, EditDescriptionText, EditCheckUsername, EditImages,
                EditImagesProgressText, EditLoaderCircle, EditImagesProgress, EditUseLocationSwitch, EditLocationShareAll, EditLocationShareLike, EditLocationShareMatch, EditLocationShareFriend, EditLocationShareNone,
                EditDistanceShareAll, EditDistanceShareLike, EditDistanceShareMatch, EditDistanceShareFriend, EditDistanceShareNone, null, null, null, null, null, null, null);

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);
                ProfileEditScroll.SetContext(this);

                EditImagesUploaded.SetContext(this);
                EditImagesUploaded.numColumns = 3; //it does not get passed in the layout file
                EditImagesUploaded.tileSpacing = 2;
                EditImagesUploaded.SetTileSize();

                EditImages.TouchUpInside += rc.Images_Click;

                EditAccountData.TouchUpInside += EditAccountData_Click;
                EditChangePassword.TouchUpInside += EditChangePassword_Click;
                EditLocationSettings.TouchUpInside += EditLocationSettings_Click;

                EditCheckUsername.TouchUpInside += rc.CheckUsername_Click;

                EditUseLocationSwitch.TouchUpInside += rc.UseLocationSwitch_Click;
                EditLocationShareAll.TouchUpInside += rc.LocationShareAll_Click;
                EditLocationShareLike.TouchUpInside += rc.LocationShareLike_Click;
                EditLocationShareMatch.TouchUpInside += rc.LocationShareMatch_Click;
                EditLocationShareFriend.TouchUpInside += rc.LocationShareFriend_Click;
                EditLocationShareNone.TouchUpInside += rc.LocationShareNone_Click;

                EditDistanceShareAll.TouchUpInside += rc.DistanceShareAll_Click;
                EditDistanceShareLike.TouchUpInside += rc.DistanceShareLike_Click;
                EditDistanceShareMatch.TouchUpInside += rc.DistanceShareMatch_Click;
                EditDistanceShareFriend.TouchUpInside += rc.DistanceShareFriend_Click;
                EditDistanceShareNone.TouchUpInside += rc.DistanceShareNone_Click;

                EditSave.TouchUpInside += EditSave_Click;
                EditCancel.TouchUpInside += EditCancel_Click;
                EditMoreOptions.TouchUpInside += EditMoreOptions_Click;
                DeactivateAccount.TouchUpInside += DeactivateAccount_Click;
                DeleteAccount.TouchUpInside += DeleteAccount_Click;

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackTopConstraint_Base = SnackTopConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;
                ScrollBottomConstraint_Base = ScrollBottomConstraint;
                ScrollBottomOuterConstraint_Base = ScrollBottomOuterConstraint;
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
                EditEmail.Text = Session.Email;
                EditUsername.Text = Session.Username;
                EditName.Text = Session.Name;
                rc.uploadedImages = new List<string>(Session.Pictures);

                int i;
                for (i = EditImagesUploaded.Subviews.Length - 1; i >= 0; i--)
                {
                    EditImagesUploaded.Subviews[i].RemoveFromSuperview();
                }
                EditImagesUploaded.RefitImagesContainer();

                i = 0;
                foreach (string image in rc.uploadedImages)
                {
                    EditImagesUploaded.AddPicture(image, i);
                    i++;
                }

                if (rc.uploadedImages.Count > 1)
                {
                    EditImagesProgressText.Text = LangEnglish.ImagesRearrange;
                }
                else
                {
                    EditImagesProgressText.Text = "";
                }

                //works only if activity is resuming. It is recreated when pressing cancel and coming here again. Can it happen?
                if (rc.imagesUploading)
                {
                    rc.StartAnim();
                }

                EditDescriptionText.Text = Session.Description;

                EditUseLocationSwitch.On = (bool)Session.UseLocation;
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

        [Foundation.Export("textViewDidBeginEditing:")]
        public virtual void EditingStarted(UITextView textView)
        {
            ProfileEditScroll.ScrollRectToVisible(EditDescriptionText.Frame, true);
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

        private void EditAccountData_Click(object sender, EventArgs e)
        {
            if (EditAccountDataSection.Frame.Height == 0)
            {
                c.ExpandY(EditAccountDataSection);
                c.CollapseY(EditChangePasswordSection);
                c.CollapseY(EditLocationSettingsSection);
                c.CollapseY(EditMoreOptionsSection);
            }
            else
            {
                c.CollapseY(EditAccountDataSection);
            }            
        }

        private void EditChangePassword_Click(object sender, EventArgs e)
        {
            if (EditChangePasswordSection.Frame.Height == 0)
            {
                c.ExpandY(EditChangePasswordSection);
                c.CollapseY(EditAccountDataSection);
                c.CollapseY(EditLocationSettingsSection);
                c.CollapseY(EditMoreOptionsSection);
            }
            else
            {
                c.CollapseY(EditChangePasswordSection);
            }
        }

        private void EditLocationSettings_Click(object sender, EventArgs e)
        {
            if (EditLocationSettingsSection.Frame.Height == 0)
            {
                c.ExpandY(EditLocationSettingsSection);
                c.CollapseY(EditAccountDataSection);
                c.CollapseY(EditChangePasswordSection);
                c.CollapseY(EditMoreOptionsSection);
            }
            else
            {
                c.CollapseY(EditLocationSettingsSection);
            }
        }

        private void EditMoreOptions_Click(object sender, EventArgs e)
        {
            if (EditMoreOptionsSection.Frame.Height == 0)
            {
                c.ExpandY(EditMoreOptionsSection);
                c.CollapseY(EditAccountDataSection);
                c.CollapseY(EditChangePasswordSection);
                c.CollapseY(EditLocationSettingsSection);

                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });

                c.ScrollToBottom(ProfileEditScroll);
            }
            else
            {
                c.CollapseY(EditMoreOptionsSection);
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
            }
        }

        private async void EditSave_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                EditSave.Enabled = false;
                EditSave.Alpha = 0.5f;
                
                //not visible form fields do not get saved, but there is no need to reload the form, since we are exiting the activity on successful save.
                string requestStringBase = "action=profileedit&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                string requestStringAdd = "";
                if (EditDescriptionText.Text != Session.Description)
                {
                    requestStringAdd += "&Description=" + c.UrlEncode(EditDescriptionText.Text);
                    c.CW("req-" + requestStringAdd + "-------------------");
                }
                if (GetSexChoice() != Session.SexChoice)
                {
                    requestStringAdd += "&SexChoice=" + GetSexChoice();
                }
                if (EditAccountDataSection.Frame.Height != 0)
                {
                    if (EditEmail.Text.Trim() != Session.Email)
                    {
                        requestStringAdd += "&Email=" + c.UrlEncode(EditEmail.Text.Trim());
                    }
                    if (EditUsername.Text.Trim() != Session.Username)
                    {
                        requestStringAdd += "&Username=" + c.UrlEncode(EditUsername.Text.Trim());
                    }
                    if (EditName.Text.Trim() != Session.Name)
                    {
                        requestStringAdd += "&Name=" + c.UrlEncode(EditName.Text.Trim());
                    }
                }
                if (EditChangePasswordSection.Frame.Height != 0)
                {
                    requestStringAdd += "&OldPassword=" + c.UrlEncode(EditOldPassword.Text.Trim()) + "&Password=" + c.UrlEncode(EditNewPassword.Text.Trim());
                }
                if (EditLocationSettingsSection.Frame.Height != 0)
                {
                    if (EditUseLocationSwitch.On != Session.UseLocation)
                    {
                        requestStringAdd += "&UseLocation=" + EditUseLocationSwitch.On;
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

                            if (!(bool)Session.UseLocation && EditUseLocationSwitch.On)
                            {
                                locationEnabled = true;
                            }
                            else if ((bool)Session.UseLocation && !EditUseLocationSwitch.On)
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
                            }
                        }
                        Session.SnackMessage = LangEnglish.SettingsUpdated;

                        EditSave.Enabled = true;
                        EditSave.Alpha = 1;
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
                    EditSave.Enabled = true;
                    EditSave.Alpha = 1;
                    CommonMethods.OpenPage(null, 0);
                }
                EditSave.Enabled = true;
                EditSave.Alpha = 1;
            }
            else
            {
                c.Snack(checkFormMessage);
            }
        }

        private bool CheckFields()
        {
            if (EditDescriptionText.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.DescriptionEmpty;
                EditDescriptionText.BecomeFirstResponder();
                return false;
            }
            if (EditDescriptionText.Text.Substring(EditDescriptionText.Text.Length-1) == "\\")
			{
				checkFormMessage = LangEnglish.DescriptionBackslash;
                EditDescriptionText.BecomeFirstResponder();
                return false;
			}

            if (EditAccountDataSection.Frame.Height != 0)
            {
                if (EditEmail.Text.Trim() == "")
                {
                    checkFormMessage = LangEnglish.EmailEmpty;
                    EditEmail.BecomeFirstResponder();
                    return false;
                }
                //If the extension is long, the regex will freeze the app.
                int lastDotPos = EditEmail.Text.LastIndexOf(".");
                if (lastDotPos < EditEmail.Text.Length - 5)
                {
                    checkFormMessage = LangEnglish.EmailWrong;
                    return false;
                }
                Regex regex = new Regex(Constants.EmailFormat); //when the email extension is long, it will take ages for the regex to finish
                if (!regex.IsMatch(EditEmail.Text))
                {
                    checkFormMessage = LangEnglish.EmailWrong;
                    EditEmail.BecomeFirstResponder();
                    return false;
                }
                if (EditUsername.Text.Trim() == "")
                {
                    checkFormMessage = LangEnglish.UsernameEmpty;
                    EditUsername.BecomeFirstResponder();
                    return false;
                }
                if (EditUsername.Text.Trim().Substring(EditUsername.Text.Trim().Length - 1) == "\\")
				{
					checkFormMessage = LangEnglish.UsernameBackslash;
                    EditUsername.BecomeFirstResponder();
                    return false;
				}
                if (EditName.Text.Trim() == "")
                {
                    checkFormMessage = LangEnglish.NameEmpty;
                    EditName.BecomeFirstResponder();
                    return false;
                }
                if (EditName.Text.Trim().Substring(EditName.Text.Trim().Length - 1) == "\\")
				{
					checkFormMessage = LangEnglish.NameBackslash;
                    EditName.BecomeFirstResponder();
                    return false;
				}
            }

            if (EditChangePasswordSection.Frame.Height != 0)
            {
                if (EditOldPassword.Text.Trim().Length < 6)
                {
                    checkFormMessage = LangEnglish.PasswordShort;
                    EditOldPassword.BecomeFirstResponder();
                    return false;
                }
                if (EditNewPassword.Text.Trim().Length < 6)
                {
                    checkFormMessage = LangEnglish.PasswordShort;
                    EditNewPassword.BecomeFirstResponder();
                    return false;
                }
                if (EditOldPassword.Text.Trim() == EditNewPassword.Text.Trim())
                {
                    checkFormMessage = LangEnglish.PasswordNotChanged;
                    EditNewPassword.BecomeFirstResponder();
                    return false;
                }
                if (EditNewPassword.Text.Trim() != EditConfirmPassword.Text.Trim())
                {
                    checkFormMessage = LangEnglish.ConfirmPasswordNoMatch;
                    EditConfirmPassword.BecomeFirstResponder();
                    return false;
                }
            }
            return true;
        }

        private void EditCancel_Click(object sender, EventArgs e)
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