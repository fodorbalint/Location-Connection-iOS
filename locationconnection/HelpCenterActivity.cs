using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace LocationConnection
{
    public partial class HelpCenterActivity : BaseActivity, IUIScrollViewDelegate
    {
        private List<string> tutorialDescriptions;
        private List<string> tutorialPictures;
        private int currentTutorial;
        public bool cancelImageLoading;

        public HelpCenterActivity (IntPtr handle) : base (handle)
        {
        }

        public async override void ViewDidLoad()
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

                HelpCenterHeaderText.Text = LangEnglish.MenuHelpCenter;
                HelpCenterFormCaption.SetTitle(LangEnglish.HelpCenterFormCaption, UIControlState.Normal);
                MessageSend.SetTitle(LangEnglish.HelpCenterFormSend, UIControlState.Normal);
                OpenTutorial.SetTitle(LangEnglish.HelpCenterTutorial, UIControlState.Normal);
                TutorialFrame.Delegate = this;

                MessageSend.Layer.MasksToBounds = true;

                c.DrawBorder(MessageEdit);
                c.CollapseY(MessageContainer);

                HelpCenterBack.TouchDown += HelpCenterBack_TouchDown;
                HelpCenterBack.TouchUpInside += HelpCenterBack_TouchUpInside;
                OpenTutorial.TouchUpInside += OpenTutorial_TouchUpInside;
                HelpCenterFormCaption.TouchUpInside += HelpCenterFormCaption_TouchUpInside;
                MessageSend.TouchUpInside += MessageSend_TouchUpInside;

                TutorialBack.TouchDown += TutorialBack_TouchDown;
                TutorialBack.TouchUpInside += TutorialBack_TouchUpInside;
                TutorialLoadNext.TouchDown += TutorialLoad_TouchDown;
                TutorialLoadNext.TouchUpInside += TutorialLoadNext_TouchUpInside;
                TutorialLoadPrevious.TouchDown += TutorialLoad_TouchDown;
                TutorialLoadPrevious.TouchUpInside += TutorialLoadPrevious_TouchUpInside;
                
                string responseString = await c.MakeRequest("action=helpcenter");

                if (responseString.Substring(0, 2) == "OK")
                {
                    c.RemoveSubviews(QuestionsScroll);

                    responseString = responseString.Substring(3);
                    string[] lines = responseString.Split("\t");
                    int count = 0;
                    foreach (string line in lines)
                    {
                        count++;
                        UILabel text = new UILabel();
                        QuestionsScroll.AddSubview(text);

                        text.Text = line;
                        text.TextColor = UIColor.Black;
                        text.Lines = 0;
                        text.LineBreakMode = UILineBreakMode.WordWrap;

                        if (count % 2 == 1) //question, change font weight
                        {
                            text.Font = UIFont.BoldSystemFontOfSize(14);
                        }
                        else
                        {
                            text.Font = UIFont.SystemFontOfSize(14);
                        }
                        text.TranslatesAutoresizingMaskIntoConstraints = false;

                        text.LeftAnchor.ConstraintEqualTo(QuestionsScroll.LeftAnchor, 15).Active = true;
                        text.RightAnchor.ConstraintEqualTo(QuestionsScroll.RightAnchor, -15).Active = true;
                        text.WidthAnchor.ConstraintEqualTo(QuestionsScroll.WidthAnchor, 1, -30).Active = true;

                        if (count == 1)
                        {
                            text.TopAnchor.ConstraintEqualTo(QuestionsScroll.TopAnchor, 15).Active = true;
                        }
                        else
                        {
                            text.TopAnchor.ConstraintEqualTo(QuestionsScroll.Subviews[count - 2].BottomAnchor, 10).Active = true;
                        }
                        if (count == lines.Length)
                        {
                            NSLayoutConstraint constraint = text.BottomAnchor.ConstraintEqualTo(QuestionsScroll.BottomAnchor, -15);
                            constraint.Priority = 200; //in case there are fewer questions than what would fill the page, priority needs to be lower than the texts' hugging priority. 
                            constraint.Active = true;
                        }
                    }
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

        public override void ViewWillAppear(bool animated) //ViewDidLoad is called before ListActivity ViewWillDisappear
        {
            base.ViewWillAppear(animated);

            firstRun = false;
        }

        private void HelpCenterBack_TouchDown(object sender, EventArgs e)
        {
            c.AnimateRipple(RippleHelpCenter, 2);
        }

        private void HelpCenterBack_TouchUpInside(object sender, EventArgs e)
        {
            CommonMethods.OpenPage(null, 0);
        }

        private void HelpCenterFormCaption_TouchUpInside(object sender, EventArgs e)
        {
            if (MessageContainer.Frame.Height == 0)
            {
                c.ExpandY(MessageContainer);
                c.CollapseY(QuestionsScroll);
                MessageEdit.BecomeFirstResponder();
            }
            else
            {
                c.CollapseY(MessageContainer);
                c.ExpandY(QuestionsScroll);
                View.EndEditing(true);
            }
        }

        private async void MessageSend_TouchUpInside(object sender, EventArgs e)
        {
            View.EndEditing(true);
            if (MessageEdit.Text != "")
            {
                MessageSend.Enabled = false;
                MessageSend.Alpha = 0.5f;

                string url = "action=helpcentermessage&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&Content=" + c.UrlEncode(MessageEdit.Text);
                string responseString = await c.MakeRequest(url);
                if (responseString == "OK")
                {
                    MessageEdit.Text = "";
                    c.CollapseY(MessageContainer);
                    c.ExpandY(QuestionsScroll);
                    c.Snack(LangEnglish.HelpCenterSent);
                }
                else
                {
                    c.ReportError(responseString);
                }                
                MessageSend.Enabled = true;
                MessageSend.Alpha = 1f;
            }
        }

        private async void OpenTutorial_TouchUpInside(object sender, EventArgs e)
        {
            c.CollapseY(MessageContainer);
            c.ExpandY(QuestionsScroll);
            View.EndEditing(true);

            foreach (UIView view in TutorialFrame.Subviews)
            {
                if (view is UIImageView)
                {
                    view.RemoveFromSuperview();
                }
            }
            TutorialText.Text = "";
            TutorialNavText.Text = "";

            TutorialTopBar.Hidden = false;
            TutorialFrameBg.Hidden = false;
            TutorialFrame.Hidden = false;
            TutorialTopSeparator.Hidden = false;
            TutorialBottomSeparator.Hidden = false;
            TutorialNavBar.Hidden = false;
            RoundBottomTutorial.Hidden = false;
            StartAnim();

            cancelImageLoading = false;
                                 

            string url = "action=tutorial&OS=iOS&dpWidth=" + dpWidth;
			string responseString = await c.MakeRequest(url);
			if (responseString.Substring(0, 2) == "OK")
			{
				tutorialDescriptions = new List<string>();
				tutorialPictures = new List<string>();
				responseString = responseString.Substring(3);
				
				string[] lines = responseString.Split("\t");
				int count = 0;
				foreach (string line in lines)
				{
					count++;
					if (count % 2 == 1)
					{
						tutorialDescriptions.Add(line);
					}
					else
					{
						tutorialPictures.Add(line);
					}
				}
				
				currentTutorial = 0;
				LoadTutorial(true); 
				LoadEmptyPictures(tutorialDescriptions.Count);

				await Task.Run(() =>
				{
					for (int i = 0; i < tutorialDescriptions.Count; i++)
					{
						if (cancelImageLoading)
						{
							break;
						}
						LoadPicture(i);
					}
				});
			}
			else
			{
				c.ReportError(responseString);
			}
        }

        private void TutorialBack_TouchDown(object sender, EventArgs e)
        {
            c.AnimateRipple(RippleTutorial1, 2);
        }

        private void TutorialBack_TouchUpInside(object sender, EventArgs e)
        {
            TutorialTopBar.Hidden = true;
            TutorialFrameBg.Hidden = true;
            TutorialFrame.Hidden = true;
            TutorialTopSeparator.Hidden = true;
            TutorialBottomSeparator.Hidden = true;
            TutorialNavBar.Hidden = true;
            RoundBottomTutorial.Hidden = true;
            LoaderCircle.Hidden = true;
            cancelImageLoading = true;
        }

        private void StartAnim()
		{
			LoaderCircle.Hidden = false;

            CABasicAnimation rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
            rotationAnimation.To = NSNumber.FromDouble(Math.PI * 2);
            rotationAnimation.RepeatCount = int.MaxValue;
            rotationAnimation.Duration = Constants.loaderAnimTime;
            LoaderCircle.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
		}

		private void StopAnim()
		{
			LoaderCircle.Hidden = true;
            LoaderCircle.Layer.RemoveAllAnimations();
		}

        private void TutorialLoad_TouchDown(object sender, EventArgs e)
        {
            foreach (NSLayoutConstraint constraint in TutorialNavBar.Constraints)
            {
                if (constraint.FirstItem == RippleTutorial2)
                {
                    TutorialNavBar.RemoveConstraint(constraint);
                }
            }
            RippleTutorial2.CenterXAnchor.ConstraintEqualTo(((UIButton)sender).CenterXAnchor).Active = true;
            RippleTutorial2.CenterYAnchor.ConstraintEqualTo(((UIButton)sender).CenterYAnchor).Active = true;
            TutorialNavBar.LayoutIfNeeded();
            c.AnimateRipple(RippleTutorial2, 1.8f);
        }

        private void TutorialLoadPrevious_TouchUpInside(object sender, EventArgs e)
        {
            currentTutorial--;
			if (currentTutorial < 0)
			{
				currentTutorial = tutorialDescriptions.Count - 1;
			}
			LoadTutorial(true);
        }

        private void TutorialLoadNext_TouchUpInside(object sender, EventArgs e)
        {
            currentTutorial++;
			if (currentTutorial > tutorialDescriptions.Count - 1)
			{
				currentTutorial = 0;
			}
			LoadTutorial(true);
        }

        private void LoadTutorial(bool scroll)
		{
			TutorialText.Text = tutorialDescriptions[currentTutorial];
			TutorialNavText.Text = currentTutorial + 1 + " / " + tutorialDescriptions.Count;
            if (scroll)
            {
                TutorialFrame.ContentOffset = new CGPoint(currentTutorial * TutorialFrame.Frame.Width, 0);
            }
		}

        private void LoadEmptyPictures(int count)
		{
            for (int index = 0; index < count; index++)
            {
				UIImageView image = new UIImageView();

				TutorialFrame.AddSubview(image);
				image.TranslatesAutoresizingMaskIntoConstraints = false;
                image.ContentMode = UIViewContentMode.ScaleAspectFit;

				image.WidthAnchor.ConstraintEqualTo(TutorialFrame.WidthAnchor).Active = true;
				image.HeightAnchor.ConstraintEqualTo(TutorialFrame.HeightAnchor).Active = true;
				image.TopAnchor.ConstraintEqualTo(TutorialFrame.TopAnchor).Active = true;
				image.BottomAnchor.ConstraintEqualTo(TutorialFrame.BottomAnchor).Active = true;
				image.RightAnchor.ConstraintEqualTo(TutorialFrame.RightAnchor).Active = true;

				if (index == 0)
				{
					image.LeftAnchor.ConstraintEqualTo(TutorialFrame.LeftAnchor).Active = true;
				}
				else
				{
                    //there are two _UIScrollViewScrollIndicators in the ScrollView
                    UIView prevImage = GetChildAt(index-1);

					foreach (NSLayoutConstraint constraint in TutorialFrame.Constraints)
					{
						if (constraint.FirstItem == prevImage && constraint.FirstAttribute == NSLayoutAttribute.Right)
						{
							TutorialFrame.RemoveConstraint(constraint);
						}
					}
					image.LeftAnchor.ConstraintEqualTo(prevImage.RightAnchor).Active = true;
				}
			}
		}

        private UIImageView GetChildAt(int index)
        {
            int indexCount = -1;
            foreach (UIView view in TutorialFrame.Subviews)
            {
                if (view is UIImageView)
                {
                    indexCount++;
                    if (indexCount == index)
                    {
                        return (UIImageView)view;
                    }
                }
            }
            return null;
        }

		private void LoadPicture(int index)
		{
			try {
                UIImageView image = null;
				InvokeOnMainThread(() => {
					image = GetChildAt(index);
				});

				string url = Constants.HostName + Constants.TutorialFolder + "/" + tutorialPictures[index];

				UIImage im = CommonMethods.LoadFromUrl(url);

                if (index == 0)
                {
                    InvokeOnMainThread(() =>
                    {
                        StopAnim();
                    });
                }

                if (im == null)
				{
					im = UIImage.FromBundle(Constants.noImageHD);
				}

				InvokeOnMainThread(() => {
					image.Image = im;
				});
		    }
			catch (Exception ex)
			{
				c.CW(ex.Message + " " + ex.StackTrace);
			}
		}

        [Export("scrollViewDidScroll:")]
		public void Scrolled(UIScrollView scrollView)
        {
            
            int newImageIndex = (int)Math.Round(scrollView.ContentOffset.X / scrollView.Frame.Width);
            if (newImageIndex != currentTutorial)
            {
                if (newImageIndex >= 0 && newImageIndex < tutorialDescriptions.Count)
                {
                    currentTutorial = newImageIndex;
                    LoadTutorial(false);
                }
            }
        }
    }
}