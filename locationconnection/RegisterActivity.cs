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
    public partial class RegisterActivity : BaseActivity, IUITextViewDelegate
    {
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

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                SexLabel.Text = LangEnglish.Sex;
                EmailLabel.Text = LangEnglish.Email;
                EmailExplanationLabel.Text = LangEnglish.EmailExplanation;
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
                ImageEditorLabel.Text = LangEnglish.ImageEditorLabel;

                DescriptionText.Delegate = this;

                CheckUsername.Layer.MasksToBounds = true; //required for presevering corner radius for highlighted state
                Images.Layer.MasksToBounds = true;
                Register.Layer.MasksToBounds = true;
                Reset.Layer.MasksToBounds = true;
                RegisterCancel.Layer.MasksToBounds = true;

                Sex.Model = new DropDownList(LangEnglish.SexEntries, "Sex", 120, this);
                c.DrawBorder(DescriptionText);
                c.DrawBorder(EulaText);

                EulaLabel.Text = LangEnglish.EulaLabel;
                EulaText.Text = LangEnglish.EulaText;

                LoaderCircle.Hidden = true;

                ImageEditorFrameBorder.Layer.BorderColor = UIColor.Black.CGColor;
                ImageEditorFrameBorder.Layer.BorderWidth = 1;

                rc = new RegisterCommonMethods(this, c, ImagesUploaded, Email, Username, Name, DescriptionText, CheckUsername, Images,
                ImagesProgressText, LoaderCircle, ImagesProgress, UseLocationSwitch, LocationShareAll, LocationShareLike, LocationShareMatch, LocationShareFriend, LocationShareNone,
                DistanceShareAll, DistanceShareLike, DistanceShareMatch, DistanceShareFriend, DistanceShareNone, ImageEditorControls, TopSeparator, RippleImageEditor, ImageEditorStatus, ImageEditorCancel, ImageEditorOK, ImageEditor, ImageEditorFrame, ImageEditorFrameBorder);

                RegisterScroll.SetContext(this);

                ImagesUploaded.SetContext(this);
                ImagesUploaded.numColumns = 5; //it does not get passed in the layout file
                ImagesUploaded.tileSpacing = 2;

                if (!File.Exists(regSessionFile))
                {
                    regsessionid = "";
                }
                else
                {
                    regsessionid = File.ReadAllText(regSessionFile);
                }                

                Register.TouchUpInside += Register_Click;
                Reset.TouchUpInside += Reset_Click;
                RegisterCancel.TouchUpInside += RegisterCancel_Click;

                ImageEditorCancel.TouchUpInside += rc.CancelImageEditing;
                ImageEditorOK.TouchUpInside += rc.OKImageEditing;

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

        public async override void ViewWillAppear(bool animated)
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
                    string[] arr = File.ReadAllLines(regSaveFile);
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

                    DescriptionText.Text = arr[7].Replace("[newline]", "\n");

                    UseLocationSwitch.On = bool.Parse(arr[8]);
                    rc.EnableLocationSwitches(UseLocationSwitch.On);
                    rc.SetLocationShareLevel(byte.Parse(arr[9]));
                    rc.SetDistanceShareLevel(byte.Parse(arr[10]));
                }
                else //in case we are stepping back from a successful registration
                {
                    ResetForm();
                }

                string responseString = await c.MakeRequest("action=eula"); //deleting images from server
                if (responseString.Substring(0, 2) == "OK")
                {
                    NSError error = null;
                    string s = "<span style=\"font-family: '-apple-system', 'HelveticaNeue'; font-size: 12px\">" + responseString.Substring(3) + "</span>";
                    var htmlString = new NSAttributedString(s, new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML }, ref error);
                    EulaText.AttributedText = htmlString;
                }
                else
                {
                    c.ReportError(responseString);
                }
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void ViewDidLayoutSubviews() //called after ViewWillTransitionToSize
        {
            c.CW("ViewDidLayoutSubviews");

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

        [Export("textViewDidBeginEditing:")]
        public virtual void EditingStarted(UIKit.UITextView textView)
        {
            RegisterScroll.ScrollRectToVisible(DescriptionText.Frame, true);
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
                    + "&Pictures=" + c.UrlEncode(string.Join("|", rc.uploadedImages)) + "&Description=" + c.UrlEncode(DescriptionText.Text) + "&UseLocation=" + UseLocationSwitch.On
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
            if (Username.Text.Trim().Substring(Username.Text.Trim().Length - 1) == "\\")
			{
				checkFormMessage = LangEnglish.UsernameBackslash;
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
            if (DescriptionText.Text.Substring(DescriptionText.Text.Length - 1) == "\\")
			{
				checkFormMessage =LangEnglish.DescriptionBackslash;
				return false;
			}
            return true;
        }

        private async void Reset_Click(object sender, EventArgs e)
        {
            View.EndEditing(true);

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
            File.WriteAllText(regSaveFile, Sex.SelectedRowInComponent(0) + "\n" + Email.Text.Trim() + "\n" + Password.Text.Trim() + "\n" + ConfirmPassword.Text.Trim()
                    + "\n" + Username.Text.Trim() + "\n" + Name.Text.Trim()
                    + "\n" + string.Join("|", rc.uploadedImages) + "\n" + DescriptionText.Text.Trim().Replace("\n", "[newline]")
                    + "\n" + UseLocationSwitch.On + "\n" + rc.GetLocationShareLevel() + "\n" + rc.GetDistanceShareLevel());
        }

        private void ResetForm()
        {
            if (File.Exists(regSaveFile))
            {
                File.Delete(regSaveFile);
            }

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

            UIView.Animate(Constants.tweenTime, () => { RegisterScroll.ContentOffset = new CGPoint(0,0); }, () => { });
        }
    }
}

