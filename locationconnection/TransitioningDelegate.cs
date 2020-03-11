using UIKit;

namespace LocationConnection
{
    internal class TransitioningDelegate : UIViewControllerTransitioningDelegate
    {
        private byte anim;

        public TransitioningDelegate(byte anim)
        {
            this.anim = anim;
        }

        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            switch (anim)
            {
                default:
                case 1:
                    return new RightToLeftTransitionAnimator();
                case 2:
                    return new RightToLeftNoOverlapTransitionAnimator();
                case 3:
                    return new LeftToRightNoOverlapTransitionAnimator();
            }            
        }

        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        {
            return new LeftToRightTransitionAnimator();
        }
    }
}