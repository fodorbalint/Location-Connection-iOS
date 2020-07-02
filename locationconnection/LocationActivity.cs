using CoreAnimation;
using CoreLocation;
using Foundation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace LocationConnection
{
    public partial class LocationActivity : BaseActivity, IUITableViewDelegate
    {
        private static bool foregroundNotificationSet;
        List<LocationItem> locationList;
        LocationListAdapter adapter;
        MKPointAnnotation circle;
        List<MKPolyline> lines;

        int selectedPos;

        public LocationActivity (IntPtr handle) : base (handle)
        {
        }               

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                if (!foregroundNotificationSet)
                {
                    UIApplication.Notifications.ObserveDidEnterBackground((sender, args) => {
                        c.Log("Entered background LocationActivity");
                    });

                    UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => { //once set, will be called when other activities go to background and foreground
                        c.Log("Entered foreground LocationActivity");

                        BaseActivity currentController = CommonMethods.GetCurrentViewController();
                        if (currentController is LocationActivity)
                        {
                            ((LocationActivity)currentController).LoadList();
                            ((LocationActivity)currentController).AddLinesToMap();
                        }                        
                    });

                    foregroundNotificationSet = true;
                }

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackTopConstraint_Base = SnackTopConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;

                LocationHistoryList.SeparatorStyle = UITableViewCellSeparatorStyle.None;

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                MapStreet.SetTitle(LangEnglish.MapStreet, UIControlState.Normal);
                MapSatellite.SetTitle(LangEnglish.MapSatellite, UIControlState.Normal);
                LocationHistoryMap.MapType = (MKMapType)Settings.LocationMapType;
                if (Settings.LocationMapType == (int)MKMapType.Standard)
                {
                    MapStreet.BackgroundColor = UIColor.FromName("MapButtonActive");
                    MapSatellite.BackgroundColor = UIColor.FromName("MapButtonPassive");
                }
                else
                {
                    MapStreet.BackgroundColor = UIColor.FromName("MapButtonPassive");
                    MapSatellite.BackgroundColor = UIColor.FromName("MapButtonActive");
                }
                LocationHistoryMap.Delegate = new CustomAnnotationView(this);
                LocationHistoryMap.LayoutMargins = new UIEdgeInsets(43, 0, 0, 7); //to move the compass

                LocationBack.TouchUpInside += LocationBack_Click;
                LocationBack.TouchDown += LocationBack_TouchDown;
                MapStreet.TouchUpInside += MapStreet_Click;
                MapSatellite.TouchUpInside += MapSatellite_Click;

                LoadList();                
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (!(locMgr is null))
                {
                    locMgr.LocationUpdated += LocMgr_LocationUpdated;
                }

                AddLinesToMap();
                //List count: 3569 time to draw: 3018. All lines appear at the end, drawing piece by piece is not working even if LayoutIfNeeded is called. Realistically, with 1 h daily use and a 15 s refresh interval, it would need 200 ms.
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            c.SetRoundShadow(MapStreet, -1, 1, 1, 2, true);
            c.SetRoundShadow(MapSatellite, -1, 1, 1, 2, false);

            MapStreet.Layer.CornerRadius = 2;
            MapStreet.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MinXMaxYCorner;
            MapSatellite.Layer.CornerRadius = 2;
            MapSatellite.Layer.MaskedCorners = CACornerMask.MaxXMinYCorner | CACornerMask.MaxXMaxYCorner;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (!(locMgr is null))
            {
                locMgr.LocationUpdated -= LocMgr_LocationUpdated;
            }

            if (LocationHistoryMap.MapType != (MKMapType)Settings.LocationMapType)
            {
                Settings.LocationMapType = (int)LocationHistoryMap.MapType;
                c.SaveSettings();
            }
        }

        private void LoadList()
        {
            locationList = new List<LocationItem>();
            if (lines != null)
            {
                LocationHistoryMap.RemoveOverlays(lines.ToArray());
            }
            lines = new List<MKPolyline>();

            //File.Delete(c.locationLogFile);

            if (File.Exists(c.locationLogFile))
            {
                string[] fileLines = File.ReadAllLines(c.locationLogFile);

                for (int i = fileLines.Length - 1; i >= 0; i--)
                {
                    string line = fileLines[i];
                    LocationItem item = new LocationItem();

                    int sep1Pos = line.IndexOf('|');
                    int sep2Pos = line.IndexOf('|', sep1Pos + 1);
                    int sep3Pos = line.IndexOf('|', sep2Pos + 1);

                    if (sep2Pos != -1)
                    {
                        item.time = long.Parse(line.Substring(0, sep1Pos));
                        item.latitude = double.Parse(line.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1), CultureInfo.InvariantCulture);
                        item.longitude = double.Parse(line.Substring(sep2Pos + 1, sep3Pos - sep2Pos - 1), CultureInfo.InvariantCulture);
                        string status = line.Substring(sep3Pos + 1);
                        switch (status)
                        {
                            case "0":
                                item.inApp = false;
                                item.sent = false;
                                break;
                            case "1":
                                item.inApp = true;
                                item.sent = false;
                                break;
                            case "2":
                                item.inApp = true;
                                item.sent = true;
                                break;
                        }
                        item.isSelected = false;
                        locationList.Add(item);
                    }
                }
                locationList[0].isSelected = true;
                selectedPos = 0;                

                SetMap();
            }
            else
            {
                c.Snack(LangEnglish.NoLocationRecords);
            }

            adapter = new LocationListAdapter(locationList);
            LocationHistoryList.Source = adapter;
            LocationHistoryList.ReloadData();
            LocationHistoryList.Delegate = this;
        }

        public void SetMap()
        {
            double latitude = locationList[0].latitude;
            double longitude = locationList[0].longitude;

            CLLocationCoordinate2D location = new CLLocationCoordinate2D((double)latitude, (double)longitude);

            MoveMap(location, true);
            AddCircle(location);
        }

        public void AddLinesToMap()
        {
            for(int i = locationList.Count -1; i > 0; i--)
            {
                long unixTimestamp = c.Now();
                long time = locationList[i - 1].time;
                float ratio = ((float)unixTimestamp - time) / Constants.LocationKeepTime;

                AddLine(new CLLocationCoordinate2D(locationList[i].latitude, locationList[i].longitude), new CLLocationCoordinate2D(locationList[i - 1].latitude, locationList[i - 1].longitude), (int)(ratio * 255), (int)((1 - ratio) * 255), 0);
            }
        }

        private void MoveMap(CLLocationCoordinate2D location, bool isFirst)
        {
            if (isFirst)
            {
                MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(location, 2000, 2000);
                LocationHistoryMap.CenterCoordinate = location;
                LocationHistoryMap.Region = mapRegion;
            }
            else
            {
                LocationHistoryMap.CenterCoordinate = location;
            }
        }

        private void AddCircle(CLLocationCoordinate2D location)
        {
            if (!(circle is null))
            {
                LocationHistoryMap.RemoveAnnotation(circle);
            }
            circle = new MKPointAnnotation() { Title = "Center", Coordinate = location };
            LocationHistoryMap.AddAnnotation(circle);
        }

        private void AddLine(CLLocationCoordinate2D location1, CLLocationCoordinate2D location2, int red, int green, int blue)
        {
            MKPolyline line1 = MKPolyline.FromCoordinates(new CLLocationCoordinate2D[] { location1, location2 });
            MKPolyline line2 = MKPolyline.FromCoordinates(new CLLocationCoordinate2D[] { location1, location2 }); //subclassing MKPolyline to add a color property would not work. https://forums.xamarin.com/discussion/38410/extend-mkpolyline-to-draw-on-map-with-different-colors
            line2.Title = red + "|" + green + "|" + blue;

            LocationHistoryMap.AddOverlay(line1);
            LocationHistoryMap.AddOverlay(line2);

            lines.Add(line1);
            lines.Add(line2);
        }

        private void LocationBack_TouchDown(object sender, EventArgs e)
        {
            c.AnimateRipple(RippleLocation, 2);
        }

        private void LocationBack_Click(object sender, EventArgs e)
        {
            CommonMethods.OpenPage(null, 0);
        }

        private void MapStreet_Click(object sender, EventArgs e)
        {
            LocationHistoryMap.MapType = MKMapType.Standard;
            MapStreet.BackgroundColor = UIColor.FromName("MapButtonActive");
            MapSatellite.BackgroundColor = UIColor.FromName("MapButtonPassive");
        }

        private void MapSatellite_Click(object sender, EventArgs e)
        {
            LocationHistoryMap.MapType = MKMapType.Satellite;
            MapStreet.BackgroundColor = UIColor.FromName("MapButtonPassive");
            MapSatellite.BackgroundColor = UIColor.FromName("MapButtonActive");
        }

        [Export("tableView:didSelectRowAtIndexPath:")] //DismissKeyboard() would block it
        public virtual void RowSelected(UIKit.UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            if (indexPath.Row != selectedPos)
            {
                locationList[selectedPos].isSelected = false;
                locationList[indexPath.Row].isSelected = true;
                selectedPos = indexPath.Row;

                CLLocationCoordinate2D location = new CLLocationCoordinate2D(locationList[indexPath.Row].latitude, locationList[indexPath.Row].longitude);
                MoveMap(location, false);
                AddCircle(location);
            }
            else
            {
                if (locationList[selectedPos].isSelected == false)
                {
                    locationList[selectedPos].isSelected = true;

                    CLLocationCoordinate2D location = new CLLocationCoordinate2D(locationList[indexPath.Row].latitude, locationList[indexPath.Row].longitude);
                    MoveMap(location, false);
                    AddCircle(location);
                }
                else
                {
                    locationList[selectedPos].isSelected = false;
                    if (!(circle is null))
                    {
                        LocationHistoryMap.RemoveAnnotation(circle);
                    }
                }
            }
            LocationHistoryList.ReloadData();
        }

        private void LocMgr_LocationUpdated(object sender, LocationUpdatedEventArgs e)
        {
            AddItem();
        }

        public void AddItem()
        {
            LocationItem item = new LocationItem();
            CLLocationCoordinate2D location;

            if (!Constants.SafeLocationMode)
            {
                item.time = (long)Session.LocationTime;
                item.latitude = (double)Session.Latitude;
                item.longitude = (double)Session.Longitude;

                location = new CLLocationCoordinate2D((double)Session.Latitude, (double)Session.Longitude);
            }
            else
            {
                item.time = (long)Session.SafeLocationTime;
                item.latitude = (double)Session.SafeLatitude;
                item.longitude = (double)Session.SafeLongitude;

                location = new CLLocationCoordinate2D((double)Session.SafeLatitude, (double)Session.SafeLongitude);
            }
            
            item.inApp = true;

            if (selectedPos == 0 && locationList.Count > 0)
            {
                item.isSelected = true;
                locationList[selectedPos].isSelected = false;
                AddCircle(location);
                MoveMap(location, false);
            }
            else if (locationList.Count == 0) {
                item.isSelected = true;
                AddCircle(location);
                MoveMap(location, true);
            }
            else
            {
                item.isSelected = false;
                selectedPos++;
            }

            locationList.Insert(0, item);
            LocationHistoryList.ReloadData();

            if (locationList.Count >= 2)
            {
                AddLine(new CLLocationCoordinate2D(locationList[1].latitude, locationList[1].longitude), location, 0, 255, 0);
            }
            
        }
    }
}