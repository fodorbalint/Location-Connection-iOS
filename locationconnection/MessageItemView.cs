using Foundation;
using System;
using UIKit;

namespace LocationConnection
{
    public partial class MessageItemView : UIView
    {
        public UITextView MessageText { get { return Message_Text; } }
        public UILabel TimeText { get { return Time_Text; } }

        public MessageItemView () : base ()
        {
            NSBundle.MainBundle.LoadNib("MessageItemView", this, null);

            //Frame = Bounds;
            //MainView.Frame = Bounds;

            AddSubview(MainView);
            MainView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            MainView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
            MainView.LeadingAnchor.ConstraintEqualTo(LeadingAnchor).Active = true;
            MainView.TrailingAnchor.ConstraintEqualTo(TrailingAnchor).Active = true;
            MessageText.TextContainerInset = new UIEdgeInsets(10, 5, 10, 10);
            MessageText.Layer.CornerRadius = 5;
        }

        public void AlignLeft()
        {
            MessageText.LeftAnchor.ConstraintEqualTo(MainView.LeftAnchor).Active = true;
            MessageText.RightAnchor.ConstraintLessThanOrEqualTo(MainView.RightAnchor).Active = true;
            TimeText.LeftAnchor.ConstraintEqualTo(MainView.LeftAnchor).Active = true;
            TimeText.RightAnchor.ConstraintLessThanOrEqualTo(MainView.RightAnchor).Active = true;
        }

        public void AlignRight()
        {
            MessageText.LeftAnchor.ConstraintGreaterThanOrEqualTo(MainView.LeftAnchor).Active = true;
            MessageText.RightAnchor.ConstraintEqualTo(MainView.RightAnchor).Active = true;
            TimeText.LeftAnchor.ConstraintGreaterThanOrEqualTo(MainView.LeftAnchor).Active = true;
            TimeText.RightAnchor.ConstraintEqualTo(MainView.RightAnchor).Active = true;
        }
    }
}