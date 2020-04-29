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
		public int colCount;
		public nfloat spacing;

		public GridLayout(int colCount, nfloat spacing)
		{
			this.colCount = colCount;
			this.spacing = spacing;
			MinimumInteritemSpacing = spacing;
			MinimumLineSpacing = spacing;

			nfloat actualWidth = BaseActivity.dpWidth;
			if (BaseActivity.dpWidth / BaseActivity.dpHeight > 1) // in landscape
			{
				actualWidth -= BaseActivity.safeAreaLeft + BaseActivity.safeAreaRight;
			}
			nfloat size = GetSize(actualWidth);
			ItemSize = new CGSize(size, size);
		}

		public void UpdateCellSize()
		{
			nfloat actualWidth = BaseActivity.dpWidth;
			if (BaseActivity.dpWidth / BaseActivity.dpHeight > 1) // rotated to landscape
			{
				actualWidth -= BaseActivity.safeAreaTop + BaseActivity.safeAreaBottom;
			}
			nfloat size = GetSize(actualWidth);
			Console.WriteLine("UpdateCellSize " + size + " " + actualWidth + " " + BaseActivity.safeAreaTop + " " + BaseActivity.safeAreaBottom );
			ItemSize = new CGSize(size, size);

			/*
-------------------------------------------------------------- ViewWillTransitionToSize w 812 h 375 44 34 0 0
2020-04-29 08:59:51.531 locationconnection[18852:218573] 
2020-04-29 08:59:51.532 locationconnection[18852:218573] UpdateItemSize 243,333333333333 734 44 34
2020-04-29 08:59:51.532 locationconnection[18852:218573] UpdateCellSize 243,333333333333 734 44 34 */
		}

		public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
		{
			Console.WriteLine("-----ShouldInvalidateLayoutForBoundsChange ------------");
			//nfloat size = GetSize(newBounds.Width);
			//ItemSize = new CGSize(size, size);
			//return true;
			return base.ShouldInvalidateLayoutForBoundsChange(newBounds);
		}

		private nfloat GetSize(nfloat width)
		{
			return (width - spacing * (colCount - 1)) / colCount;
		}
	}
}