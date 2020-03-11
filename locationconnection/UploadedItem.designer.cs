// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using UIKit;

namespace LocationConnection
{
    [Register ("UploadedItem")]
    partial class UploadedItem
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteUploadedImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MainView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UploadedImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DeleteUploadedImage != null) {
                DeleteUploadedImage.Dispose ();
                DeleteUploadedImage = null;
            }

            if (MainView != null) {
                MainView.Dispose ();
                MainView = null;
            }

            if (UploadedImage != null) {
                UploadedImage.Dispose ();
                UploadedImage = null;
            }
        }
    }
}