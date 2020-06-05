using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public class ChatMessageWindowAdapter : UITableViewSource
    {
        private List<MessageItem> items;

        public ChatMessageWindowAdapter(List<MessageItem> items)
        {
            this.items = items;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return items.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            ChatMessageWindowCell cell = (ChatMessageWindowCell)tableView.DequeueReusableCell("ChatMessageWindowCell");

            MessageItem item = items[indexPath.Row];           

            cell.Construct();

            if (indexPath.Row == 0)
            {
                cell.MainViewTopConstraint.Constant = -10;
                cell.MainViewBottomConstraint.Constant = 0;
            }
            else if (indexPath.Row == items.Count -1)
            {
                cell.MainViewTopConstraint.Constant = 0;
                cell.MainViewBottomConstraint.Constant = 10;
            }
            else
            {
                cell.MainViewTopConstraint.Constant = 0;
                cell.MainViewBottomConstraint.Constant = 0;
            }   

            if (item.SenderID == Session.ID)
            {
                cell.AlignRight();
                cell.MessageText.BackgroundColor = UIColor.FromName("ChatOwn");
                cell.TimeText.TextAlignment = UITextAlignment.Right;
            }
            else
            {
                cell.AlignLeft();
                cell.MessageText.BackgroundColor = UIColor.FromName("ChatTarget");
                cell.TimeText.TextAlignment = UITextAlignment.Left;
            }
            cell.MessageText.Text = item.Content;

            SetMessageTime(cell, item.SentTime, item.SeenTime, item.ReadTime);

            return cell;
        }

        public void SetMessageTime(ChatMessageWindowCell view, long sentTime, long seenTime, long readTime)
        {
            UILabel TimeText = view.TimeText;

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            DateTime sentDate = dateTime.AddSeconds(sentTime).ToLocalTime();
            if (sentDate.Date == DateTime.Today)
            {
                TimeText.Text = LangEnglish.MessageStatusSent + " " + sentDate.ToString("HH:mm");
            }
            else
            {
                if (sentDate.Year == DateTime.Now.Year)
                {
                    TimeText.Text = LangEnglish.MessageStatusSent + " " + sentDate.ToString("dd MMM HH:mm");
                }
                else
                {
                    TimeText.Text = LangEnglish.MessageStatusSent + " " + sentDate.ToString("dd MMM yyyy HH:mm");
                }

            }

            if (readTime != 0)
            {
                DateTime readDate = dateTime.AddSeconds(readTime).ToLocalTime();
                if (readTime < sentTime + 60)
                {
                    TimeText.Text += " - " + LangEnglish.MessageStatusRead;
                }
                else if (readDate.Date == sentDate.Date)
                {
                    TimeText.Text += " - " + LangEnglish.MessageStatusRead + " " + readDate.ToString("HH:mm");
                }
                else
                {
                    if (readDate.Year == sentDate.Year)
                    {
                        TimeText.Text += " - " + LangEnglish.MessageStatusRead + " " + readDate.ToString("dd MMM HH:mm");
                    }
                    else
                    {
                        TimeText.Text += " - " + LangEnglish.MessageStatusRead + " " + readDate.ToString("dd MMM yyyy HH:mm");
                    }

                }
            }
            else if (seenTime != 0)
            {
                DateTime seenDate = dateTime.AddSeconds(seenTime).ToLocalTime();
                if (seenTime < sentTime + 60)
                {
                    TimeText.Text += " - " + LangEnglish.MessageStatusSeen;
                }
                else if (seenDate.Date == sentDate.Date)
                {
                    TimeText.Text += " - " + LangEnglish.MessageStatusSeen + " " + seenDate.ToString("HH:mm");
                }
                else
                {
                    if (seenDate.Year == sentDate.Year)
                    {
                        TimeText.Text += " - " + LangEnglish.MessageStatusSeen + " " + seenDate.ToString("dd MMM HH:mm");
                    }
                    else
                    {
                        TimeText.Text += " - " + LangEnglish.MessageStatusSeen + " " + seenDate.ToString("dd MMM yyyy HH:mm");
                    }
                }
            }
        }
    }
}
