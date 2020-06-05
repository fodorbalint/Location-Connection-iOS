using System;
using System.Timers;
using Foundation;
using UIKit;

//see CustomTap for differences between devices

namespace LocationConnection
{
    public class CustomTapNoDelay : UIGestureRecognizer
    {
        ChatOneActivity context;
        UIView view;
        bool valid;
        private nfloat startX, startY;

        public CustomTapNoDelay(ChatOneActivity context, UIView view)
        {
            this.context = context;
            this.view = view;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            var location = LocationInView(view);
            startX = location.X;
            startY = location.Y;

            valid = true;
            SetHighlighted();
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            var location = LocationInView(view);

            if (location.X != startX || location.Y != startY)
            {
                valid = false;
                SetNormal();
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            SetNormal();

            if (valid && (int)context.currentMatch.TargetID != 0) //not a deleted user
            {
                IntentData.profileViewPageType = Constants.ProfileViewType_Standalone;
                IntentData.targetID = (int)context.currentMatch.TargetID;
                CommonMethods.OpenPage("ProfileViewActivity", 1);
            }
        }

        private void SetHighlighted ()
        {
            view.BackgroundColor = UIColor.FromName("ChatBackgroundDark");
        }

        public void SetNormal ()
        {

            view.BackgroundColor = UIColor.FromName("ContentDarkBackground");
        }
    }
}
