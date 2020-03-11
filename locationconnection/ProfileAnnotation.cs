using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using MapKit;
using UIKit;

namespace LocationConnection
{
	public class ProfileAnnotation : MKAnnotation
	{
		string title;
		public int UserID;
		public string image;
		CLLocationCoordinate2D coord;

		public ProfileAnnotation(int UserID, string image, CLLocationCoordinate2D coord)
		{
			title = UserID.ToString();
			this.UserID = UserID;
			this.image = image;
			this.coord = coord;
		}

        [Export("setCoordinate:")]
        public override void SetCoordinate(CLLocationCoordinate2D value)
        {
			WillChangeValue("coordinate");
			coord = value;
			DidChangeValue("coordinate");
		}

		public override string Title => title;
		public override CLLocationCoordinate2D Coordinate
		{
			get { return coord; }
		}

	}
}