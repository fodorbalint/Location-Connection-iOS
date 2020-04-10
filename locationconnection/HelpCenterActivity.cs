using Foundation;
using System;
using UIKit;

namespace LocationConnection
{
    public partial class HelpCenterActivity : BaseActivity
    {
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

                MessageSend.Layer.MasksToBounds = true;

                c.DrawBorder(MessageEdit);
                c.CollapseY(MessageContainer);

                HelpCenterBack.TouchDown += HelpCenterBack_TouchDown;
                HelpCenterBack.TouchUpInside += HelpCenterBack_TouchUpInside;
                HelpCenterFormCaption.TouchUpInside += HelpCenterFormCaption_TouchUpInside;
                MessageSend.TouchUpInside += MessageSend_TouchUpInside;

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
                            constraint.Priority = 999;
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
    }
}