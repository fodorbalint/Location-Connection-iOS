using Foundation;
using System;
using UIKit;

namespace LocationConnection
{
    public partial class LocationHistoryListCell : UITableViewCell
    {
        public UILabel LocationHistoryLabel { get { return LocationHistory_Label; } }

        public LocationHistoryListCell (IntPtr handle) : base (handle)
        {
        }
    }
}