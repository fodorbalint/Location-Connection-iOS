using Foundation;
using System;
using UIKit;

namespace LocationConnection
{
    public partial class CustomAlert : UIView
    {
        public UITextView AlertText { get { return Alert_Text; } }
        public UIButton AlertButton { get { return Alert_Button; } }

        public CustomAlert (IntPtr handle) : base (handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            NSBundle.MainBundle.LoadNib("CustomAlert", this, null);

            AddSubview(this.MainView);

            MainView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            MainView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
            MainView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
            MainView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;

            Alert_Button.TouchUpInside += Alert_Button_TouchUpInside;
        }

        private void Alert_Button_TouchUpInside(object sender, EventArgs e)
        {
            this.Hidden = true;
        }
    }
}