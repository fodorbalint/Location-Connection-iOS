// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LocationConnection
{
    [Register ("MessageItemView")]
    partial class MessageItemView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MainView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Message_Text { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.InsetLabel Time_Text { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MainView != null) {
                MainView.Dispose ();
                MainView = null;
            }

            if (Message_Text != null) {
                Message_Text.Dispose ();
                Message_Text = null;
            }

            if (Time_Text != null) {
                Time_Text.Dispose ();
                Time_Text = null;
            }
        }
    }
}