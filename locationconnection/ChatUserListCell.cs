using Foundation;
using System;
using UIKit;

namespace LocationConnection
{
    public partial class ChatUserListCell : UITableViewCell
    {
        public bool activeCell;
        public UIView ChatUserListItems { get { return ChatUserList_Items; } }
        public UIImageView ChatUserListImage { get { return ChatUserList_Image; } }
        public UILabel ChatUserListName { get { return ChatUserList_Name; } }

        public ChatUserListCell(IntPtr handle) : base(handle)
        {
        }
    }
}