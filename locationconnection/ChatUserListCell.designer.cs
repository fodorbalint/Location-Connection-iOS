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
    [Register ("ChatUserListCell")]
    partial class ChatUserListCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChatUserList_Image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ChatUserList_Items { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.InsetLabel ChatUserList_Name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ChatUserList_Image != null) {
                ChatUserList_Image.Dispose ();
                ChatUserList_Image = null;
            }

            if (ChatUserList_Items != null) {
                ChatUserList_Items.Dispose ();
                ChatUserList_Items = null;
            }

            if (ChatUserList_Name != null) {
                ChatUserList_Name.Dispose ();
                ChatUserList_Name = null;
            }
        }
    }
}