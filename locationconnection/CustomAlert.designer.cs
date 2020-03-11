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
    [Register ("CustomAlert")]
    partial class CustomAlert
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Alert_Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Alert_Text { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MainView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Alert_Button != null) {
                Alert_Button.Dispose ();
                Alert_Button = null;
            }

            if (Alert_Text != null) {
                Alert_Text.Dispose ();
                Alert_Text = null;
            }

            if (MainView != null) {
                MainView.Dispose ();
                MainView = null;
            }
        }
    }
}