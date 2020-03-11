using Foundation;
using System;
using System.ComponentModel;
using UIKit;

namespace LocationConnection
{
    public partial class Snackbar : UIView
    {
        public ISite Site { get; set; }

        public UILabel SnackText { get { return Snack_Text; } }
        public UIButton SnackButton { get { return Snack_Button; } }

        public Snackbar (IntPtr handle) : base (handle)
        {
        }

        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if ((Site != null) && Site.DesignMode)
            {
                // Bundle resources aren't available in DesignMode
                return;
            }
            
            NSBundle.MainBundle.LoadNib("Snackbar", this, null);

            AddSubview(this.MainView);

            MainView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            MainView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
            MainView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
            MainView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;
        }
    }
}