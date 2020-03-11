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
    [Register ("ChatListActivity")]
    partial class ChatListActivity
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChatListBottomSeparator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChatListTopSeparator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ChatUserList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MenuList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MenuListBg { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MenuListBgCorner { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoMatch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NoofMatches { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RippleChatList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RoundBottom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.Snackbar Snackbar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SnackBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BottomConstraint != null) {
                BottomConstraint.Dispose ();
                BottomConstraint = null;
            }

            if (ChatListBottomSeparator != null) {
                ChatListBottomSeparator.Dispose ();
                ChatListBottomSeparator = null;
            }

            if (ChatListTopSeparator != null) {
                ChatListTopSeparator.Dispose ();
                ChatListTopSeparator = null;
            }

            if (ChatUserList != null) {
                ChatUserList.Dispose ();
                ChatUserList = null;
            }

            if (MenuList != null) {
                MenuList.Dispose ();
                MenuList = null;
            }

            if (MenuListBg != null) {
                MenuListBg.Dispose ();
                MenuListBg = null;
            }

            if (MenuListBgCorner != null) {
                MenuListBgCorner.Dispose ();
                MenuListBgCorner = null;
            }

            if (NoMatch != null) {
                NoMatch.Dispose ();
                NoMatch = null;
            }

            if (NoofMatches != null) {
                NoofMatches.Dispose ();
                NoofMatches = null;
            }

            if (RippleChatList != null) {
                RippleChatList.Dispose ();
                RippleChatList = null;
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
        }
    }
}