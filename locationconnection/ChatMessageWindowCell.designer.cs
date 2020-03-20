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
    [Register ("ChatMessageWindowCell")]
    partial class ChatMessageWindowCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MainView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainView_BottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainView_TopConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainViewLeftConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainViewRightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.InsetLabel Message_Text { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MessageTextLeftConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MessageTextRightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.InsetLabel Time_Text { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TimeTextLeftConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TimeTextRightConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MainView != null) {
                MainView.Dispose ();
                MainView = null;
            }

            if (MainView_BottomConstraint != null) {
                MainView_BottomConstraint.Dispose ();
                MainView_BottomConstraint = null;
            }

            if (MainView_TopConstraint != null) {
                MainView_TopConstraint.Dispose ();
                MainView_TopConstraint = null;
            }

            if (MainViewLeftConstraint != null) {
                MainViewLeftConstraint.Dispose ();
                MainViewLeftConstraint = null;
            }

            if (MainViewRightConstraint != null) {
                MainViewRightConstraint.Dispose ();
                MainViewRightConstraint = null;
            }

            if (Message_Text != null) {
                Message_Text.Dispose ();
                Message_Text = null;
            }

            if (MessageTextLeftConstraint != null) {
                MessageTextLeftConstraint.Dispose ();
                MessageTextLeftConstraint = null;
            }

            if (MessageTextRightConstraint != null) {
                MessageTextRightConstraint.Dispose ();
                MessageTextRightConstraint = null;
            }

            if (Time_Text != null) {
                Time_Text.Dispose ();
                Time_Text = null;
            }

            if (TimeTextLeftConstraint != null) {
                TimeTextLeftConstraint.Dispose ();
                TimeTextLeftConstraint = null;
            }

            if (TimeTextRightConstraint != null) {
                TimeTextRightConstraint.Dispose ();
                TimeTextRightConstraint = null;
            }
        }
    }
}