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

		public GridLayout(int colCount, nfloat spacing, nfloat width)
		{
			this.colCount = colCount;
			this.spacing = spacing;
			MinimumInteritemSpacing = spacing;
			MinimumLineSpacing = spacing;
			nfloat size = GetSize(width);
			ItemSize = new CGSize(size, size);
		}

		public void UpdateCellSize(nfloat newWidth)
		{
			nfloat size = GetSize(newWidth);
			ItemSize = new CGSize(size, size);
			//InvalidateLayout();
		}

		/*public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
		{
			Console.WriteLine("-----ShouldInvalidateLayoutForBoundsChange ------------");
			nfloat size = GetSize(newBounds.Width);
			ItemSize = new CGSize(size, size);
			return true;
			//return base.ShouldInvalidateLayoutForBoundsChange(newBounds);
		}*/

		private nfloat GetSize(nfloat width)
		{
			return (width - spacing * (colCount - 1)) / colCount;
		}
	}
}