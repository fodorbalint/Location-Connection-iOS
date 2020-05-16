using System;
using System.Globalization;
using CoreLocation;
using UIKit;

namespace LocationConnection
{
    public class LocationManager
    {
        public CLLocationManager locMgr;

        private BaseActivity context;

        public LocationManager(BaseActivity context)
        {
            this.context = context;
            locMgr = new CLLocationManager();        
        }

        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public void StartLocationUpdates()
        {
            if (context.c.IsLocationEnabled())
            {
                locMgr.PausesLocationUpdatesAutomatically = false;

                if (context.c.IsLoggedIn() && (bool)Session.BackgroundLocation)
                {
                    if (!Constants.SafeLocationMode)
                    {
                        locMgr.AllowsBackgroundLocationUpdates = true; 
                    }
                    else
                    {
                        locMgr.AllowsBackgroundLocationUpdates = false;
                    }
                }
                else
                {
                    locMgr.AllowsBackgroundLocationUpdates = false;
                }                

                locMgr.DesiredAccuracy = 1000; //accuracy is enough, and if set to 10, if would use a lot of battery. (1% per h on ipad). When set to 100 or above, update frequency is 15s. When set to 10 or below, it may be 1s.

                locMgr.LocationsUpdated += LocMgr_LocationsUpdated;

                locMgr.LocationUpdatesPaused += LocMgr_LocationUpdatesPaused;
                locMgr.LocationUpdatesResumed += LocMgr_LocationUpdatesResumed;

                locMgr.StartUpdatingLocation();
                BaseActivity.locationUpdating = true;

                context.c.LogActivity("Location updates started"); //, allowBackground: " + locMgr.AllowsBackgroundLocationUpdates);
                context.c.CW("Location updates started"); //, allowBackground: " + locMgr.AllowsBackgroundLocationUpdates);
            }
        }

        public void StopLocationUpdates()
        {
            locMgr.StopUpdatingLocation();
            BaseActivity.locationUpdating = false;
            BaseActivity.firstLocationAcquired = false;
            Session.SafeLocationTime = null;
            context.c.LogActivity("Location updates stopped");
            context.c.CW("Location updates stopped");
        }

        public void RestartLocationUpdates()
        {
            context.c.LogActivity("Restarting location updates");
            context.c.CW("Restarting location updates");
            StopLocationUpdates();
            StartLocationUpdates();
        }

        private async void LocMgr_LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            int inAppLocationRate;
            long unixTimestamp = context.c.Now();

            CLLocation location = e.Locations[e.Locations.Length - 1];

            if (!Constants.SafeLocationMode)
            {
                if (BaseActivity.isAppForeground)
                {
                    if (Session.InAppLocationRate != null) //if logged in, sometimes this is still null. Might be the delay between setting SessionID and other fields
                    {
                        inAppLocationRate = (int)Session.InAppLocationRate;
                    }
                    else
                    {
                        inAppLocationRate = (int)Settings.InAppLocationRate;
                    }

                    if (Session.LocationTime is null || unixTimestamp - Session.LocationTime >= inAppLocationRate)
                    {
                        Session.Latitude = location.Coordinate.Latitude;
                        Session.Longitude = location.Coordinate.Longitude;
                        Session.LocationTime = unixTimestamp;

                        LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));

                        if (context.c.IsLoggedIn())
                        {
                            Session.LastActiveDate = unixTimestamp;
                            await context.c.UpdateLocationSync(false);
                        }
                        context.c.LogLocation(unixTimestamp + "|" + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + "|" + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture) + "|1");
                    }
                }
                else
                {
                    //even if AllowsBackgroundLocationUpdates is false, a few location updates might occur when entering foreground and background

                    if (context.c.IsLoggedIn() && (bool)Session.BackgroundLocation && (Session.LocationTime is null || unixTimestamp - Session.LocationTime >= Session.BackgroundLocationRate))
                    {
                        Session.Latitude = location.Coordinate.Latitude;
                        Session.Longitude = location.Coordinate.Longitude;
                        Session.LocationTime = unixTimestamp;

                        Session.LastActiveDate = unixTimestamp;
                        await context.c.UpdateLocationSync(false);

                        context.c.LogLocation(unixTimestamp + "|" + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + "|" + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture) + "|0");
                    }
                }
            }
            else //foreground updates only
            {
                Session.LatestLatitude = location.Coordinate.Latitude;
                Session.LatestLongitude = location.Coordinate.Longitude;
                Session.LatestLocationTime = unixTimestamp;

                if (Session.InAppLocationRate != null) //if logged in, sometimes this is still null. Might be the delay between setting SessionID and other fields
                {
                    inAppLocationRate = (int)Session.InAppLocationRate;
                }
                else
                {
                    inAppLocationRate = (int)Settings.InAppLocationRate;
                }

                if (Session.SafeLocationTime is null || unixTimestamp - Session.SafeLocationTime >= inAppLocationRate)
                {
                    Session.SafeLatitude = location.Coordinate.Latitude;
                    Session.SafeLongitude = location.Coordinate.Longitude;
                    Session.SafeLocationTime = unixTimestamp;

                    LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));

                    if (context.c.IsLoggedIn())
                    {
                        await context.c.UpdateLocationSync(true); //to match
                    }

                    context.c.LogLocation(unixTimestamp + "|" + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + "|" + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture) + "|1");
                }
            }

            //context.c.LogLocation(unixTimestamp + "|" + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + "|" + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture) + "|" + (BaseActivity.isAppForeground ? 1 : 0) + updatedStr);

            if (!BaseActivity.firstLocationAcquired)
            {
                if (ListActivity.locationTimer != null && ListActivity.locationTimer.Enabled)
                {
                    ListActivity.locationTimer.Stop();
                }

                BaseActivity.firstLocationAcquired = true;

                context.c.CW("LocationManager_LocationUpdated first location");
                context.c.LogActivity("LocationManager_LocationUpdated first location");

                var currentController = CommonMethods.GetCurrentViewController();
                if (currentController is ListActivity)
                {
                    ((ListActivity)currentController).LoadListStartup();
                }
            }
        }

        //are these ever called?
        private void LocMgr_LocationUpdatesPaused(object sender, EventArgs e)
        {
            context.c.LogActivity("Location updates paused");
        }

        private void LocMgr_LocationUpdatesResumed(object sender, EventArgs e)
        {
            context.c.LogActivity("Location updates resumed");
        }
    }
}
