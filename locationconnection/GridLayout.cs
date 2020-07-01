using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
	public class GridLayout : UICollectionViewFlowLayout
	{
		private ListActivity context;
		public int colCount;
		public nfloat spacing;
		private nfloat actualWidth;

		public GridLayout(ListActivity context, int colCount, nfloat spacing)
		{
			this.context = context;
			this.colCount = colCount;
			this.spacing = spacing;
			MinimumInteritemSpacing = spacing;
			MinimumLineSpacing = spacing;

			actualWidth = BaseActivity.dpWidth;
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone && BaseActivity.dpWidth / BaseActivity.dpHeight > 1) // in landscape
			{
				actualWidth -= BaseActivity.safeAreaLeft + BaseActivity.safeAreaRight;
			}
			nfloat size = GetSize(actualWidth);
			ItemSize = new CGSize(size, size);
		}

		public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
		{
			context.c.CW("ShouldInvalidateLayoutForBoundsChange");
			ResizeTiles();

			return base.ShouldInvalidateLayoutForBoundsChange(newBounds);
		}

		public void ResizeTiles()
        {
			BaseActivity.dpWidth = UIScreen.MainScreen.Bounds.Width;
			BaseActivity.dpHeight = UIScreen.MainScreen.Bounds.Height;

			UIWindow window = UIApplication.SharedApplication.KeyWindow;
			BaseActivity.safeAreaLeft = window.SafeAreaInsets.Left; // values are new, after rotation.
			BaseActivity.safeAreaRight = window.SafeAreaInsets.Right;

			//Console.WriteLine("ShouldInvalidateLayoutForBoundsChange "  + newBounds + " " + BaseActivity.safeAreaLeft + " " + BaseActivity.safeAreaRight + " " + BaseActivity.dpWidth + " " + BaseActivity.dpHeight);

			nfloat newActualWidth = BaseActivity.dpWidth - BaseActivity.safeAreaLeft - BaseActivity.safeAreaRight;
			if (newActualWidth != actualWidth)
            {
				actualWidth = newActualWidth;

				nfloat size = GetSize(actualWidth);
				ItemSize = new CGSize(size, size);
				context.adapter.itemWidth = size;

				foreach (UIView view in context.User_SearchList.Subviews)
				{
					if (view is UICollectionViewCell)
					{
						UIImageView image = (UIImageView)view.Subviews[0].Subviews[0];
						image.Frame = new CGRect(new CGPoint(0, 0), new CGSize(size, size));
					}
				}
			}
		}

		private nfloat GetSize(nfloat width)
		{
			return (width - spacing * (colCount - 1)) / colCount;
		}
	}
}