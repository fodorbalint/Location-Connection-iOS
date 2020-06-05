using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public class ChatUserListAdapter : UITableViewSource
    {
        private List<MatchItem> items;
        int rowHeight = 101;

        public ChatUserListAdapter(List<MatchItem> items)
        {
            this.items = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return items.Count;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return rowHeight;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            ChatUserListCell cell = (ChatUserListCell)tableView.DequeueReusableCell("ChatUserListCell");
            MatchItem item = items[indexPath.Row];

            cell.SeparatorInset = new UIEdgeInsets(0, 0, 0, 0);

            if ((bool)item.Active)
            {
                cell.ContentView.BackgroundColor = UIColor.FromName("ChatTarget");
                cell.activeCell = true;
            }
            else
            {
                cell.ContentView.BackgroundColor = UIColor.FromName("ChatPassive");
                cell.activeCell = false;
            }
            
            UIImageView imageView = cell.ChatUserListImage;

            ImageCache im = new ImageCache(this);
            im.LoadImage(imageView, item.TargetID.ToString(), item.TargetPicture);

            cell.ChatUserListName.Text = item.TargetName;

            foreach (UILabel label in cell.ChatUserListItems.Subviews)
            {
                label.Text = "";
            }

            int j = 0;
            for (int i = item.Chat.Length - 1; i >= 0; i--)
            {
                string messageItem = item.Chat[i];
                int sep1Pos = messageItem.IndexOf('|');
                int sep2Pos = messageItem.IndexOf('|', sep1Pos + 1);
                int sep3Pos = messageItem.IndexOf('|', sep2Pos + 1);
                int sep4Pos = messageItem.IndexOf('|', sep3Pos + 1);
                int sep5Pos = messageItem.IndexOf('|', sep4Pos + 1);
                int senderID = int.Parse(messageItem.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1));
                long readTime = long.Parse(messageItem.Substring(sep4Pos + 1, sep5Pos - sep4Pos - 1));
                string message = messageItem.Substring(sep5Pos + 1);

                UILabel label = (UILabel)cell.ChatUserListItems.Subviews[j];
                label.Text = message.Replace(Environment.NewLine, " ");
                j++;
                label.TextColor = UIColor.FromName("PrimaryDark");
                if (senderID != Session.ID)
                {
                    label.Font = UIFont.BoldSystemFontOfSize(14);
                    if (readTime == 0)
                    {
                        label.BackgroundColor = UIColor.FromName("ChatHighlight");
                    }
                    else
                    {
                        label.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0); //label may remain blue after list reload
                    }
                }
                else
                {
                    label.Font = UIFont.SystemFontOfSize(14);
                    label.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
                }
            }

            return cell;
        }
    }
}
