using Foundation;
using System;
using UIKit;

namespace LocationConnection
{
    public partial class ChatMessageWindowCell : UITableViewCell
    {
        public UILabel MessageText { get { return Message_Text; } }
        public UILabel TimeText { get { return Time_Text; } }
        public NSLayoutConstraint MainViewTopConstraint { get { return MainView_TopConstraint; } }
        public NSLayoutConstraint MainViewBottomConstraint { get { return MainView_BottomConstraint; } }

        public ChatMessageWindowCell (IntPtr handle) : base (handle)
        {
        }

        public void Construct()
        {
            //MessageText.TextContainerInset = new UIEdgeInsets(10, 5, 10, 10);
            MessageText.Layer.MasksToBounds = true;
            MessageText.Layer.CornerRadius = 5;
        }

        public void AlignLeft()
        {
            MainViewLeftConstraint.Active = true;
            MainViewRightConstraint.Active = false;

            MessageTextLeftConstraint.Active = true;
            MessageTextRightConstraint.Active = false;
            TimeTextLeftConstraint.Active = true;
            TimeTextRightConstraint.Active = false;

            
            /*MainViewLeftConstraint.Priority = 1000;

            MessageTextLeftConstraint.Priority = 1000;
            MessageTextRightConstraint.Priority = 200;
            TimeTextLeftConstraint.Priority = 1000;
            TimeTextRightConstraint.Priority = 200;*/
        }

        public void AlignRight()
        {
            MainViewLeftConstraint.Active = false;
            MainViewRightConstraint.Active = true;

            MessageTextLeftConstraint.Active = false;
            MessageTextRightConstraint.Active = true;
            TimeTextLeftConstraint.Active = false;
            TimeTextRightConstraint.Active = true;

            //iOS 12 throws error: Objective - C exception thrown. Name: NSInternalInconsistencyException Reason: Mutating a priority from required to not on an installed constraint(or vice - versa) is not supported. You passed priority 1000 and the existing priority was 998.

            /*MainViewLeftConstraint.Priority = 998;

            MessageTextLeftConstraint.Priority = 200;
            MessageTextRightConstraint.Priority = 1000;
            TimeTextLeftConstraint.Priority = 200;
            TimeTextRightConstraint.Priority = 1000;*/
        }
    }
}