﻿using System;
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
                    locMgr.AllowsBackgroundLocationUpdates = true;
                }
                else
                {
                    locMgr.AllowsBackgroundLocationUpdates = false;
                }                

                locMgr.DesiredAccuracy = 1000; //accuracy is enough, and if set to 10, if would use a lot of battery. (1% per h on ipad)

                locMgr.LocationsUpdated += LocMgr_LocationsUpdated;

                locMgr.LocationUpdatesPaused += LocMgr_LocationUpdatesPaused;
                locMgr.LocationUpdatesResumed += LocMgr_LocationUpdatesResumed;

                locMgr.StartUpdatingLocation();
                BaseActivity.locationUpdating = true;

                context.c.LogActivity("Location updates started, allowBackground: " + locMgr.AllowsBackgroundLocationUpdates);
                context.c.CW("Location updates started, allowBackground: " + locMgr.AllowsBackgroundLocationUpdates);
            }
        }

        public void StopLocationUpdates()
        {
            locMgr.StopUpdatingLocation();
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
            long unixTimestamp = context.c.Now();

            CLLocation location = e.Locations[e.Locations.Length - 1];

            if (BaseActivity.isAppForeground)
            {
                int inAppLocationRate;

                if (context.c.IsLoggedIn())
                {
                    try
                    {
                        inAppLocationRate = (int)Session.InAppLocationRate; //nullable object must have a value error sometimes
                    }
                    catch (Exception ex)
                    {
                        context.c.ReportErrorSilent("Error at LocMgr_LocationsUpdated: " + ex.Message + context.c.ShowClass<Session>() + Environment.NewLine + " Session.InAppLocationRate " + Session.InAppLocationRate);
                        inAppLocationRate = (int)Settings.InAppLocationRate;
                    }
                }
                else
                {
                    inAppLocationRate = (int)Settings.InAppLocationRate;
                }

                if (Session.LocationTime is null || unixTimestamp - Session.LocationTime > inAppLocationRate)
                {
                    Session.Latitude = location.Coordinate.Latitude;
                    Session.Longitude = location.Coordinate.Longitude;
                    Session.LocationTime = unixTimestamp;

                    LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));

                    if (context.c.IsLoggedIn())
                    {
                        Session.LastActiveDate = unixTimestamp;
                        await context.c.UpdateLocationSync();
                    }
                    context.c.LogLocation(unixTimestamp + "|" + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + "|" + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture) + "|" + (BaseActivity.isAppForeground ? 1 : 0));
                }
            }
            else
            {
                //even if AllowsBackgroundLocationUpdates is false, a few location updates might occur when entering foreground and background
                //context.c.LogLocation(unixTimestamp + "|Background location update, " + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + " " + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture));

                if (context.c.IsLoggedIn() && (bool)Session.BackgroundLocation && (Session.LocationTime is null || unixTimestamp - Session.LocationTime > Session.BackgroundLocationRate))
                {
                    Session.Latitude = location.Coordinate.Latitude;
                    Session.Longitude = location.Coordinate.Longitude;
                    Session.LocationTime = unixTimestamp;

                    Session.LastActiveDate = unixTimestamp;
                    await context.c.UpdateLocationSync();

                    //updatedStr = " x ";
                    context.c.LogLocation(unixTimestamp + "|" + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + "|" + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture) + "|" + (BaseActivity.isAppForeground ? 1 : 0));
                }
            }

            //context.c.LogLocation(unixTimestamp + "|" + location.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture) + "|" + location.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture) + "|" + (BaseActivity.isAppForeground ? 1 : 0) + updatedStr);

            if (!BaseActivity.firstLocationAcquired)
            {
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
