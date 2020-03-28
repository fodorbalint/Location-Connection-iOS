using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace LocationConnection
{
    public partial class UploadedItem : UIView
    {
        public ISite Site { get; set; }

        public UploadedItem() : base()
        {
        }

        public void SetImage(BaseActivity context, string userID, string picture, bool temp = false)
        {
            NSBundle.MainBundle.LoadNib("UploadedItem", this, null);

            Frame = Bounds;
            MainView.Frame = Bounds;

            AddSubview(MainView);

            ImageCache im = new ImageCache(context);
            im.LoadImage(UploadedImage, userID, picture, false, temp);
        }
    }
}

