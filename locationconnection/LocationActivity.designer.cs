// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace LocationConnection
{
    [Register ("LocationActivity")]
    partial class LocationActivity
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LocationBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView LocationHistoryList { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView LocationHistoryMap { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MapSatellite { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MapStreet { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RippleLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RoundBottom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.Snackbar Snackbar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SnackBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SnackTopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BottomConstraint != null) {
                BottomConstraint.Dispose ();
                BottomConstraint = null;
            }

            if (LocationBack != null) {
                LocationBack.Dispose ();
                LocationBack = null;
            }

            if (LocationHistoryList != null) {
                LocationHistoryList.Dispose ();
                LocationHistoryList = null;
            }

            if (LocationHistoryMap != null) {
                LocationHistoryMap.Dispose ();
                LocationHistoryMap = null;
            }

            if (MapSatellite != null) {
                MapSatellite.Dispose ();
                MapSatellite = null;
            }

            if (MapStreet != null) {
                MapStreet.Dispose ();
                MapStreet = null;
            }

            if (RippleLocation != null) {
                RippleLocation.Dispose ();
                RippleLocation = null;
            }

            if (RoundBottom != null) {
                RoundBottom.Dispose ();
                RoundBottom = null;
            }

            if (Snackbar != null) {
                Snackbar.Dispose ();
                Snackbar = null;
            }

            if (SnackBottomConstraint != null) {
                SnackBottomConstraint.Dispose ();
                SnackBottomConstraint = null;
            }

            if (SnackTopConstraint != null) {
                SnackTopConstraint.Dispose ();
                SnackTopConstraint = null;
            }
        }
    }
}