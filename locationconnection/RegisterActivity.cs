using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public partial class RegisterActivity : BaseActivity
    {
        
        public static string regSessionFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "regsession.txt");
        private string regSaveFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "regsave.txt");

        public RegisterCommonMethods rc;

        public static string regsessionid; //for use in UploadedListAdapter
        private string checkFormMessage;
        private bool registerCompleted;

        public ImageFrameLayout ImagesUploaded { get { return RegisterImagesUploaded; } }
        public TouchScrollView RegisterScroll { get { return Register_Scroll; } }

        public RegisterActivity(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                GetScreenMetrics();
                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                SexLabel.Text = LangEnglish.Sex;
                EmailLabel.Text = LangEnglish.Email;
                PasswordLabel.Text = LangEnglish.Password;
                ConfirmPasswordLabel.Text = LangEnglish.ConfirmPassword;
                UsernameLabel.Text = LangEnglish.Username;
                CheckUsername.SetTitle(LangEnglish.CheckAvailability, UIControlState.Normal);
                NameLabel.Text = LangEnglish.Name;
                Images.SetTitle(LangEnglish.Images, UIControlState.Normal);
                ImagesProgressText.Text = "";
                DescriptionLabel.Text = LangEnglish.Description;
                UseLocationLabel.Text = LangEnglish.UseLocation;
                LocationExplanation.Text = LangEnglish.LocationExplanation;
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
                Register.SetTitle(LangEnglish.Register, UIControlState.Normal);
                Reset.SetTitle(LangEnglish.Reset, UIControlState.Normal);
                RegisterCancel.SetTitle(LangEnglish.Cancel, UIControlState.Normal);

                CheckUsername.Layer.MasksToBounds = true; //required for presevering corner radius for highlighted state
                Images.Layer.MasksToBounds = true;
                Register.Layer.MasksToBounds = true;
                Reset.Layer.MasksToBounds = true;
                RegisterCancel.Layer.MasksToBounds = true;

                Sex.Model = new DropDownList(LangEnglish.SexEntries, "Sex", 120, this);
                c.DrawBorder(DescriptionText);

                LoaderCircle.Hidden = true;

                rc = new RegisterCommonMethods(this, c, ImagesUploaded, Email, Username, Name, DescriptionText, CheckUsername, Images,
                ImagesProgressText, LoaderCircle, ImagesProgress, UseLocationSwitch, LocationShareAll, LocationShareLike, LocationShareMatch, LocationShareFriend, LocationShareNone,
                DistanceShareAll, DistanceShareLike, DistanceShareMatch, DistanceShareFriend, DistanceShareNone);

                RegisterScroll.SetContext(this);

                ImagesUploaded.SetContext(this);
                ImagesUploaded.numColumns = 5; //it does not get passed in the layout file
                ImagesUploaded.tileSpacing = 2;
                ImagesUploaded.SetTileSize();

                if (!File.Exists(regSessionFile))
                {
                    regsessionid = "";
                }
                else
                {
                    regsessionid = File.ReadAllText(regSessionFile);
                }

                CheckUsername.TouchUpInside += rc.CheckUsername_Click;
                Images.TouchUpInside += rc.Images_Click;

                UseLocationSwitch.TouchUpInside += rc.UseLocationSwitch_Click;
                LocationShareAll.TouchUpInside += rc.LocationShareAll_Click;
                LocationShareLike.TouchUpInside += rc.LocationShareLike_Click;
                LocationShareMatch.TouchUpInside += rc.LocationShareMatch_Click;
                LocationShareFriend.TouchUpInside += rc.LocationShareFriend_Click;
                LocationShareNone.TouchUpInside += rc.LocationShareNone_Click;

                DistanceShareAll.TouchUpInside += rc.DistanceShareAll_Click;
                DistanceShareLike.TouchUpInside += rc.DistanceShareLike_Click;
                DistanceShareMatch.TouchUpInside += rc.DistanceShareMatch_Click;
                DistanceShareFriend.TouchUpInside += rc.DistanceShareFriend_Click;
                DistanceShareNone.TouchUpInside += rc.DistanceShareNone_Click;

                Register.TouchUpInside += Register_Click;
                Reset.TouchUpInside += Reset_Click;
                RegisterCancel.TouchUpInside += RegisterCancel_Click;

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

                if (!(ListActivity.listProfiles is null))
                {
                    ListActivity.listProfiles.Clear();
                    ListActivity.totalResultCount = null;
                }

                Session.LastDataRefresh = null;
                Session.LocationTime = null;

                registerCompleted = false;

                if (File.Exists(regSaveFile))
                {
                    string content = File.ReadAllText(regSaveFile);
                    string[] arr = content.Split(";");
                    Sex.Select(int.Parse(arr[0]), 0, false);
                    Email.Text = arr[1];
                    Password.Text = arr[2];
                    ConfirmPassword.Text = arr[3];
                    Username.Text = arr[4];
                    Name.Text = arr[5];
                    if (arr[6] != "") //it would give one element
                    {
                        string[] images = arr[6].Split("|");
                        rc.uploadedImages = new List<string>(images);
                    }
                    else
                    {
                        rc.uploadedImages = new List<string>();
                    }

                    int i;
                    for (i = ImagesUploaded.Subviews.Length - 1; i >= 0; i--)
                    {
                        ImagesUploaded.Subviews[i].RemoveFromSuperview();
                    }
                    ImagesUploaded.RefitImagesContainer();
                    //ImagesUploaded.drawOrder = new List<int>();

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

                    Console.WriteLine("Onresume: imagesuploading: " + rc.imagesUploading);

                    //works only if activity is resuming. It is recreaed when pressing cancel and coming here again.
                    if (rc.imagesUploading)
                    {
                        rc.StartAnim();
                    }

                    DescriptionText.Text = arr[7];

                    UseLocationSwitch.On = bool.Parse(arr[8]);
                    rc.EnableLocationSwitches(UseLocationSwitch.On);
                    rc.SetLocationShareLevel(byte.Parse(arr[9]));
                    rc.SetDistanceShareLevel(byte.Parse(arr[10]));
                }
                else //in case we are stepping back from a successful registration
                {
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            Console.WriteLine("ViewWillDisappear");
            try
            {
                if (!registerCompleted)
                {
                    SaveRegData();
                }
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            GetScreenMetrics();
            ImagesUploaded.SetTileSize();
            ImagesUploaded.Reposition();
            ImagesUploaded.RefitImagesContainer();

            base.ViewWillTransitionToSize(toSize, coordinator);
        }

        private async void Register_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                View.EndEditing(true);

                Register.Enabled = false;
                Register.Alpha = 0.5f;
                int locationShare = 0;
                int distanceShare = 0;

                if (UseLocationSwitch.On)
                {
                    locationShare = rc.GetLocationShareLevel();
                    distanceShare = rc.GetDistanceShareLevel();
                }

                string url = "action=register&Sex=" + (Sex.SelectedRowInComponent(0) - 1) + "&Email=" + c.UrlEncode(Email.Text.Trim()) + "&Password=" + c.UrlEncode(Password.Text.Trim())
                    + "&Username=" + c.UrlEncode(Username.Text.Trim()) + "&Name=" + c.UrlEncode(Name.Text.Trim())
                    + "&Pictures=" + c.UrlEncode(string.Join("|", rc.uploadedImages)) + "&Description=" + c.UrlEncode(DescriptionText.Text.Trim()) + "&UseLocation=" + UseLocationSwitch.On
                    + "&LocationShare=" + locationShare + "&DistanceShare=" + distanceShare + "&regsessionid=" + regsessionid;

                if (File.Exists(deviceTokenFile)) //sends the token whether it was sent from this device or not
                {
                    url += "&token=" + File.ReadAllText(deviceTokenFile) + "&ios=1";
                }
                string responseString = await c.MakeRequest(url);
                if (responseString.Substring(0, 2) == "OK")
                {
                    if (File.Exists(regSessionFile))
                    {
                        File.Delete(regSessionFile);
                    }
                    regsessionid = "";
                    if (File.Exists(regSaveFile))
                    {
                        File.Delete(regSaveFile);
                    }
                    registerCompleted = true; //to prevent OnPause from saving form data.

                    if (File.Exists(deviceTokenFile))
                    {
                        File.WriteAllText(tokenUptoDateFile, "True");
                    }

                    c.LoadCurrentUser(responseString);

                    Register.Enabled = true;
                    Register.Alpha = 1;

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
                Register.Enabled = true;
                Register.Alpha = 1;
            }
            else
            {
                c.Snack(checkFormMessage);
            }
        }

        private bool CheckFields() //need to resize window on keyboard appereance, otherwise Snackbar will be covered
        {
            if (Sex.SelectedRowInComponent(0) == 0)
            {
                checkFormMessage = LangEnglish.SexEmpty;
                Sex.BecomeFirstResponder();
                return false;
            }
            if (Email.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.EmailEmpty;
                Email.BecomeFirstResponder();
                return false;
            }
            int lastDotPos = Email.Text.LastIndexOf(".");
            if (lastDotPos < Email.Text.Length - 5)
            {
                checkFormMessage = LangEnglish.EmailWrong;
                Email.BecomeFirstResponder();
                return false;
            }
            //If the extension is long, the regex will freeze the app.
            Regex regex = new Regex(Constants.EmailFormat);
            if (!regex.IsMatch(Email.Text))
            {
                checkFormMessage = LangEnglish.EmailWrong;
                Email.BecomeFirstResponder();
                return false;
            }
            if (Password.Text.Trim().Length < 6)
            {
                checkFormMessage = LangEnglish.PasswordShort;
                Password.BecomeFirstResponder();
                return false;
            }
            if (Password.Text.Trim() != ConfirmPassword.Text.Trim())
            {
                checkFormMessage = LangEnglish.ConfirmPasswordNoMatch;
                ConfirmPassword.BecomeFirstResponder();
                return false;
            }
            if (Username.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.UsernameEmpty;
                Username.BecomeFirstResponder();
                return false;
            }
            if (Name.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.NameEmpty;
                Name.BecomeFirstResponder();
                return false;
            }
            if (rc.uploadedImages.Count == 0)
            {
                checkFormMessage = LangEnglish.ImagesEmpty;
                Images.BecomeFirstResponder();
                return false;
            }
            if (DescriptionText.Text.Trim() == "")
            {
                checkFormMessage = LangEnglish.DescriptionEmpty;
                DescriptionText.BecomeFirstResponder();
                return false;
            }
            return true;
        }

        private async void Reset_Click(object sender, EventArgs e)
        {
            Reset.Enabled = false;
            Reset.Alpha = 0.5f;

            if (regsessionid != "")
            {
                string responseString = await c.MakeRequest("action=deletetemp&imageName=&regsessionid=" + regsessionid); //deleting images from server
                if (responseString == "OK" || responseString == "INVALID_TOKEN")
                {
                    if (File.Exists(regSessionFile))
                    {
                        File.Delete(regSessionFile);
                    }
                    regsessionid = "";
                    if (File.Exists(regSaveFile))
                    {
                        File.Delete(regSaveFile);
                    }
                    ResetForm();
                }
                else
                {
                    c.ReportError(responseString);
                }
            }
            else
            {
                ResetForm();
            }
            
            Reset.Enabled = true;
            Reset.Alpha = 1;
        }

        private void RegisterCancel_Click(object sender, EventArgs e)
        {
            CommonMethods.OpenPage(null, 0);
        }

        public void SaveRegData()
        {
            File.WriteAllText(regSaveFile, Sex.SelectedRowInComponent(0) + ";" + Email.Text.Trim() + ";" + Password.Text.Trim() + ";" + ConfirmPassword.Text.Trim()
                    + ";" + Username.Text.Trim() + ";" + Name.Text.Trim()
                    + ";" + string.Join("|", rc.uploadedImages) + ";" + DescriptionText.Text.Trim()
                    + ";" + UseLocationSwitch.On + ";" + rc.GetLocationShareLevel() + ";" + rc.GetDistanceShareLevel());
        }

        private void ResetForm()
        {
            Sex.Select(0, 0, true);
            Email.Text = "";
            Password.Text = "";
            ConfirmPassword.Text = "";
            Username.Text = "";
            Name.Text = "";
            DescriptionText.Text = "";
            rc.uploadedImages = new List<string>();
            for (int i = ImagesUploaded.Subviews.Length - 1; i >= 0; i--)
            {
                ImagesUploaded.Subviews[i].RemoveFromSuperview();
            }
            ImagesUploaded.RefitImagesContainer();
            DescriptionText.Text = "";

            ImagesProgressText.Text = "";
            ImagesProgress.Progress = 0;

            UseLocationSwitch.On = false;

            LocationShareAll.On = false;
            LocationShareLike.On = false;
            LocationShareMatch.On = false;
            LocationShareFriend.On = false;
            LocationShareNone.On = true;

            DistanceShareAll.On = false;
            DistanceShareLike.On = false;
            DistanceShareMatch.On = false;
            DistanceShareFriend.On = false;
            DistanceShareNone.On = true;

            rc.EnableLocationSwitches(false);
        }
    }
}

