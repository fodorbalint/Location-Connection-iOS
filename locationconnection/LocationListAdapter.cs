using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public class LocationListAdapter : UITableViewSource
    {
        private List<LocationItem> items;
        int rowHeight = 27;

        public LocationListAdapter(List<LocationItem> items)
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
            LocationHistoryListCell cell = (LocationHistoryListCell)tableView.DequeueReusableCell("LocationHistoryListCell");
            LocationItem item = items[indexPath.Row];

            if (item.isSelected)
            {
                if (item.inApp)
                {
                    if (!item.sent)
                    {
                        cell.ContentView.BackgroundColor = UIColor.FromName("LocationForegroundSelected"); //hsl(150, 58%, 55%)
                    }
                    else
                    {
                        cell.ContentView.BackgroundColor = UIColor.FromName("LocationSentSelected"); //hsl(15, 77%, 65%)
                    }
                }
                else
                {
                    cell.ContentView.BackgroundColor = UIColor.FromName("LocationBackgroundSelected"); //hsl(40, 77%, 55%)
                }
            }
            else
            {
                if (item.inApp)
                {
                    if (!item.sent)
                    {
                        cell.ContentView.BackgroundColor = UIColor.FromName("LocationForeground"); //85%
                    }
                    else
                    {
                        cell.ContentView.BackgroundColor = UIColor.FromName("LocationSent");
                    }
                }
                else
                {
                    cell.ContentView.BackgroundColor = UIColor.FromName("LocationBackground");
                }
            }

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(item.time).ToLocalTime();
            cell.LocationHistoryLabel.Text = dt.ToString("HH:mm:ss");

            return cell;
        }
    }
}
