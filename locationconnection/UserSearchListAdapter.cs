using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LocationConnection
{
	public class UserSearchListAdapter : UICollectionViewController
	{
		private ListActivity context;
		public List<Profile> items;
		public int colCount;
		public nfloat spacing;
		nfloat itemWidth; //since the cell does not scale content, it has to be set manually

		public UserSearchListAdapter(ListActivity context, int colCount, nfloat spacing)
		{
			this.context = context;
			this.colCount = colCount;
			this.spacing = spacing;

			nfloat actualWidth = BaseActivity.dpWidth;
			if (BaseActivity.dpWidth / BaseActivity.dpHeight > 1) // in landscape
			{
				actualWidth -= BaseActivity.safeAreaLeft + BaseActivity.safeAreaRight;
			}
			itemWidth = GetSize(actualWidth);
		}

		public void UpdateItemSize()
		{
			nfloat actualWidth = BaseActivity.dpWidth;
            if (BaseActivity.dpWidth / BaseActivity.dpHeight > 1) // rotated to landscape
            {
				actualWidth -= BaseActivity.safeAreaTop + BaseActivity.safeAreaBottom; //results in 243.333 originally // w 812 h 375 44 34 0 0
			}
			itemWidth = GetSize(actualWidth);
			Console.WriteLine("UpdateItemSize " + itemWidth + " " + actualWidth + " " + BaseActivity.safeAreaTop + " " + BaseActivity.safeAreaBottom);
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return items.Count;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
            
			var cell = (UICollectionViewCell)collectionView.DequeueReusableCell("UserSearchListCell", indexPath);

			try //error once on simulator when entering MainActivity, indexPath.row is out of bounds
			{
				UIImageView imageView = (UIImageView)cell.Subviews[0].Subviews[0];
				imageView.Frame = new CGRect(new CGPoint(0, 0), new CGSize(itemWidth, itemWidth));

				Profile item = items[indexPath.Row];

				ImageCache im = new ImageCache(this);
				im.LoadImage(imageView, item.ID.ToString(), item.Pictures[0]);
			}
			catch (Exception ex)
			{
				context.c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + " index: " + indexPath.Row);
			}

			return cell;
		}

        private nfloat GetSize(nfloat width)
		{
			return (width - spacing * (colCount - 1)) / colCount;
		}
	}
}