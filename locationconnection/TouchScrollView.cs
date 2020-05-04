    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LocationConnection
{
	[Register("TouchScrollView"), DesignTimeVisible(true)]
	public class TouchScrollView : UIScrollView
	{
		private BaseActivity context;

		public TouchScrollView(IntPtr p) : base(p)
		{
		}

		public void SetContext(BaseActivity context)
		{
			this.context = context;
		}

        public override bool TouchesShouldCancelInContentView(UIView view)
        {
            if (context is RegisterActivity)
            {
				if (view == ((RegisterActivity)context).ImagesUploaded || view.Superview is UploadedItem)
				{
					return false;
				}
			}
            else if (context is ProfileEditActivity)
			{
				if (view == ((ProfileEditActivity)context).ImagesUploaded || view.Superview is UploadedItem)
				{
					return false;
				}
			}

			return base.TouchesShouldCancelInContentView(view);
        }
        
    }
}