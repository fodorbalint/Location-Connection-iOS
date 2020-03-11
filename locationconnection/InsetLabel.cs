using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
	[Register("InsetLabel"), DesignTimeVisible(true)]
	public class InsetLabel : UILabel
	{
		[Export("TopInset"), Browsable(true)]
		public float TopInset { get; set; }

		[Export("LeftInset"), Browsable(true)]
		public float LeftInset { get; set; }

		[Export("RightInset"), Browsable(true)]
		public float RightInset { get; set; }

		[Export("BottomInset"), Browsable(true)]
		public float BottomInset { get; set; }

		public InsetLabel(IntPtr p) : base(p)
		{
		}

        public override void DrawText(CGRect rect)
        {
			var insets = new UIEdgeInsets(TopInset, LeftInset, BottomInset, RightInset);
			
            base.DrawText(insets.InsetRect(rect));
        }

		public override CGSize IntrinsicContentSize { get {
				CGSize size = base.IntrinsicContentSize;
				size.Height += TopInset + BottomInset;
				size.Width += LeftInset + RightInset;
				return size;
			} }
    }
}