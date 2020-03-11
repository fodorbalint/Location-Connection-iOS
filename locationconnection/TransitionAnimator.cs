using System;
using CoreGraphics;
using UIKit;

namespace LocationConnection
{
    internal class RightToLeftTransitionAnimator : UIViewControllerAnimatedTransitioning
    {
        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return 0.35;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var inView = transitionContext.ContainerView;
            var toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var toView = toVC.View;

            inView.AddSubview(toView);

            var frame = toView.Frame;
            toView.Frame = new CGRect(frame.Width, 0, frame.Width, frame.Height);

            Console.WriteLine("RightToLeftTransitionAnimator start");

            UIView.Animate(TransitionDuration(transitionContext), () => {
                toView.Frame = new CGRect(0, 0, frame.Width, frame.Height);
            }, () => {
                transitionContext.CompleteTransition(true);

                CommonMethods.transitionRunning = false;
                if (CommonMethods.transitionTarget != "empty")
                {
                    CommonMethods.OpenPage(CommonMethods.transitionTarget, CommonMethods.transitionAnim);
                    CommonMethods.transitionTarget = "empty";
                }
            });
        }
    }

    internal class LeftToRightTransitionAnimator : UIViewControllerAnimatedTransitioning
    {
        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return 0.35;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var inView = transitionContext.ContainerView;
            var fromVC = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);            
            var fromView = fromVC.View;
            var toView = toVC.View;

            inView.InsertSubview(toView, 0);

            var frame = toView.Frame;
            toView.Frame = new CGRect(0, 0, frame.Width, frame.Height);

            Console.WriteLine("LeftToRightTransitionAnimator start");

            UIView.Animate(TransitionDuration(transitionContext), () => {
                fromView.Frame = new CGRect(frame.Width, 0, frame.Width, frame.Height);
            }, () => {
                transitionContext.CompleteTransition(true);

                CommonMethods.transitionRunning = false;
                if (CommonMethods.transitionTarget != "empty")
                {
                    CommonMethods.OpenPage(CommonMethods.transitionTarget, CommonMethods.transitionAnim);
                    CommonMethods.transitionTarget = "empty";
                }
            });
        }
    }

    internal class RightToLeftNoOverlapTransitionAnimator : UIViewControllerAnimatedTransitioning
    {
        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return 0.35;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var inView = transitionContext.ContainerView;
            var fromVC = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var fromView = fromVC.View;
            var toView = toVC.View;

            inView.AddSubview(toView);

            var frame = toView.Frame;
            toView.Frame = new CGRect(frame.Width, 0, frame.Width, frame.Height);

            UIView.Animate(TransitionDuration(transitionContext), () => {
                fromView.Frame = new CGRect(-frame.Width, 0, frame.Width, frame.Height);
                toView.Frame = new CGRect(0, 0, frame.Width, frame.Height);
            }, () => {
                transitionContext.CompleteTransition(true);

                CommonMethods.transitionRunning = false;
                if (CommonMethods.transitionTarget != "empty")
                {
                    CommonMethods.OpenPage(CommonMethods.transitionTarget, CommonMethods.transitionAnim);
                    CommonMethods.transitionTarget = "empty";
                }
            });
        }
    }

    internal class LeftToRightNoOverlapTransitionAnimator : UIViewControllerAnimatedTransitioning
    {
        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return 0.35;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var inView = transitionContext.ContainerView;
            var fromVC = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toVC = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var fromView = fromVC.View;
            var toView = toVC.View;

            inView.AddSubview(toView);

            var frame = toView.Frame;
            toView.Frame = new CGRect(-frame.Width, 0, frame.Width, frame.Height);

            UIView.Animate(TransitionDuration(transitionContext), () => {
                fromView.Frame = new CGRect(frame.Width, 0, frame.Width, frame.Height);
                toView.Frame = new CGRect(0, 0, frame.Width, frame.Height);
            }, () => {
                transitionContext.CompleteTransition(true);

                CommonMethods.transitionRunning = false;
                if (CommonMethods.transitionTarget != "empty")
                {
                    CommonMethods.OpenPage(CommonMethods.transitionTarget, CommonMethods.transitionAnim);
                    CommonMethods.transitionTarget = "empty";
                }
            });
        }
    }
}