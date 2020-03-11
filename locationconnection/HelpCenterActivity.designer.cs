// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LocationConnection
{
    [Register ("HelpCenterActivity")]
    partial class HelpCenterActivity
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HelpCenterBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HelpCenterFormCaption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HelpCenterHeaderText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MessageContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView MessageEdit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MessageSend { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView QuestionsScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RippleHelpCenter { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RoundBottom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.Snackbar Snackbar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SnackBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SnackTopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BottomConstraint != null) {
                BottomConstraint.Dispose ();
                BottomConstraint = null;
            }

            if (HelpCenterBack != null) {
                HelpCenterBack.Dispose ();
                HelpCenterBack = null;
            }

            if (HelpCenterFormCaption != null) {
                HelpCenterFormCaption.Dispose ();
                HelpCenterFormCaption = null;
            }

            if (HelpCenterHeaderText != null) {
                HelpCenterHeaderText.Dispose ();
                HelpCenterHeaderText = null;
            }

            if (MessageContainer != null) {
                MessageContainer.Dispose ();
                MessageContainer = null;
            }

            if (MessageEdit != null) {
                MessageEdit.Dispose ();
                MessageEdit = null;
            }

            if (MessageSend != null) {
                MessageSend.Dispose ();
                MessageSend = null;
            }

            if (QuestionsScroll != null) {
                QuestionsScroll.Dispose ();
                QuestionsScroll = null;
            }

            if (RippleHelpCenter != null) {
                RippleHelpCenter.Dispose ();
                RippleHelpCenter = null;
            }

            if (RoundBottom != null) {
                RoundBottom.Dispose ();
                RoundBottom = null;
            }

            if (Snackbar != null) {
                Snackbar.Dispose ();
                Snackbar = null;
            }

            if (SnackBottomConstraint != null) {
                SnackBottomConstraint.Dispose ();
                SnackBottomConstraint = null;
            }

            if (SnackTopConstraint != null) {
                SnackTopConstraint.Dispose ();
                SnackTopConstraint = null;
            }
        }
    }
}