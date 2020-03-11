using System;
using System.Timers;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public class CustomTapNoDelay : UIGestureRecognizer
    {
        ChatOneActivity context;
        UIView view;
        bool valid;

        public CustomTapNoDelay(ChatOneActivity context, UIView view)
        {
            this.context = context;
            this.view = view;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            valid = true;
            SetHighlighted();
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            valid = false;
            SetNormal();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            SetNormal();

            if (valid)
            {
                IntentData.profileViewPageType = "standalone";

                IntentData.targetID = (int)Session.CurrentMatch.TargetID;
                CommonMethods.OpenPage("ProfileViewActivity", 1);
            }
        }

        private void SetHighlighted ()
        {
            
            view.BackgroundColor = UIColor.FromRGB(179, 179, 179);
        }

        public void SetNormal ()
        {

            view.BackgroundColor = UIColor.FromRGB(217, 217, 217);
        }
    }
}
