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
                    cell.ContentView.BackgroundColor = UIColor.FromRGB(74, 207, 140);
                }
                else
                {
                    cell.ContentView.BackgroundColor = UIColor.FromRGB(229, 170, 52);
                }
            }
            else
            {
                if (item.inApp)
                {
                    cell.ContentView.BackgroundColor = UIColor.FromRGB(195, 239, 217);
                }
                else
                {
                    cell.ContentView.BackgroundColor = UIColor.FromRGB(246, 226, 187);
                }
            }

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(item.time).ToLocalTime();
            cell.LocationHistoryLabel.Text = dt.ToString("HH:mm:ss");

            return cell;
        }
    }
}
