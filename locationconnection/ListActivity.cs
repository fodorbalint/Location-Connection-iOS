﻿/*
Device screens:

iphone 11: {X=0,Y=0,Width=414,Height=896} --- {X=0,Y=44,Width=414,Height=818} --- 34, to microphone 21
iphone 11 pro: {X=0,Y=0,Width=375,Height=812} --- {X=0,Y=44,Width=375,Height=734} --- 34, to microphone 21
iphone 11 pro max: {X=0,Y=0,Width=414,Height=896} --- {X=0,Y=44,Width=414,Height=818} --- 34, to microphone 21
iphone 8 plus: {X=0,Y=0,Width=414,Height=736} --- {X=0,Y=20,Width=414,Height=716} --- 0
iphone 8: {X=0,Y=0,Width=375,Height=667} --- {X=0,Y=20,Width=375,Height=647} --- 0
ipad pro 12.9 3th gen: {X=0,Y=0,Width=1024,Height=1366} --- {X=0,Y=24,Width=1024,Height=1322} --- 20, to microphone 7 px
ipad pro 11 2018: {X=0,Y=0,Width=834,Height=1194} --- {X=0,Y=24,Width=834,Height=1150} --- 20, to microphone 7 px
ipad pro 9.7 2016: {X=0,Y=0,Width=768,Height=1024} --- {X=0,Y=20,Width=768,Height=1004} --- 0
ipad air 3th gen = {X=0,Y=0,Width=834,Height=1112} --- {X=0,Y=24,Width=834,Height=1088} --- 0
ipad 7 th gen 10.2: {X=0,Y=0,Width=810,Height=1080} --- {X=0,Y=20,Width=810,Height=1060} --- 0
ipad mini 2019: {X=0,Y=0,Width=768,Height=1024} --- {X=0,Y=20,Width=768,Height=1004} --- 0


**********

Code to insert when creating a new page with Snackbar
Replace 1 with page number

<view contentMode="scaleToFill" id="RoundBottom1" translatesAutoresizingMaskIntoConstraints="NO"> ----- Put it before views that should go on top -----
    <color key="backgroundColor" systemColor="systemBackgroundColor" cocoaTouchSystemColor="whiteColor"/> ----- Use desired color -----
</vi    ew>
<view contentMode="scaleToFill" id="Snackbar1" customClass="Snackbar" translatesAutoresizingMaskIntoConstraints="NO" />

secondItem="RoundBottom1" secondAttribute="top" ----- Link to the bottom element(s) -----

<constraint firstAttribute="bottom" secondItem="RoundBottom1" secondAttribute="top" id="RoundBottom12"/>
<constraint firstAttribute="bottom" secondItem="RoundBottom1" secondAttribute="bottom" id="RoundBottom13"/>
<constraint firstItem="RoundBottom1" firstAttribute="leading" secondItem="SafeArea1" secondAttribute="leading" id="RoundBottom14"/>
<constraint firstItem="RoundBottom1" firstAttribute="trailing" secondItem="SafeArea1" secondAttribute="trailing" id="RoundBottom15"/>
<constraint firstAttribute="bottom" secondItem="Snackbar1" secondAttribute="top" priority="200" id="Snackbar12"/>
<constraint firstAttribute="bottom" secondItem="Snackbar1" secondAttribute="bottom" priority="199" id="Snackbar13"/>
<constraint firstItem="Snackbar1" firstAttribute="leading" secondItem="SafeArea1" secondAttribute="leading" id="Snackbar14"/>
<constraint firstItem="Snackbar1" firstAttribute="trailing" secondItem="SafeArea1" secondAttribute="trailing" id="Snackbar15"/>

<outlet property="RoundBottom" destination="RoundBottom1" id="name-outlet-RoundBottom1"/>
<outlet property="Snackbar" destination="Snackbar1" id="name-outlet-Snackbar1"/>
<outlet property="BottomConstraint" destination="RoundBottom12" id="name-outlet-RoundBottom12"/>
<outlet property="SnackTopConstraint" destination="Snackbar12" id="name-outlet-Snackbar12"/>
<outlet property="SnackBottomConstraint" destination="Snackbar13" id="name-outlet-Snackbar13"/>
<outlet property="ScrollBottomConstraint" destination="Register4" id="name-outlet-Register4"/> ----- For pages with ScrollView ----

RoundBottom_Base = RoundBottom;
Snackbar_Base = Snackbar;
BottomConstraint_Base = BottomConstraint;
SnackTopConstraint_Base = SnackTopConstraint;
SnackBottomConstraint_Base = SnackBottomConstraint;
ScrollBottomConstraint_Base = ScrollBottomConstraint; ----- For pages with ScrollView -----

ResizeWindowOnKeyboard(); ----- For keyboard window resizing, goes to ViewWillAppear -----  
*/

using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using UIKit;

namespace LocationConnection
{
    public partial class ListActivity : BaseActivity, IUITextFieldDelegate, IUISearchBarDelegate, IUIAlertViewDelegate
    {
        private static bool backgroundNotificationsSet;        
        public static ListActivity thisInstance;
        private bool autoLogin;

        private bool distanceFiltersOpen;

        private bool listLoading;
        private bool mapSetting;
        private bool mapToSet;
        private bool mapSet;
        private bool mapLoaded;
        public static nint? totalResultCount;
        public UserSearchListAdapter adapter;
        public GridLayout gridLayout;
        private bool usersLoaded; //determines if the map can be set
        private bool recenterMap;
        private MKPointAnnotation circlePin;
        private MKCircle circle;
        private bool distanceSourceCurrentClicked;

        public static List<Profile> listProfiles;
        public static List<Profile> viewProfiles;
        private static List<Profile> newListProfiles;
        public static List<ProfileAnnotation> profileAnnotations;

        public static bool addResultsBefore;
        public static bool addResultsAfter;
        public static int absoluteStartIndex; //Session.ResultsFrom when we enter ProfileView (absolute index of first item in list)
        public static int absoluteIndex; //index in the total number of results.
        public static int viewIndex; //index in list, will change as the view list expands backwards
        public static int absoluteFirstIndex; //absolute position of first element in the list. Changes as list is expanded backwards.

        //if we get location before logging in is completed, we do not need to request it again. 
        private double? localLatitude;
        private double? localLongitude;
        private long? localLocationTime;

        //pull down refresh icon
        /*private nfloat? startY;
        private nfloat loaderHeight = 50;
        private nfloat maxY = 186;
        private nfloat diff;*/
        //private bool animPulldown;

        private UIRefreshControl refresh;

        private bool distanceLimitChangedByCode;
        private bool distanceSourceAddressTextChanging;
        private Timer ProgressTimer;

        public UICollectionView User_SearchList { get { return UserSearchList; } }

        private Timer firstRunTimer;
        public static Timer locationTimer;

        public ListActivity(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                if (!backgroundNotificationsSet)
                {
                    UIApplication.Notifications.ObserveDidEnterBackground((sender, args) => {

                        c.Log("Entered background");

                        isAppForeground = false;

                        if (!string.IsNullOrEmpty(locationUpdatesTo))
                        {
                            EndLocationShare();
                        }
                        locationUpdatesTo = null; //stop real-time location updates when app goes to background
                        locationUpdatesFrom = null;
                        locationUpdatesFromData = null;

                        if ((!c.IsLoggedIn() || !(bool)Session.UseLocation || !(bool)Session.BackgroundLocation || !c.IsLocationEnabled()) && !(locMgr is null)) //because of Session.BackgroundLocation, it will always stop.
                        {
                            locMgr.StopLocationUpdates();
                        }
                    });

                    UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => {
                        c.Log("Entered foreground");

                        isAppForeground = true;

                        if (Session.UseLocation == true && c.IsLocationEnabled() && !(locMgr is null)) //user might have logged out, and Session is nulled.
                        {
                            locMgr.StartLocationUpdates();
                        }
                    });

                    backgroundNotificationsSet = true;
                }
                
                thisInstance = this;
                c.LoadSettings(false); //overwrites DisplaySize

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                if (File.Exists(c.errorFile))
                {
                    string url = "action=reporterror&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                    string content = "Content=" + CommonMethods.UrlEncode(File.ReadAllText(c.errorFile) + System.Environment.NewLine
                        + "IOS version: " + "-----" + " " + "-----" + " " + System.Environment.NewLine + "-----" + System.Environment.NewLine + File.ReadAllText(CommonMethods.logFile));
                    string responseString = c.MakeRequestSync(url, "POST", content);
                    if (responseString == "OK")
                    {
                        File.Delete(c.errorFile);
                    }
                }

                //IsPlayServicesAvailable();
                //CreateNotificationChannel();

                if (!c.IsLoggedIn() && File.Exists(c.loginSessionFile))
                {
                    autoLogin = true;
                }
                else
                {
                    autoLogin = false;
                }

                if (autoLogin)
                {
                    c.Log("Autologin start");
                    Task.Run(async () =>
                    {
                        Session.LastDataRefresh = null;
                        Session.LocationTime = null;

                        string str = File.ReadAllText(c.loginSessionFile);
                        string[] strarr = str.Split(";");

                        string url = "action=loginsession&ID=" + strarr[0] + "&SessionID=" + strarr[1];

                        InvokeOnMainThread(() => { //called after last (third) ViewDidLayoutSubviews
                            ResultSet.Hidden = false;
                            if (ResultSet.Text == "") //only set it if "Getting location" is not being displayed already.
                            {
                                ResultSet.Text = LangEnglish.LoggingIn;
                            }
                        });

                        string responseString = c.MakeRequestSync(url);
                        if (responseString.Substring(0, 2) == "OK")
                        {
                            c.LoadCurrentUser(responseString);

                            InvokeOnMainThread(() => {

                                SetLoggedInMenu();
                                LoggedInLayout();
                                SetViews();

                                c.Log("Autologin logged in views set");
                            });

                            c.Log("Autologin uselocation " + Session.UseLocation + " enabled " + c.IsLocationEnabled());


                            if ((bool)Session.UseLocation)
                            {
                                if (c.IsLocationEnabled())
                                {
                                    if (!Constants.SafeLocationMode)
                                    {
                                        if ((bool)Session.BackgroundLocation)
                                        {
                                            locMgr.RestartLocationUpdates(); //allowBackground will change
                                        }
                                    }

                                    if (!(localLatitude is null) && !(localLongitude is null) && !(localLocationTime is null)) //this has to be more recent than the loaded data
                                    {
                                        if (!Constants.SafeLocationMode)
                                        {
                                            c.Log("Autologin updating location");

                                            if (Session.LocationTime is null || c.Now() - Session.LocationTime > Session.InAppLocationRate) //a location update could have happened just after login. 12 ms between views set and this point.
                                            {
                                                Session.Latitude = localLatitude;
                                                Session.Longitude = localLongitude;
                                                Session.LocationTime = localLocationTime;

                                                await c.UpdateLocationSync(false);
                                            }
                                        }

                                        recenterMap = true;
                                        if (Session.LastSearchType == Constants.SearchType_Filter)
                                        {
                                            LoadList();
                                        }
                                        else
                                        {
                                            LoadListSearch();
                                        }
                                    }
                                    else
                                    {
                                        //first location update will load the list, otherwise the Getting location... message will remain in the status bar until timeout
                                        c.Log("Autologin location unavailable");
                                    }
                                }
                                else
                                {
                                    c.Log("Autologin LocationDisabledButUsingLocation");

                                    if (appeared)
                                    {
                                        InvokeOnMainThread(() =>
                                        {
                                            c.SnackIndef(LangEnglish.LocationDisabledButUsingLocation);
                                        });
                                    }
                                    else
                                    {
                                        Session.SnackMessage = LangEnglish.LocationDisabledButUsingLocation;
                                        Session.SnackPermanent = true;
                                    }

                                    recenterMap = true;
                                    if (Session.LastSearchType == Constants.SearchType_Filter)
                                    {
                                        LoadList();
                                    }
                                    else
                                    {
                                        LoadListSearch();
                                    }
                                }
                            }
                            else
                            {
                                c.Log("Autologin logged in, not using location");

                                if (!(locMgr is null))
                                {
                                    locMgr.StopLocationUpdates();
                                }

                                recenterMap = true;
                                if (Session.LastSearchType == Constants.SearchType_Filter)
                                {
                                    LoadList();
                                }
                                else
                                {
                                    LoadListSearch();
                                }
                            }

                            c.RequestNotification();
                        }
                        else if (responseString.Substring(0, 6) == "ERROR_")
                        {
                            InvokeOnMainThread(() =>
                            {
                                try
                                {
                                    LoggedOutLayout();
                                    if (!(RefreshDistance is null) && !(LoaderCircle is null))
                                    {
                                        StopLoaderAnim();
                                    }
                                    if (!(ResultSet is null))
                                    {
                                        SetResultStatus();
                                    }
                                    string error = responseString.Substring(6);

                                    c.SnackIndef(c.GetLang(responseString.Substring(6)));

                                    if (error == "LoginFailed") // this is the only error we can get
                                    {
                                        File.Delete(c.loginSessionFile);
                                    }

                                    recenterMap = true;
                                    if (Session.LastSearchType == Constants.SearchType_Filter)
                                    {
                                        LoadList();
                                    }
                                    else
                                    {
                                        LoadListSearch();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.ReportError(ex.Message + Environment.NewLine + ex.StackTrace);
                                }
                            });
                        }
                        else
                        {
                            InvokeOnMainThread(() =>
                            {
                                try
                                {
                                    LoggedOutLayout();
                                    if (!(RefreshDistance is null) && !(LoaderCircle is null))
                                    {
                                        StopLoaderAnim();
                                    }
                                    if (!(ResultSet is null))
                                    {
                                        SetResultStatus();
                                    }
                                    c.ReportError(responseString);
                                }
                                catch (Exception ex)
                                {
                                    c.ReportError(ex.Message + Environment.NewLine + ex.StackTrace);
                                }
                            });
                        }
                        autoLogin = false;
                    });
                }

                //ListTypeCaption.Text = LangEnglish.ListType;
                SortByCaption.Text = LangEnglish.SortBy;
                UseGeoNoLabel.SetTitle(LangEnglish.UseGeoNo, UIControlState.Normal);
                UseGeoYesLabel.SetTitle(LangEnglish.UseGeoYes, UIControlState.Normal);
                DistanceSourceCurrentLabel.SetTitle(LangEnglish.DistanceSourceCurrent, UIControlState.Normal);
                DistanceSourceAddressLabel.SetTitle(LangEnglish.DistanceSourceAddress, UIControlState.Normal);
                DistanceUnitText.Text = LangEnglish.DistanceUnit;
                NoResult.Text = LangEnglish.NoResult;
                DistanceLimit.MinValue = 1;
                DistanceLimit.MaxValue = Constants.DistanceLimitMax;
                ResultSet.Text = "";
                LoaderCircle.Hidden = true;
                LoadPrevious.Hidden = true;
                LoadNext.Hidden = true;
                //ReloadPulldown.Alpha = 0;

                MenuLogOut.SetTitle(LangEnglish.MenuLogOut, UIControlState.Normal);
                MenuLogIn.SetTitle(LangEnglish.MenuLogIn, UIControlState.Normal);
                MenuRegister.SetTitle(LangEnglish.MenuRegister, UIControlState.Normal);
                MenuSettings.SetTitle(LangEnglish.MenuSettings, UIControlState.Normal);
                MenuLocation.SetTitle(LangEnglish.MenuLocation, UIControlState.Normal);
                MenuHelpCenter.SetTitle(LangEnglish.MenuHelpCenter, UIControlState.Normal);
                MenuAbout.SetTitle(LangEnglish.MenuAbout, UIControlState.Normal);

                c.HideMenu(MenuLayer, MenuContainer, false);

                MapStreet.SetTitle(LangEnglish.MapStreet, UIControlState.Normal);
                MapSatellite.SetTitle(LangEnglish.MapSatellite, UIControlState.Normal);
                MapStreet.Hidden = true;
                MapSatellite.Hidden = true;
                ListViewMap.MapType = (MKMapType)Settings.ListMapType;
                RippleMain.Alpha = 0;

                usersLoaded = false;
                mapToSet = false;
                listLoading = false;

                distanceFiltersOpen = true;

                UITextField.Notifications.ObserveTextFieldTextDidChange(ChangesDetected);

                StatusImage.TouchUpInside += StatusImage_Click;
                OpenFilters.TouchUpInside += OpenFilters_Click;
                OpenSearch.TouchUpInside += OpenSearch_Click;
                ListView.TouchUpInside += ListView_Click;
                MapView.TouchUpInside += MapView_Click;

                SearchTerm.Delegate = this;
                SortBy_LastActiveDate.TouchUpInside += SortBy_LastActiveDate_Click;
                SortBy_ResponseRate.TouchUpInside += SortBy_ResponseRate_Click;
                SortBy_RegisterDate.TouchUpInside += SortBy_RegisterDate_Click;
                OrderBy.TouchUpInside += OrderBy_Click;
                DistanceFiltersOpenClose.TouchUpInside += DistanceFiltersOpenClose_Click;
                UseGeoNo.SetContext("UseGeoNo", this);
                UseGeoNoLabel.TouchUpInside += (object sender, EventArgs e) => { UseGeoNo.Checked = true; UseGeo_Click(false); };
                UseGeoYes.SetContext("UseGeoYes", this);
                UseGeoYesLabel.TouchUpInside += (object sender, EventArgs e) => { UseGeoYes.Checked = true; UseGeo_Click(true); };
                RefreshDistance.TouchDown += RefreshDistance_TouchDown;
                RefreshDistance.TouchUpInside += RefreshDistance_Click;
                DistanceSourceCurrent.SetContext("DistanceSourceCurrent", this);
                DistanceSourceCurrentLabel.TouchUpInside += (object sender, EventArgs e) => { DistanceSourceCurrent.Checked = true; DistanceSource_Click(true); };
                DistanceSourceAddress.SetContext("DistanceSourceAddress", this);
                DistanceSourceAddressLabel.TouchUpInside += (object sender, EventArgs e) => { DistanceSourceAddress.Checked = true; DistanceSource_Click(false); };

                DistanceSourceAddressText.Delegate = this; //enter press, text change and losing focus
                AddressOK.TouchUpInside += AddressOK_Click;
                DistanceLimit.ValueChanged += DistanceLimit_ValueChanged;
                DistanceLimitInput.Delegate = this;

                UserSearchList.UserInteractionEnabled = true;
                UserSearchList.AllowsSelection = true;
                var tap = new UITapGestureRecognizer();
                tap.AddTarget(() => UserSearchList_ItemClick(tap));
                UserSearchList.AddGestureRecognizer(tap);
                /*var pan = new UIPanGestureRecognizer();
                pan.AddTarget(() => UserSearchList_Touch(pan));
                UserSearchList.AddGestureRecognizer(pan);*/

                refresh = new UIRefreshControl();
                refresh.ValueChanged += Refresh_ValueChanged;
                UserSearchList.RefreshControl = refresh;

                ListViewMap.Delegate = new CustomAnnotationView(this);
                MapStreet.TouchUpInside += MapStreet_Click;
                MapSatellite.TouchUpInside += MapSatellite_Click;

                LoadPrevious.TouchUpInside += LoadPrevious_Click;
                LoadNext.TouchUpInside += LoadNext_Click;

                MenuChatList.TouchUpInside += MenuChatList_Click;
                MenuChatList.TouchDown += MenuChatList_Touch;

                MenuIcon.TouchUpInside += MenuIcon_Click;
                MenuLayer.TouchDown += MenuLayer_TouchDown;

                MenuLogOut.TouchUpInside += MenuLogOut_Click;
                MenuLogIn.TouchUpInside += MenuLogIn_Click;
                MenuRegister.TouchUpInside += MenuRegister_Click;
                MenuSettings.TouchUpInside += MenuSettings_Click;
                MenuLocation.TouchUpInside += MenuLocation_Click;
                MenuHelpCenter.TouchUpInside += MenuHelpCenter_Click;
                MenuAbout.TouchUpInside += MenuAbout_Click;

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackTopConstraint_Base = SnackTopConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;                
                LoaderCircleLeftConstraint_Base = LoaderCircleLeftConstraint;
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace); //Alert cannot be presented in ViewDidLoad, ViewWillAppear and ViewWillDisappear (but in ViewWillLayoutSubviews). Warning: Attempt to present <UIAlertController: 0x107804600> on <ListActivity: 0x104009180> whose view is not in the window hierarchy!
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                if (File.Exists(c.locationLogFile))
                {
                    TruncateLocationLog();
                }
                TruncateSystemLog();

                mapLoaded = false;

                //Safe practice from Android: c.IsLoggedIn() true does not mean, all session variables are set. Nullable object must have a value error can occur if autologin response is at the same time as ONResume.
                bool isLoggedIn = c.IsLoggedIn();
                c.Log("Logged in: " + isLoggedIn);

                if (isLoggedIn)
                {
                    LoggedInLayout();
                    SetLoggedInMenu();
                }
                else
                {
                    LoggedOutLayout();
                    SetLoggedOutMenu();
                }

                if (!(listProfiles is null) && !(newListProfiles is null) && Session.ListType != "hid") { //hid list will reload
					if (absoluteIndex < absoluteStartIndex)
					{
						c.CW("OnResume got low range: absoluteIndex " + absoluteIndex + " absoluteStartIndex " + absoluteStartIndex + " absoluteFirstIndex " + absoluteFirstIndex + " viewIndex: " + (absoluteIndex - absoluteFirstIndex));
						do
						{
							absoluteStartIndex -= Constants.MaxResultCount;
						} while (absoluteStartIndex > absoluteIndex);

						if (absoluteStartIndex - absoluteFirstIndex + Constants.MaxResultCount > viewProfiles.Count) // this range contains less elements than MaxResultCount (could have happened, it profiles were hid)
						{
							listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, viewProfiles.Count - (absoluteStartIndex - absoluteFirstIndex));
						}
						else //range is full
						{
							listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, Constants.MaxResultCount);
						}
					}
					else if (absoluteIndex >= absoluteStartIndex + listProfiles.Count)
					{
						c.CW("OnResume got high range: absoluteIndex " + absoluteIndex + " absoluteStartIndex " + absoluteStartIndex + " absoluteFirstIndex " + absoluteFirstIndex + " viewIndex: " + (absoluteIndex - absoluteFirstIndex));
						do
						{
							absoluteStartIndex += Constants.MaxResultCount;
						} while (absoluteStartIndex <= absoluteIndex);
						absoluteStartIndex -= Constants.MaxResultCount;

						if (absoluteStartIndex - absoluteFirstIndex + Constants.MaxResultCount > viewProfiles.Count) // this range contains less elements than MaxResultCount 
						{
							listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, viewProfiles.Count - (absoluteStartIndex - absoluteFirstIndex));
						}
						else //range is full
						{
							listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, Constants.MaxResultCount);
						}
					}
					else //we are in the original range, but could have hid profiles, so the section must be recreated.
					{
						c.CW("OnResume in normal range: absoluteIndex " + absoluteIndex + " absoluteStartIndex " + absoluteStartIndex + " absoluteFirstIndex " + absoluteFirstIndex + " viewIndex: " + (absoluteIndex - absoluteFirstIndex));
						if (absoluteStartIndex - absoluteFirstIndex + Constants.MaxResultCount > viewProfiles.Count) // this range contains less elements than MaxResultCount 
						{
							listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, viewProfiles.Count - (absoluteStartIndex - absoluteFirstIndex));
						}
						else //range is full
						{
							listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, Constants.MaxResultCount);
						}
					}

					if ((bool)Settings.IsMapView)
					{
						mapToSet = true;
					}
					else
					{
						mapToSet = false;
					}

					Session.ResultsFrom = absoluteStartIndex + 1;
					c.CW("Session.ResultsFrom " + Session.ResultsFrom + " listProfiles.Count " + listProfiles.Count + " newListProfiles.Count " + newListProfiles.Count);
				}
				newListProfiles = null;

                if (!usersLoaded && (bool)Settings.IsMapView) //set map only when ViewDidLoad was called: when switching from ChatList to List.
                    //we don't want to set the map when returning from Profile View as it will reset the zoom and the position.
                {
                    mapToSet = true;
                }

                GetScreenMetrics();
                gridLayout = new GridLayout(this, 3, 2f);
                UserSearchList.SetCollectionViewLayout(gridLayout, false);
                adapter = new UserSearchListAdapter(this, 3, 2f);
                UserSearchList.DataSource = adapter;

                if (!(listProfiles is null))
                {
                    adapter.items = listProfiles;
                    usersLoaded = true;

                    UserSearchList.ReloadData();
                    UserSearchList.LayoutIfNeeded();
                }
                else
                {
                    adapter.items = new List<Profile>();
                }
                

                c.Log("Location permission status: " + CLLocationManager.Status);

                if (isLoggedIn)
                {
                    if ((bool)Session.UseLocation && !c.IsLocationEnabled())
                    {
                        c.Log("ViewWillAppear LocationDisabledButUsingLocation Session" + Session.UseLocation + " IsLocationEnabled " + c.IsLocationEnabled());
                        Session.SnackMessage = LangEnglish.LocationDisabledButUsingLocation;
                        Session.SnackPermanent = true;
                    }
                }
                else
                {
                    if (!c.IsLocationEnabled())
                    {
                        Session.UseLocation = false;
                    }
                    else
                    {
                        Session.UseLocation = true;
                    }
                }
                if (!(bool)Session.UseLocation || !c.IsLocationEnabled()) //the user might have turned off location while having current location checked
				{
					SetDistanceSourceAddress();
				}

                SetViews();

                if ((bool)Session.UseLocation && c.IsLocationEnabled())
                {
                    isAppForeground = true;

                    if (locMgr is null)
                    {
                        locMgr = new LocationManager(this);
                    }                    

                    if (!locationUpdating)
                    {
                        c.Log("Location not yet updating");
                        locMgr.StartLocationUpdates();
                        ResultSet.Hidden = false;
                        ResultSet.Text = LangEnglish.GettingLocation;
                        StartLocationTimer();
                    }
                    else if (!firstLocationAcquired) //first location will load the list
                    {
                        c.Log("Location updating, awaiting location");
                        ResultSet.Hidden = false;
                        ResultSet.Text = LangEnglish.GettingLocation;
                        StartLocationTimer();
                    }
                    else 
                    {
                        c.Log("Location exists");
                        LoadListStartup();
                    }
                    //when autologin, background location is not yet enabled at this point
                }
                else //no location, loading list
                {
                    if (locationUpdating) //not logged in user didn't enable PM, or logged-in user does not use location
                    {
                        locMgr.StopLocationUpdates();
                    }
                    c.Log("ViewWillAppear no location");
                    LoadListStartup();
                }

                if ((!(bool)Session.UseLocation || !c.IsLocationEnabled()) && !((bool)Session.GeoFilter && (bool)Session.GeoSourceOther)) {
                    if ((bool)Settings.IsMapView)
                    {
                        ListView_Click(null, null);
                    }
                }

                if (firstRun) //if alert is not shown, will be changed next time ListActivity is recreated.
				{
					firstRunTimer = new Timer();
					firstRunTimer.Interval = Constants.TutorialInterval;
					firstRunTimer.Elapsed += FirstRunTimer_Elapsed;
					firstRunTimer.Start();
				}

                //after logging in, ViewDidLayoutSubviews is not called, therefore the bottom is not set.

                c.Log("ViewWillAppear end");
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void StartLocationTimer()
        {
            c.Log("Starting location timer");
            locationTimer = new Timer();
            locationTimer.Interval = Constants.LocationTimeout;
            locationTimer.Elapsed += LocationTimer_Elapsed;
            locationTimer.Start();
        }

        private void LocationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ((Timer)sender).Stop();
            c.Log("Location timeout");
            this.InvokeOnMainThread(() =>
            {
                LoadListStartup();
            });
        }

        public void LoadListStartup () {

            long unixTimestamp = c.Now();

            if (autoLogin)
            {
                if (!Constants.SafeLocationMode)
                {
                    c.Log("LoadListStartup autologin, locationUpdating " + locationUpdating + ", locationtime: " + Session.LocationTime);

                    localLatitude = Session.Latitude;
                    localLongitude = Session.Longitude;
                    localLocationTime = Session.LocationTime;
                }
                else
                {
                    c.Log("LoadListStartup autologin, locationUpdating " + locationUpdating + ", locationtime: " + Session.LatestLocationTime);

                    localLatitude = Session.LatestLatitude;
                    localLongitude = Session.LatestLongitude;
                    localLocationTime = Session.LatestLocationTime;
                }
            }
            else
            {
                if (!Constants.SafeLocationMode)
                {
                    c.Log("LoadListStartup not autologin, logged in: " + c.IsLoggedIn() + ", locationtime: " + Session.LocationTime + " Settings.IsMapView " + Settings.IsMapView + " mapToSet " + mapToSet);
                }
                else
                {
                    c.Log("LoadListStartup not autologin, logged in: " + c.IsLoggedIn() + ", locationtime: " + Session.LatestLocationTime + " Settings.IsMapView " + Settings.IsMapView + " mapToSet " + mapToSet);
                }
                
                //reloading list if it expired
                if (Session.LastDataRefresh is null || Session.LastDataRefresh < unixTimestamp - Constants.DataRefreshInterval)
                {
                    c.Log("LoadListStartup will load");

                    recenterMap = true;
                    if (Session.LastSearchType == Constants.SearchType_Filter)
                    {
                        Task.Run(() => LoadList());
                    }
                    else
                    {
                        Task.Run(() => LoadListSearch());
                    }
                }
                else //List is not being reloaded
                     //Would be used for setting map with the results loaded in ProfileView, but ViewDidLayoutSubviews is not called yet.
                     //Setting map after location authorization, (ViewDidLayoutSubviews is called before, but location is not available yet.)
                {
                    c.Log("Setting map only mapLoaded " + mapLoaded + " mapToSet " + mapToSet);

                    if (mapLoaded && mapToSet)
                    {
                        StartLoaderAnim();
                        mapSet = false;
                        recenterMap = true;
                        SetMap();
                    }
                    SetResultStatus();
                }
            }
        }

        public override void ViewDidLayoutSubviews() //Setting map region before this point results in incorrect zoom
        {
            base.ViewDidLayoutSubviews();

            c.SetShadow(MenuContainer, 0, 0, 10); //clipsToBounds / clipsSubviews and Layer.MaskToBound are exclusive. The outer view has the shadow, while the inner view of the same size clips the content.          

            c.SetRoundShadow(MapStreet, 1, 1, 1, 2, true);
            c.SetRoundShadow(MapSatellite, 1, 1, 1, 2, false);            

            MapStreet.Layer.CornerRadius = 2;
            MapStreet.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MinXMaxYCorner;
            MapSatellite.Layer.CornerRadius = 2;
            MapSatellite.Layer.MaskedCorners = CACornerMask.MaxXMinYCorner | CACornerMask.MaxXMaxYCorner;

            if (active) //this function is called after ViewWillDisappear when logging out, where Session has been nulled, thus throwing error in IsMapLocationAvailable()
            {
                mapLoaded = true;
                // Setting map with the results loaded in ProfileView
                // After location authorization ViewDidLayoutSubviews is called, but location is not available yet
                if (usersLoaded && mapToSet && c.IsMapLocationAvailable() && !listLoading)
                {
                    c.Log("ViewDidLayoutSubviews setting map");
                    StartLoaderAnim();
                    mapSet = false;
                    recenterMap = true;
                    Task.Run(() => SetMap());
                }
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (!c.IsLoggedIn())
            {
                Settings.SearchTerm = Session.SearchTerm;
                Settings.SearchIn = Session.SearchIn;

                Settings.ListType = Session.ListType;
                Settings.SortBy = Session.SortBy;
                Settings.OrderBy = Session.OrderBy;
                Settings.GeoFilter = Session.GeoFilter;
                Settings.GeoSourceOther = Session.GeoSourceOther;
                Settings.OtherLatitude = Session.OtherLatitude;
                Settings.OtherLongitude = Session.OtherLongitude;
                Settings.OtherAddress = Session.OtherAddress;
                Settings.DistanceLimit = Session.DistanceLimit;
                Settings.ResultsFrom = Session.ResultsFrom;
            }
            if ((int)ListViewMap.MapType != Settings.ListMapType)
            {
                Settings.ListMapType = (int)ListViewMap.MapType;
            }
            c.Log("ListActivity ViewWillDisappear SaveSettings");
            c.SaveSettings();

            if (firstRun && firstRunTimer.Enabled)
			{
				firstRunTimer.Stop();
			}
        }

		private void FirstRunTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			firstRunTimer.Stop();
			InvokeOnMainThread(() => {
				c.SnackIndef(LangEnglish.FirstRunMessage);
			});
			firstRun = false;
		}       

        private void LoggedInLayout()
        {
            StatusImage.Hidden = false;
            StatusText.Hidden = true;
            string url;
            if (Constants.isTestDB)
			{
				url = Constants.HostName + Constants.UploadFolderTest + "/" + Session.ID + "/" + Constants.SmallImageSize + "/" + Session.Pictures[0];
			}
			else
			{
				url = Constants.HostName + Constants.UploadFolder + "/" + Session.ID + "/" + Constants.SmallImageSize + "/" + Session.Pictures[0];
			}

            ImageCache im = new ImageCache(this);
            im.LoadImage(StatusImage, Session.ID.ToString(), Session.Pictures[0]);

            MenuChatList.Hidden = false;
            MenuChatListBg.Hidden = false;
            MenuChatListBgCorner.Hidden = false;
            c.ExpandX(MenuChatListBg);
        }

        private void LoggedOutLayout()
        {
            Session.SearchTerm = Settings.SearchTerm;
            Session.SearchIn = Settings.SearchIn;

            Session.ListType = Settings.ListType;
            Session.SortBy = Settings.SortBy;
            Session.OrderBy = Settings.OrderBy;
            Session.GeoFilter = Settings.GeoFilter;
            Session.GeoSourceOther = Settings.GeoSourceOther;
            Session.OtherLatitude = Settings.OtherLatitude;
            Session.OtherLongitude = Settings.OtherLongitude;
            Session.OtherAddress = Settings.OtherAddress;
            Session.DistanceLimit = Settings.DistanceLimit;
            Session.ResultsFrom = Settings.ResultsFrom;

            StatusImage.Hidden = true;
            StatusText.Hidden = false;
            StatusText.Text = LangEnglish.NotLoggedIn;

            MenuChatList.Hidden = true;
            MenuChatListBg.Hidden = true;
            MenuChatListBgCorner.Hidden = true;
            c.CollapseX(MenuChatListBg);
        }

        public void SetViews()
        {
            if (!(bool)Settings.IsMapView || (Session.UseLocation is null || !(bool)Session.UseLocation) && !((bool)Session.GeoFilter && (bool)Session.GeoSourceOther))
            {
                UserSearchList.Hidden = false;
                ListViewMap.Hidden = true;
                MapStreet.Hidden = true;
                MapSatellite.Hidden = true;
                ListView.SetBackgroundImage(UIImage.FromBundle("IcListallPressed"), UIControlState.Normal);
                MapView.SetBackgroundImage(UIImage.FromBundle("IcMap"), UIControlState.Normal);
                Settings.IsMapView = false;
            }
            else
            {
                UserSearchList.Hidden = true;
                ListViewMap.Hidden = false;
                MapStreet.Hidden = false;
                MapSatellite.Hidden = false;
                ListView.SetBackgroundImage(UIImage.FromBundle("IcListall"), UIControlState.Normal);
                MapView.SetBackgroundImage(UIImage.FromBundle("IcMapPressed"), UIControlState.Normal);
            }

            if (Settings.ListMapType == (int)MKMapType.Standard)
            {
                MapStreet.BackgroundColor = UIColor.FromName("MapButtonActive");
                MapSatellite.BackgroundColor = UIColor.FromName("MapButtonPassive");
            }
            else
            {
                MapStreet.BackgroundColor = UIColor.FromName("MapButtonPassive");
                MapSatellite.BackgroundColor = UIColor.FromName("MapButtonActive");
            }

            if (!(bool)Settings.SearchOpen)
            {
                c.CollapseY(SearchLayout);
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("IcSearch"), UIControlState.Normal);
            }
            else
            {
                Session.LastSearchType = Constants.SearchType_Search;
                c.ExpandY(SearchLayout);
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("IcSearchPressed"), UIControlState.Normal);
            }

            SearchTerm.Text = Session.SearchTerm;

            DropDownList entries = new DropDownList(LangEnglish.SearchInEntries, "SearchIn", 100, this);
            SearchIn.Model = entries;

            int index = LangEnglish.SearchInEntries_values.ToList().IndexOf(Session.SearchIn);
            SearchIn.Select(index, 0, false);

            if (!(bool)Settings.FiltersOpen)
            {
                c.CollapseY(FilterLayout);
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("IcFilters"), UIControlState.Normal);
            }
            else
            {
                Session.LastSearchType = Constants.SearchType_Filter;
                c.ExpandY(FilterLayout);
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("IcFiltersPressed"), UIControlState.Normal);
            }

            if (!(bool)Settings.GeoFiltersOpen)
            {
                c.CollapseY(DistanceFilters);
                distanceFiltersOpen = false;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("IcExpand"), UIControlState.Normal);
            }
            else
            {
                c.ExpandY(DistanceFilters);
                distanceFiltersOpen = true;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("IcCollapse"), UIControlState.Normal);
            }

            if (c.IsLoggedIn())
            {
                entries = new DropDownList(LangEnglish.ListTypeEntries, "ListType", 100, this);
                ListType.Model = entries;
                index = LangEnglish.ListTypeEntries_values.ToList().IndexOf(Session.ListType);
                ListType.Select(index, 0, false);
            }
            else
            {
                entries = new DropDownList(LangEnglish.ListTypeEntriesNotLoggedIn, "ListType", 100, this);
                ListType.Model = entries;
                index = LangEnglish.ListTypeEntriesNotLoggedIn_values.ToList().IndexOf(Session.ListType);
                ListType.Select(index, 0, false);
            }

            switch (Session.SortBy)
            {
                case "LastActiveDate":
                    SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("IcLightning36Pressed"), UIControlState.Normal);
                    SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("IcChatTwo36"), UIControlState.Normal);
                    SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("IcRegistered36"), UIControlState.Normal);
                    break;
                case "ResponseRate":
                    SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("IcLightning36"), UIControlState.Normal);
                    SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("IcChatTwo36Pressed"), UIControlState.Normal);
                    SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("IcRegistered36"), UIControlState.Normal);
                    break;
                case "RegisterDate":
                    SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("IcLightning36"), UIControlState.Normal);
                    SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("IcChatTwo36"), UIControlState.Normal);
                    SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("IcRegistered36Pressed"), UIControlState.Normal);
                    break;
            }

            if (Session.OrderBy == "desc")
            {
                //TooltipCompat.SetTooltipText(OrderBy, res.GetString(Resource.String.Descending));
                //OrderBy.TooltipText = res.GetString(Resource.String.Descending); //threw an error in Google's test, found out it is not supported proir to API 26.
                OrderBy.SetBackgroundImage(UIImage.FromBundle("IcDescending"), UIControlState.Normal);
            }
            else
            {
                //TooltipCompat.SetTooltipText(OrderBy, res.GetString(Resource.String.Ascending));
                //OrderBy.TooltipText = res.GetString(Resource.String.Ascending);
                OrderBy.SetBackgroundImage(UIImage.FromBundle("IcAscending"), UIControlState.Normal);
            }

            if (!(bool)Session.GeoFilter)
            {
                UseGeoNo.Checked = true;
                UseGeoYes.Checked = false;
                c.CollapseY(UseGeoContainer);
            }
            else
            {
                UseGeoNo.Checked = false;
                UseGeoYes.Checked = true;
                c.ExpandY(UseGeoContainer);
            }

            if (!(bool)Session.GeoSourceOther && c.IsLocationEnabled())
            {
                DistanceSourceCurrent.Checked = true;
                DistanceSourceAddress.Checked = false;
                c.CollapseY(DistanceSourceAddressText);
                c.CollapseY(AddressOK);
            }
            else
            {
                Session.GeoSourceOther = true;
                DistanceSourceCurrent.Checked = false;
                DistanceSourceAddress.Checked = true;
                c.ExpandY(DistanceSourceAddressText);
                c.ExpandY(AddressOK);
            }

            if (!string.IsNullOrEmpty(Session.OtherAddress))
            {
                DistanceSourceAddressText.Text = Session.OtherAddress;
            }
            else if (!(Session.OtherLatitude is null) && !(Session.OtherLongitude is null))
            {
                DistanceSourceAddressText.Text = ((double)Session.OtherLatitude).ToString(CultureInfo.InvariantCulture) + ", " + ((double)Session.OtherLongitude).ToString(CultureInfo.InvariantCulture);
            }

            DistanceLimit.Value = (int)Session.DistanceLimit;
            DistanceLimitInput.Text = Session.DistanceLimit.ToString();
        }

        private void StatusImage_Click(object sender, EventArgs e)
        {
            IntentData.profileViewPageType = Constants.ProfileViewType_Self;

            CommonMethods.OpenPage("ProfileViewActivity", 1);
        }

        private void OpenFilters_Click(object sender, EventArgs e)
        {
            View.EndEditing(true);
            if (!(bool)Settings.FiltersOpen)
            {
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("IcFiltersPressed"), UIControlState.Normal);
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("IcSearch"), UIControlState.Normal);
                c.ExpandY(FilterLayout);
                
                if ((bool)Settings.SearchOpen)
                {
                    c.CollapseY(SearchLayout);
                    Settings.SearchOpen = false;
                }
                else
                {
                    UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                }
                Settings.FiltersOpen = true;
                Session.LastDataRefresh = null;
                if (Session.LastSearchType == Constants.SearchType_Search)
                {
                    Session.ResultsFrom = 1;
                    recenterMap = true;
                    Task.Run(() => LoadList());
                }
            }
            else
            {
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("IcFilters"), UIControlState.Normal);
                c.CollapseY(FilterLayout);
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                Settings.FiltersOpen = false;
            }
        }

        private void OpenSearch_Click(object sender, EventArgs e)
        {
            View.EndEditing(true);
            if (!(bool)Settings.SearchOpen)
            {
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("IcFilters"), UIControlState.Normal);
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("IcSearchPressed"), UIControlState.Normal);
                c.ExpandY(SearchLayout);

                if ((bool)Settings.FiltersOpen)
                {
                    c.CollapseY(FilterLayout);
                    Settings.FiltersOpen = false;
                }
                else
                {
                    UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                }
                Settings.SearchOpen = true;
                Session.LastDataRefresh = null;
                if (Session.LastSearchType == Constants.SearchType_Filter)
                {
                    Session.ResultsFrom = 1;
                    recenterMap = true;
                    Task.Run(() => LoadListSearch());
                }
            }
            else
            {
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("IcSearch"), UIControlState.Normal);
                c.CollapseY(SearchLayout);
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                Settings.SearchOpen = false;
            }
        }

        private void ListView_Click(object sender, EventArgs e)
        {
            ListView.SetBackgroundImage(UIImage.FromBundle("IcListallPressed"),UIControlState.Normal);
            MapView.SetBackgroundImage(UIImage.FromBundle("IcMap"), UIControlState.Normal);

            UserSearchList.Hidden = false;
            ListViewMap.Hidden = true;
            MapStreet.Hidden = true;
            MapSatellite.Hidden = true;
            Settings.IsMapView = false;

            SetResultStatus();
        }

        private void MapView_Click(object sender, EventArgs e)
        {
            mapToSet = true;
            if (CheckLocationSettings())
            {
                MapViewSecond();
            };
        }

        private bool CheckLocationSettings()
        {
            c.Log("CheckLocationSettings UseLocation " + Session.UseLocation + " IsLocationEnabled " + c.IsLocationEnabled());
            if ((Session.UseLocation is null || !(bool)Session.UseLocation || !c.IsLocationEnabled()) && !((bool)Session.GeoFilter && (bool)Session.GeoSourceOther))
            {
                //cases: - not logged in user not granted location access
                //		 - logged in user with use location setting off and not granted location access
                //		 - logged in use with use location setting off, but granted location access.
                if (!c.IsLoggedIn())
                {
                    c.DisplayCustomDialog("", LangEnglish.MapViewNoLocation, LangEnglish.DialogYes, LangEnglish.DialogNo, alert => RequestPermissions(), alert => {
                        mapToSet = false;
                        SetDistanceSourceAddress();
                    });
                }
                else
                {
                    if (!c.IsLocationEnabled())
                    {
                        c.DisplayCustomDialog("", LangEnglish.MapViewNoLocation, LangEnglish.DialogYes, LangEnglish.DialogNo, alert => RequestPermissions(), alert => {
                            mapToSet = false;
                            SetDistanceSourceAddress();
                        });
                    }
                    else //permission is granted, but UseLocation is off (coming from MapView_Click)
                    {
                        c.DisplayCustomDialog("", LangEnglish.MapViewNoUseLocation, LangEnglish.DialogYes, LangEnglish.DialogNo, alert => {
                            if (UpdateLocationSetting(true))
                            {
                                firstLocationAcquired = false;

                                ResultSet.Hidden = false;
                                ResultSet.Text = LangEnglish.GettingLocation;

                                if (locMgr is null)
                                {
                                    locMgr = new LocationManager(this);
                                }
                                locMgr.StartLocationUpdates(); //will load the list or set the map only
                            }
                        }, alert => {
                            mapToSet = false;
                            SetDistanceSourceAddress();
                        });
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private void RequestPermissions()
        {
            CLLocationManager locationManager = new CLLocationManager();
            locationManager.AuthorizationChanged += LocationManager_AuthorizationChanged;

            //Location seems to be working in the background with this
            locationManager.RequestWhenInUseAuthorization();
            /*if (c.IsLoggedIn() && (bool)Session.BackgroundLocation)
            {
                locationManager.RequestAlwaysAuthorization();   
            }
            else
            {
                locationManager.RequestWhenInUseAuthorization();
            }*/
        }

        private void LocationManager_AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
        {
            c.Log("LocationManager_AuthorizationChanged " + e.Status + " logged in " + c.IsLoggedIn() + " distanceSourceCurrentClicked " + distanceSourceCurrentClicked + " mapToSet " + mapToSet);
            if (e.Status == CLAuthorizationStatus.AuthorizedAlways || e.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                if (c.IsLoggedIn())
                {
                    if (c.snackPermanentText == LangEnglish.LocationDisabledButUsingLocation && c.snackVisible)
                    {
                        c.HideSnack();
                    }

                    if (distanceSourceCurrentClicked)
                    {
                        c.Log("PM logged in granted distanceSourceCurrentClicked");
                        //it resets to address by itself
                        DistanceSourceCurrent.Checked = true;
                        DistanceSourceAddress.Checked = false;
                        Session.GeoSourceOther = false;
                        Session.LastDataRefresh = null;
                        distanceSourceCurrentClicked = false;
                    }
                    else
                    {
                        c.Log("PM logged in granted map clicked, mapToSet " + mapToSet);
                    }
                    Session.LocationTime = null;
                    UpdateLocationSetting(true);
                }
                else
                {
                    Session.UseLocation = true;
                    Session.LocationTime = null;

                    if (distanceSourceCurrentClicked)
                    {
                        c.Log("PM not logged in granted distanceSourceCurrentClicked");
                        Session.GeoSourceOther = Settings.GeoSourceOther = false;
                        Session.LastDataRefresh = null;
                        distanceSourceCurrentClicked = false;
                    }
                    else
                    {
                        c.Log("PM not logged in granted map clicked, mapToSet " + mapToSet);
                    }
                }

                firstLocationAcquired = false;

                ResultSet.Hidden = false;
                ResultSet.Text = LangEnglish.GettingLocation;

                if (locMgr is null)
                {
                    locMgr = new LocationManager(this);
                }
                locMgr.StartLocationUpdates(); //will load the list or only set the map
            }
            else if (e.Status == CLAuthorizationStatus.Denied)
            {
                mapToSet = false;
                distanceSourceCurrentClicked = false;
                Session.UseLocation = false;
                SetDistanceSourceAddress();
                c.Snack(LangEnglish.LocationNotGranted); //in the dialog the user choose to turn on location, but now denied it. Message needs to be shown.
                if (c.IsLoggedIn())
                {
                    UpdateLocationSetting(false);
                }
            }
        }

        public bool UpdateLocationSetting(bool useLocation)
        {
            string url = "action=profileedit&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&UseLocation=" + useLocation;
            string responseString = c.MakeRequestSync(url);
            if (responseString.Substring(0, 2) == "OK")
            {
                Session.UseLocation = true;
                
                if (distanceSourceCurrentClicked)
                {
                    Session.LastDataRefresh = null;
                    distanceSourceCurrentClicked = false;
                }
                return true;
            }
            else
            {
                c.ReportError(responseString);
                return false;
            }
        }

        private void MapViewSecond()
        {
            c.Log("MapViewSecond usersloaded " + usersLoaded + " Settings.IsMapView " + Settings.IsMapView +" mapSet" + mapSet + " mapToSet " + mapToSet);
            if (usersLoaded && !(bool)Settings.IsMapView)
            {
                if (!mapSet) {
                    StartLoaderAnim();
                    recenterMap = true;
                    Task.Run(() => { SetMap(); });
                }
                else
                {
                    mapToSet = false;

                    MapView.SetBackgroundImage(UIImage.FromBundle("IcMapPressed"), UIControlState.Normal);
                    ListView.SetBackgroundImage(UIImage.FromBundle("IcListall"), UIControlState.Normal);

                    UserSearchList.Hidden = true;
                    ListViewMap.Hidden = false;
                    MapStreet.Hidden = false;
                    MapSatellite.Hidden = false;
                    Settings.IsMapView = true;
                }
            }
        }

        private void MenuIcon_Click(object sender, EventArgs e)
        {
            c.ShowMenu(MenuLayer, MenuContainer);
        }

        private void MenuLayer_TouchDown(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
        }

        private void MenuLogOut_Click(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
            IntentData.logout = true;
            CommonMethods.OpenPage("MainActivity", 1);
        }

        private void MenuLogIn_Click(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
            CommonMethods.OpenPage("MainActivity", 1);
        }

        private void MenuRegister_Click(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
            CommonMethods.OpenPage("RegisterActivity", 1);
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
            CommonMethods.OpenPage("SettingsActivity", 1);
        }

        private void MenuLocation_Click(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
            //c.LogLocationAlert();
            CommonMethods.OpenPage("LocationActivity", 1);
        }

        private void MenuHelpCenter_Click(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
            CommonMethods.OpenPage("HelpCenterActivity", 1);
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            c.HideMenu(MenuLayer, MenuContainer, true);
            c.AlertLinks(LangEnglish.versionInfo);
        }

        private void SetDistanceSourceAddress()
        {
            DistanceSourceCurrent.Checked = false;
            DistanceSourceAddress.Checked = true;
            c.ExpandY(DistanceSourceAddressText);
            c.ExpandY(AddressOK);
            Session.GeoSourceOther = true;
        }

        private void SetLoggedInMenu()
        {
            MenuLogOut.Hidden = false;
            MenuLogIn.Hidden = true;
            MenuRegister.Hidden = true;
        }

        private void SetLoggedOutMenu()
        {
            MenuLogOut.Hidden = true;
            MenuLogIn.Hidden = false;
            MenuRegister.Hidden = false;
        }

        [Export("searchBarSearchButtonClicked:")]
        public void SearchButtonClicked(UISearchBar searchBar)
        {
            View.EndEditing(true);
            if ((bool)Settings.IsMapView) // if I load the data immediately while the keyboard is open, the pictures will not appear on the map. Not even keyboardAnimationDuration (0.25) is enough, even though the loading takes time too (ca. 100 ms).
            //A total of 441 ms can still fail.
            {
                Timer t = new Timer();

                t.Elapsed += T_Elapsed1;
                t.Interval = keyboardAnimationDuration * 1000 * 2; // = 500
                t.Start();
            }
            else
            {
                Session.ResultsFrom = 1;
                recenterMap = true;
                Task.Run(() => LoadListSearch());
            }
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            View.EndEditing(true); //calls EditingEnded too
            if (textField == DistanceSourceAddressText)
            {
                GetAddressCoordinates();
            }
            else if (textField == DistanceLimitInput)
            {
                bool parsed = int.TryParse(DistanceLimitInput.Text, out int progress);
                if (!parsed)
                {
                    c.Alert(LangEnglish.WrongNumberFormat);
                    return false;
                }

                if (progress > Constants.MaxGoogleMapDistance)
                {
                    return false;
                }
                if (progress < 1)
                {
                    return false;
                }
                SetDistanceLimit();

                if ((bool)Settings.IsMapView)
                {
                    Timer t = new Timer();

                    t.Elapsed += T_Elapsed2;
                    t.Interval = keyboardAnimationDuration * 1000 * 2; // = 500
                    t.Start();
                }
                else
                {
                    recenterMap = false;
                    Task.Run(() => LoadList());
                }
            }
            return true;
        }

        private void T_Elapsed1(object sender, ElapsedEventArgs e)
        {
            ((Timer)sender).Stop();
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadListSearch());
        }

        private void T_Elapsed2(object sender, ElapsedEventArgs e)
        {
            ((Timer)sender).Stop();
            recenterMap = false;
            Task.Run(() => LoadList());
        }

        private void ChangesDetected(object sender, NSNotificationEventArgs nSNotificationEventArgs)
        {
            if ((UITextField)nSNotificationEventArgs.Notification.Object == DistanceSourceAddressText)
            {
                MatchCoordinates(false);
            }
        }

        [Export("textFieldDidEndEditing:")]
        public void EditingEnded(UITextField textField)
        {
            if (textField == DistanceSourceAddressText)
            {
                MatchCoordinates(true);
                DistanceSourceAddressText.BackgroundColor = null;
                //DistanceSourceAddressText.Background.ClearColorFilter();
            }
            else if (textField == DistanceLimitInput)
            {
                bool parsed = int.TryParse(DistanceLimitInput.Text, out int progress);
                if (!parsed)
                {
                    c.Alert(LangEnglish.WrongNumberFormat);
                    return;
                }

                if (progress > Constants.MaxGoogleMapDistance)
                {
                    c.Alert(LangEnglish.MaxDistanceMessage);
                    return;
                }
                if (progress < 1)
                {
                    c.Alert(LangEnglish.MinDistanceMessage);
                    return;
                }

                SetDistanceLimit();
            }
        }

        public void SearchIn_ItemSelected()
        {
            View.EndEditing(true);
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadListSearch());
        }

        public void ListType_ItemSelected()
        {
            if (c.IsLoggedIn())
            {
                Session.ListType = LangEnglish.ListTypeEntries_values[ListType.SelectedRowInComponent(0)];
            }
            else
            {
                Session.ListType = LangEnglish.ListTypeEntriesNotLoggedIn_values[ListType.SelectedRowInComponent(0)];
            }
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        private void SortBy_LastActiveDate_Click(object sender, EventArgs e)
        {
            SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("IcLightning36Pressed"), UIControlState.Normal);
            SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("IcChatTwo36"), UIControlState.Normal);
            SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("IcRegistered36"), UIControlState.Normal);
            Session.SortBy = "LastActiveDate";
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        private void SortBy_ResponseRate_Click(object sender, EventArgs e)
        {
            SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("IcLightning36"), UIControlState.Normal);
            SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("IcChatTwo36Pressed"), UIControlState.Normal);
            SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("IcRegistered36"), UIControlState.Normal);
            Session.SortBy = "ResponseRate";
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        private void SortBy_RegisterDate_Click(object sender, EventArgs e)
        {
            SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("IcLightning36"), UIControlState.Normal);
            SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("IcChatTwo36"), UIControlState.Normal);
            SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("IcRegistered36Pressed"), UIControlState.Normal);
            Session.SortBy = "RegisterDate";
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        private void OrderBy_Click(object sender, EventArgs e)
        {
            if (Session.OrderBy == "desc")
            {
                Session.OrderBy = "asc";
                //OrderBy.TooltipText = res.GetString(Resource.String.Ascending);
                OrderBy.SetBackgroundImage(UIImage.FromBundle("IcAscending"), UIControlState.Normal);
            }
            else
            {
                Session.OrderBy = "desc";
                //OrderBy.TooltipText = res.GetString(Resource.String.Descending);
                OrderBy.SetBackgroundImage(UIImage.FromBundle("IcDescending"), UIControlState.Normal);
            }
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        private void DistanceFiltersOpenClose_Click(object sender, EventArgs e)
        {
            if (!distanceFiltersOpen)
            {
                c.ExpandY(DistanceFilters);
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                distanceFiltersOpen = true;
                Settings.GeoFiltersOpen = true;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("IcCollapse"), UIControlState.Normal);
                //DistanceFiltersOpenClose.TooltipText = res.GetString(Resource.String.DistanceFiltersClose);
            }
            else
            {
                c.CollapseY(DistanceFilters);
                UIView.Animate(Constants.tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                distanceFiltersOpen = false;
                Settings.GeoFiltersOpen = false;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("IcExpand"), UIControlState.Normal);
                //DistanceFiltersOpenClose.TooltipText = res.GetString(Resource.String.DistanceFiltersOpen);
            }
        }

        public void UseGeo_Click(bool geoYes)
        {
            if (!geoYes)
            {
                UseGeoYes.Checked = false;
                c.CollapseY(UseGeoContainer);
                Session.GeoFilter = false;
                if ((bool)Settings.IsMapView && (!(bool)Session.UseLocation || !c.IsLocationEnabled()))
                {
                    ListView_Click(null, null);
                }
                recenterMap = true;
                Task.Run(() => LoadList());
            }
            else
            {
                UseGeoNo.Checked = false;
                c.ExpandY(UseGeoContainer);
                Session.GeoFilter = true;
                if (!(bool)Session.GeoSourceOther && c.IsOwnLocationAvailable() || (bool)Session.GeoSourceOther && c.IsOtherLocationAvailable())
                {
                    recenterMap = true;
                    Task.Run(() => LoadList());
                }
            }
        }

        private void RefreshDistance_TouchDown(object sender, EventArgs e)
        {
            c.AnimateRipple(RippleRefreshDistance, 2);
        }

        private void RefreshDistance_Click(object sender, EventArgs e)
        {
            View.EndEditing(true);
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        public void DistanceSource_Click(bool sourceCurrent)
        {
            if (sourceCurrent)
            {
                c.Log("DistanceSource_Click current IsLocationEnabled " + c.IsLocationEnabled() + " UseLocation " + Session.UseLocation);

                DistanceSourceAddress.Checked = false;
                c.CollapseY(DistanceSourceAddressText);
                c.CollapseY(AddressOK);
                if (!c.IsLocationEnabled())
                {
                    distanceSourceCurrentClicked = true;
                    RequestPermissions();
                    return;
                }
                else if (!(bool)Session.UseLocation) //can only mean, user is logged in
                {
                    c.DisplayCustomDialog("", LangEnglish.MapViewNoUseLocation, LangEnglish.DialogYes, LangEnglish.DialogNo, alert => {
                        if (UpdateLocationSetting(true))
                        {
                            Session.GeoSourceOther = false;
                            Session.LastDataRefresh = null;
                            firstLocationAcquired = false;

                            ResultSet.Hidden = false;
                            ResultSet.Text = LangEnglish.GettingLocation;

                            if (locMgr is null)
                            {
                                locMgr = new LocationManager(this);
                            }
                            locMgr.StartLocationUpdates(); //will load the list
                        }
                    }, alert => {
                        SetDistanceSourceAddress();
                    });
                    return; //wihtout returning, function would continue with DistanceSourceAddress.Checked False, and list would not load anyway because current location is unavailable.
                }
            }
            else
            {
                DistanceSourceCurrent.Checked = false;
                c.ExpandY(DistanceSourceAddressText);
                c.ExpandY(AddressOK);
            }

            Session.GeoSourceOther = DistanceSourceAddress.Checked;
            if (!(bool)Session.GeoSourceOther && c.IsOwnLocationAvailable() || (bool)Session.GeoSourceOther && c.IsOtherLocationAvailable())
            {
                Session.ResultsFrom = 1;
                recenterMap = true;
                Task.Run(() => LoadList());
            }
        }

        private void AddressOK_Click(object sender, EventArgs e)
        {
            View.EndEditing(true);
            GetAddressCoordinates();
        }

        private bool MatchCoordinates(bool reformat)
        {
            if (distanceSourceAddressTextChanging)
            {
                return true;
            }
            distanceSourceAddressTextChanging = true;
            string lookup = DistanceSourceAddressText.Text.Trim();
            Regex regex = new Regex(@"^(-?[0-9]+(\.[0-9]+)?)[,\s]+(-?[0-9]+(\.[0-9]+)?)$"); //when the email extension is long, it will take ages for the regex to finish
            var matches = regex.Matches(lookup);
            if (matches.Count != 0)
            {
                var match = matches[0];
                double latValue = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                double longValue = double.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                if (latValue <= 90 && latValue >= -90 && longValue <= 180 && longValue >= -180)
                {
                    Session.OtherLatitude = latValue;
                    Session.OtherLongitude = longValue;
                    Session.OtherAddress = null;
                    DistanceSourceAddressText.BackgroundColor = UIColor.FromName("DistanceSourceHighlight");
                    if (reformat)
                    {
                        DistanceSourceAddressText.Text = latValue + ", " + longValue;
                        //DistanceSourceAddressText.ClearFocus();
                    }
                    distanceSourceAddressTextChanging = false;
                    return true;
                }
                else
                {
                    DistanceSourceAddressText.BackgroundColor = null;
                    //DistanceSourceAddressText.Background.ClearColorFilter();
                    distanceSourceAddressTextChanging = false;
                    return false;
                }
            }
            else
            {
                DistanceSourceAddressText.BackgroundColor = null;
                //DistanceSourceAddressText.Background.ClearColorFilter();
            }
            distanceSourceAddressTextChanging = false;
            return false;
        }

        private async void GetAddressCoordinates()
        {
            if (DistanceSourceAddressText.Text.Trim() != "")
            {
                if (!MatchCoordinates(true))
                {
                    AddressOK.UserInteractionEnabled = false;
                    AddressOK.Alpha = 0.5f;

                    StartLoaderAnim();
                    ResultSet.Hidden = false;
                    ResultSet.Text = LangEnglish.ConvertingAddress;

                    string responseString = "";

                    string lookup = DistanceSourceAddressText.Text.Trim();
                    string url = "action=geocoding&Address=" + lookup + "&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                    responseString = await c.MakeRequest(url);
                    if (responseString.Substring(0, 2) == "OK")
                    {
                        responseString = responseString.Substring(3);
                        int sep1Pos = responseString.IndexOf("|");
                        int sep2Pos = responseString.IndexOf("|", sep1Pos + 1);

                        distanceSourceAddressTextChanging = true;
                        DistanceSourceAddressText.Text = Session.OtherAddress = responseString.Substring(0, sep1Pos);
                        //DistanceSourceAddressText.ClearFocus();
                        distanceSourceAddressTextChanging = false;

                        Session.OtherLatitude = double.Parse(responseString.Substring(sep1Pos + 1, sep2Pos - sep1Pos - 1), CultureInfo.InvariantCulture);
                        Session.OtherLongitude = double.Parse(responseString.Substring(sep2Pos + 1), CultureInfo.InvariantCulture);

                        recenterMap = true;
                        await Task.Run(() => LoadList());
                    }
                    else if (responseString == "ZERO_RESULTS")
                    {
                        c.Snack(LangEnglish.AddressNoResult);
                    }
                    else if (responseString == "OVER_QUERY_LIMIT")
                    {
                        c.SnackIndef(LangEnglish.OverQueryLimit);
                    }
                    else //Network error, authorization error or other geocoding status code
                    {
                        c.ReportError(responseString);
                    }

                    SetResultStatus();
                    StopLoaderAnim();

                    AddressOK.UserInteractionEnabled = true;
                    AddressOK.Alpha = 1f;
                }
                else
                {
                    recenterMap = true;
                    await Task.Run(() => LoadList());
                }

            }
            else
            {
                Session.OtherLatitude = null;
                Session.OtherLongitude = null;
                Session.OtherAddress = null;
            }
            DistanceSourceAddressText.BackgroundColor = null;
            //DistanceSourceAddressText.Background.ClearColorFilter();
        }

        private void RevertInvalidAddress()
        {
            if (!(Session.OtherAddress is null))
            {
                distanceSourceAddressTextChanging = true;
                DistanceSourceAddressText.Text = Session.OtherAddress;
                distanceSourceAddressTextChanging = false;
            }
            else if (Session.OtherLatitude != null && Session.OtherLongitude != null)
            {
                distanceSourceAddressTextChanging = true;
                DistanceSourceAddressText.Text = Session.OtherLatitude + ", " + Session.OtherLongitude;
                distanceSourceAddressTextChanging = false;
            }
        }

		private void DistanceLimit_ValueChanged(object sender, EventArgs e)
        {
            if (!distanceLimitChangedByCode)
            {
                DistanceLimitInput.Text = Math.Round(DistanceLimit.Value).ToString();
                //DistanceLimitInput.ClearFocus();

                Session.DistanceLimit = (int)Math.Round(DistanceLimit.Value);
                if (ProgressTimer is null || !ProgressTimer.Enabled)
                {
                    ProgressTimer = new Timer();
                    ProgressTimer.Interval = Constants.DistanceChangeRefreshDelay;
                    ProgressTimer.Elapsed += ProgressTimer_Elapsed;
                    ProgressTimer.Start();
                }
                else if (!(ProgressTimer is null))
                {
                    ProgressTimer.Stop();
                    ProgressTimer.Start();
                }
            }
            else
            {
                distanceLimitChangedByCode = false;
            }
        }

		private void ProgressTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProgressTimer.Stop();
            if ((bool)Session.GeoFilter && (!(bool)Session.GeoSourceOther && !c.IsOwnLocationAvailable() || (bool)Session.GeoSourceOther && !c.IsOtherLocationAvailable()))
            {
                return;
            }
            recenterMap = false;
            Task.Run(() => LoadList());
        }

        private void SetDistanceLimit()
        {
            //invalid values are reverted.
            bool parsed = int.TryParse(DistanceLimitInput.Text, out int progress);
            if (!parsed)
            {
                DistanceLimitInput.Text = Session.DistanceLimit.ToString();
                if (DistanceLimitInput.Focused)
                {
                    DistanceLimitInput.SelectAll(null);
                }
                return;
            }

            if (progress > Constants.MaxGoogleMapDistance || progress < 1)
            {
                DistanceLimitInput.Text = Session.DistanceLimit.ToString();
                if (DistanceLimitInput.Focused)
                {
                    DistanceLimitInput.SelectAll(null);
                }
                return;
            }

            //New values are set.
            if (progress != Session.DistanceLimit)
            {
                distanceLimitChangedByCode = true;
                if (progress <= Constants.DistanceLimitMax) //triggers DistanceLimit_ProgressChanged
                {
                    DistanceLimit.Value = progress;
                }
                else
                {
                    DistanceLimit.Value = Constants.DistanceLimitMax;
                }
                Session.DistanceLimit = progress;
            }
            View.EndEditing(true);
        }

        private void UserSearchList_ItemClick(UITapGestureRecognizer recognizer)
        {
            View.EndEditing(true);

            var location = recognizer.LocationInView(UserSearchList);
            var indexPath = UserSearchList.IndexPathForItemAtPoint(location);

            if (indexPath != null)
            {
                IntentData.profileViewPageType = Constants.ProfileViewType_List;
                viewProfiles = new List<Profile>(listProfiles);
                viewIndex = indexPath.Row;
                absoluteIndex = viewIndex + (int)Session.ResultsFrom - 1;
                absoluteFirstIndex = absoluteStartIndex = (int)Session.ResultsFrom - 1;

                CommonMethods.OpenPage("ProfileViewActivity", 1);

                //c.CW("UserSearchList_ItemClick viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " absoluteStartIndex " + ListActivity.absoluteStartIndex + " ResultsFrom " + Session.ResultsFrom + " view count " + ListActivity.viewProfiles.Count);
            }
        }



        private void Refresh_ValueChanged(object sender, EventArgs e)
        {
            Session.ResultsFrom = 1;
            recenterMap = true;
            if (Session.LastSearchType == Constants.SearchType_Filter)
            {
                Task.Run(() => LoadList());
            }
            else
            {
                Task.Run(() => LoadListSearch());
            }
        }

        /*private void UserSearchList_Touch(UIPanGestureRecognizer recognizer)
        {
            var location = recognizer.LocationInView(UserSearchList);
            if (recognizer.State == UIGestureRecognizerState.Began)
            {
                UserSearchList_Down(location.Y);
            }

            if (recognizer.State == UIGestureRecognizerState.Changed)
            {
                UserSearchList_Move(location.Y);
            }

            if (recognizer.State != (UIGestureRecognizerState.Cancelled | UIGestureRecognizerState.Failed | UIGestureRecognizerState.Possible))
            {
            }

            if (recognizer.State == UIGestureRecognizerState.Ended)
            {
                UserSearchList_Up();
            }
        }

        public void UserSearchList_Down(nfloat y)
        {
            if (listLoading || UserSearchList.ContentOffset.Y != 0)
            {
                return;
            }
            startY = y;
            diff = 0; // (Android) without this, it can interfere with a click where only down and up are called, and therefore results in unnecessary refresh
        }

        public void UserSearchList_Move(nfloat y)
        {

            if (startY is null)
            {
                return;
            }
            diff = y - (nfloat)startY;
            if (diff >= 0 && diff < maxY)
            {
                ReloadPulldown.Center = new CGPoint(ReloadPulldown.Center.X, -loaderHeight / 2 + diff / 2);
                ReloadPulldown.Alpha = diff / maxY;

                CGAffineTransform t = CGAffineTransform.MakeRotation((nfloat)(diff / maxY * 2 * Math.PI));
                ReloadPulldown.Transform = t;
            }
            else if (diff >= maxY)
            {
                ReloadPulldown.Center = new CGPoint(ReloadPulldown.Center.X, -loaderHeight / 2 + maxY / 2);
                ReloadPulldown.Alpha = 1;

                CGAffineTransform t = CGAffineTransform.MakeRotation(0);
                ReloadPulldown.Transform = t;

                startY += (int)(diff - maxY);
            }
            else if (diff < 0)
            {
                ReloadPulldown.Center = new CGPoint(ReloadPulldown.Center.X, -loaderHeight / 2);
                startY += (int)diff;
            }
        }

        public void UserSearchList_Up()
        {
            if (startY is null)
            {
                return;
            }
            if (diff >= maxY)
            {
                Session.ResultsFrom = 1;
                recenterMap = true;
                if (Session.LastSearchType == Constants.SearchType_Filter)
                {
                    Task.Run(() => LoadList());
                }
                else
                {
                    Task.Run(() => LoadListSearch());
                }
            }
            else
            {
                UIView.Animate(duration: Constants.tweenTime, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
                {
                ReloadPulldown.Center = new CGPoint(ReloadPulldown.Center.X, -loaderHeight / 2);
                    ReloadPulldown.Alpha = 0;
                }, completion: () => { });
            }
            startY = null;
        }*/

        private void MapStreet_Click(object sender, EventArgs e)
        {
            ListViewMap.MapType = MKMapType.Standard;
            MapStreet.BackgroundColor = UIColor.FromName("MapButtonActive");
            MapSatellite.BackgroundColor = UIColor.FromName("MapButtonPassive");
        }

        private void MapSatellite_Click(object sender, EventArgs e)
        {
            ListViewMap.MapType = MKMapType.Satellite;
            MapStreet.BackgroundColor = UIColor.FromName("MapButtonPassive");
            MapSatellite.BackgroundColor = UIColor.FromName("MapButtonActive");
        }

        private void LoadPrevious_Click(object sender, EventArgs e)
        {
            LoadPrevious.Enabled = false; //to prevent repeated clicks
            LoadPrevious.Alpha = 0.5f;

            Session.ResultsFrom = Session.ResultsFrom - Constants.MaxResultCount;
            if (Session.ResultsFrom < 1) //with consistent MaxResultCount it shouldn't happen.
            {
                Session.ResultsFrom = 1;
            }
            recenterMap = false;
            if (Session.LastSearchType == Constants.SearchType_Filter)
            {
                Task.Run(() => LoadList());
            }
            else
            {
                Task.Run(() => LoadListSearch());
            }
        }

        private void LoadNext_Click(object sender, EventArgs e)
        {
            LoadNext.Enabled = false;
            LoadNext.Alpha = 0.5f;

            Session.ResultsFrom = Session.ResultsFrom + listProfiles.Count;
            recenterMap = false;
            if (Session.LastSearchType == Constants.SearchType_Filter)
            {
                Task.Run(() => LoadList());
            }
            else
            {
                Task.Run(() => LoadListSearch());
            }
        }

        public void LoadList()
        {
            try
            {
                c.Log("LoadList listLoading " + listLoading + " caller " + new StackFrame(1, true).GetMethod().Name);
                if (listLoading)
                {
                    return;
                }

                if (Session.GeoFilter is null) //in case user logged out before location timeout
                {
                    return;
                }

                //last check
                if ((bool)Session.GeoFilter && (!(bool)Session.GeoSourceOther && !c.IsOwnLocationAvailable() || (bool)Session.GeoSourceOther && !c.IsOtherLocationAvailable()))
                {
                    InvokeOnMainThread(() => {
                        SetResultStatus();
                        c.Log("Exiting loadlist GeoFilter " + Session.GeoFilter + " GeoSourceOther " + Session.GeoSourceOther
                            + " own location " + c.IsOwnLocationAvailable() + " other location " + c.IsOtherLocationAvailable());
                        c.SnackIndef(LangEnglish.GeoFilterNoLocation);

                        if (refresh.Refreshing)
                        {
                            refresh.EndRefreshing();
                        }
                        /*if (ReloadPulldown.Alpha == 1)
                        {
                            HidePulldown();
                        }*/
                    });
                    return;
                }

                if (c.snackPermanentText == LangEnglish.GeoFilterNoLocation && c.snackVisible)
                {
                    InvokeOnMainThread(() => {
                        c.HideSnack();
                    });
                }

                InvokeOnMainThread(() => {
                    SetDistanceLimit();
                    RevertInvalidAddress();
                });

                listLoading = true;

                InvokeOnMainThread(() => {
                    StartLoaderAnim();
                    ResultSet.Text = LangEnglish.LoadingList; //makes reloadpulldown and snackbar jump back to its constraint value
                    ResultSet.Hidden = false;
                    LoadNext.Hidden = true;
                    LoadPrevious.Hidden = true;
                });

                Session.LastSearchType = Constants.SearchType_Filter;

                string latitudeStr;
                string longitudeStr;

                if (!Constants.SafeLocationMode)
                {
                    latitudeStr = (Session.Latitude is null) ? "" : ((double)Session.Latitude).ToString(CultureInfo.InvariantCulture);
                    longitudeStr = (Session.Longitude is null) ? "" : ((double)Session.Longitude).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    latitudeStr = (Session.LatestLatitude is null) ? "" : ((double)Session.LatestLatitude).ToString(CultureInfo.InvariantCulture);
                    longitudeStr = (Session.LatestLongitude is null) ? "" : ((double)Session.LatestLongitude).ToString(CultureInfo.InvariantCulture);
                }
                
                string otherLatitudeStr = (Session.OtherLatitude is null) ? "" : ((double)Session.OtherLatitude).ToString(CultureInfo.InvariantCulture);
                string otherLongitudeStr = (Session.OtherLongitude is null) ? "" : ((double)Session.OtherLongitude).ToString(CultureInfo.InvariantCulture);

                LoadResults("action=list&ID=" + Session.ID + "&SessionID=" + Session.SessionID +
                    "&Latitude=" + latitudeStr + "&Longitude=" + longitudeStr +
                    "&ListType=" + Session.ListType + "&SortBy=" + Session.SortBy + "&OrderBy=" + Session.OrderBy + "&GeoFilter=" + Session.GeoFilter +
                    "&GeoSourceOther=" + Session.GeoSourceOther + "&OtherLatitude=" + otherLatitudeStr + "&OtherLongitude=" + otherLongitudeStr +
                    "&OtherAddress=" + CommonMethods.UrlEncode(Session.OtherAddress) + "&DistanceLimit=" + Session.DistanceLimit + "&ResultsFrom=" + Session.ResultsFrom);
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine + c.ShowClass<Session>());
            }

        }

        public void LoadListSearch()
        {
            try
            {
                c.Log("LoadListSearch listLoading: " + listLoading);
                if (listLoading)
                {
                    return;
                }

                InvokeOnMainThread(() =>
                {
                    Session.SearchTerm = SearchTerm.Text.Trim();
                    Session.SearchIn = LangEnglish.SearchInEntries_values[SearchIn.SelectedRowInComponent(0)];
                }); 

                listLoading = true;

                InvokeOnMainThread(() =>
                {
                    StartLoaderAnim();
                    ResultSet.Text = LangEnglish.LoadingList;
                    ResultSet.Hidden = false;
                    LoadNext.Hidden = true; 
                    LoadPrevious.Hidden = true;
                });

                Session.LastSearchType = Constants.SearchType_Search;

                string latitudeStr;
                string longitudeStr;

                if (!Constants.SafeLocationMode)
                {
                    latitudeStr = (Session.Latitude is null) ? "" : ((double)Session.Latitude).ToString(CultureInfo.InvariantCulture);
                    longitudeStr = (Session.Longitude is null) ? "" : ((double)Session.Longitude).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    latitudeStr = (Session.LatestLatitude is null) ? "" : ((double)Session.LatestLatitude).ToString(CultureInfo.InvariantCulture);
                    longitudeStr = (Session.LatestLongitude is null) ? "" : ((double)Session.LatestLongitude).ToString(CultureInfo.InvariantCulture);
                }

                LoadResults("action=listsearch&ID=" + Session.ID + "&SessionID=" + Session.SessionID
                    + "&Latitude=" + latitudeStr + "&Longitude=" + longitudeStr + "&ListType=" + Session.ListType
                    + "&SortBy=" + Session.SortBy + "&OrderBy=" + Session.OrderBy + "&SearchTerm=" + CommonMethods.UrlEncode(Session.SearchTerm)
                    + "&SearchIn=" + Session.SearchIn + "&ResultsFrom=" + Session.ResultsFrom);
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine + c.ShowClass<Session>());
            }
        }

        private void LoadResults(string url)
        {
            string responseString = c.MakeRequestSync(url);
            if (responseString.Substring(0, 2) == "OK")
            {
                responseString = responseString.Substring(3);
                int sep1Pos = responseString.IndexOf("|");
                totalResultCount = int.Parse(responseString.Substring(0, sep1Pos));
                responseString = responseString.Substring(sep1Pos + 1);

                Session.LastDataRefresh = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (responseString != "") //result
                {
                    ServerParser<Profile> parser = new ServerParser<Profile>(responseString);

                    if (addResultsAfter)
                    {
                        //c.CW("addResultsAfter viewIndex " + viewIndex + " absoluteFirstIndex " + absoluteFirstIndex);

                        newListProfiles = parser.returnCollection;
                        viewProfiles = new List<Profile>(viewProfiles.Concat(newListProfiles));
                    }
                    else if (addResultsBefore)
                    {
                        //c.CW("addResultsBefore old viewIndex " + viewIndex + " absoluteFirstIndex " + absoluteFirstIndex);

                        newListProfiles = parser.returnCollection;
                        viewProfiles = new List<Profile>(newListProfiles.Concat(viewProfiles));
                        viewIndex += newListProfiles.Count;
                        absoluteFirstIndex -= newListProfiles.Count;

                        //c.CW("addResultsBefore new viewIndex " + viewIndex + " absoluteFirstIndex " + absoluteFirstIndex);
                    }
                    else
                    {
                        //viewProfiles should not be set here, because if the click the first/last profile, background loading will start for the previous/next range, so when we go back and click another profile, a profile from the new range will be loaded.
                        listProfiles = parser.returnCollection;
                        adapter = new UserSearchListAdapter(this, 3, 2f); //autologin completion may precede ViewWillAppear, where adapter is iniitalized
                        adapter.items = listProfiles;
                        newListProfiles = null;

                        //c.CW("normal list absoluteFirstIndex " + absoluteFirstIndex);

                        InvokeOnMainThread(() =>
                        {
                            NoResult.Hidden = true;

                            UserSearchList.DataSource = adapter;
                            UserSearchList.ReloadData();
                            UserSearchList.LayoutIfNeeded();
                        });
                    }
                }
                else if (!(addResultsAfter || addResultsBefore)) //no result; we can get empty list when unhiding profiles 
                {
                    listProfiles = new List<Profile>();
                    viewProfiles = null;
                    adapter = new UserSearchListAdapter(this, 3, 2f);
                    adapter.items = listProfiles;
                    InvokeOnMainThread(() =>
                    {
                        UserSearchList.DataSource = adapter;
                        UserSearchList.ReloadData();
                        UserSearchList.LayoutIfNeeded();
                    });
                }

                usersLoaded = true;
                listLoading = false;

                InvokeOnMainThread(() =>
                {
                    SetResultStatus();
                });

                mapSet = false;
                if (mapLoaded && ((bool)Settings.IsMapView || mapToSet) && !(addResultsBefore || addResultsAfter))
                {
                    SetMap();
                }
                else if (((bool)Settings.IsMapView || mapToSet) && !(addResultsBefore || addResultsAfter)) //in case ViewDidLayoutSubviews is not called yet
                {
                    mapToSet = true;
                }
                else if (!(bool)Settings.IsMapView && !mapToSet)
                {
                    InvokeOnMainThread(() => {
                        SetResultStatus();
                        StopLoaderAnim();
                    });
                }

                // else map is not loaded yet
            }
            else
            {
                InvokeOnMainThread(() => {
                    LoadPrevious.Enabled = true;
                    LoadPrevious.Alpha = 1;
                    LoadNext.Enabled = true;
                    LoadNext.Alpha = 1;
                    listLoading = false;
                    StopLoaderAnim();
                    SetResultStatus();
                    c.ReportError(responseString);
                });
            }
            addResultsBefore = false;
            addResultsAfter = false;
        }

        private void SetMap()
        {
            mapToSet = false;

            if (mapSet) //for switching between map and list view
            {
                return;
            }

            c.Log("Setting map");

            if ((Session.UseLocation is null || !(bool)Session.UseLocation) && !((bool)Session.GeoFilter && (bool)Session.GeoSourceOther))
            {
                InvokeOnMainThread(() =>
                {
                    c.Snack(LangEnglish.LocationNotInUse);
                    StopLoaderAnim();
                });
                return;
            }

            InvokeOnMainThread(() =>
            {
                ResultSet.Text = LangEnglish.SettingMap;
                ResultSet.Hidden = false;
                mapSetting = true;
            });

            bool result;

            if ((bool)Session.GeoFilter && (bool)Session.GeoSourceOther)
            {
                result = MoveMap(Session.OtherLatitude, Session.OtherLongitude);
            }
            else
            {
                if (!Constants.SafeLocationMode)
                {
                    result = MoveMap(Session.Latitude, Session.Longitude);
                }
                else
                {
                    result = MoveMap(Session.LatestLatitude, Session.LatestLongitude);
                }
            }
            if (!result)
            {
                InvokeOnMainThread(() =>
                {
                    c.Snack(LangEnglish.NoLocationSet);
                    mapSetting = false;
                    SetResultStatus();
                    StopLoaderAnim();
                });
                return;
            }

            InvokeOnMainThread(() => {
                MapView.SetBackgroundImage(UIImage.FromBundle("IcMapPressed"), UIControlState.Normal);
                ListView.SetBackgroundImage(UIImage.FromBundle("IcListall"), UIControlState.Normal);

                UserSearchList.Hidden = true;
                ListViewMap.Hidden = false;
                MapStreet.Hidden = false;
                MapSatellite.Hidden = false;
                Settings.IsMapView = true;
                mapSet = true;
            });

            profileAnnotations = new List<ProfileAnnotation>();
            foreach (Profile profile in listProfiles)
            {
                if (profile.Latitude != null && profile.Longitude != null && profile.LocationTime != null) //location available
                {
                    ProfileAnnotation annotation = new ProfileAnnotation(profile.ID, profile.Pictures[0], new CLLocationCoordinate2D((double)profile.Latitude, (double)profile.Longitude));
                    InvokeOnMainThread(() =>
                    {
                        ListViewMap.AddAnnotation(annotation);
                    });
                    profileAnnotations.Add(annotation);
                }
            }

            InvokeOnMainThread(() =>
            {
                mapSetting = false;
                SetResultStatus();
                StopLoaderAnim();
            });
        }

        public bool MoveMap(double? latitude, double? longitude)
        {
            if (!(latitude is null) && !(longitude is null))
            {
                InvokeOnMainThread(() => {

                    if (!(profileAnnotations is null))
                    {
                        ListViewMap.RemoveAnnotations(profileAnnotations.ToArray());
                    }

                    if (!(circlePin is null))
                    {
                        ListViewMap.RemoveAnnotation(circlePin);
                    }
                    if (!(circle is null))
                    {
                        ListViewMap.RemoveOverlay(circle);
                    }

                    if (recenterMap)
                    {
                        if (c.IsLocationEnabled())
                        {
                            ListViewMap.ShowsUserLocation = true;
                        }
                        else
                        {
                            ListViewMap.ShowsUserLocation = false;
                        }

                        CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D((double)latitude, (double)longitude);
                        MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, (int)Session.DistanceLimit * 1000 * 2, (int)Session.DistanceLimit * 1000 * 2);
                        ListViewMap.CenterCoordinate = mapCenter;
                        ListViewMap.Region = mapRegion;
                        c.CW("Region: " + (int)Session.DistanceLimit * 1000 * 2 + " LatitudeDelta " + ListViewMap.Region.Span.LatitudeDelta + " LongitudeDelta " + ListViewMap.Region.Span.LongitudeDelta);
                        //20000 0.20912796568598 0.317953289884542
                        //20000 0.179636395490881 0.566293030628998

                        if ((bool)Session.GeoFilter && Session.LastSearchType == Constants.SearchType_Filter) //no geo filter on free text search
                        {
                            if ((bool)Session.GeoSourceOther) //a pin is added automatically for user location
                            {
                                circlePin = new MKPointAnnotation() { Title = "Center", Coordinate = mapCenter };
                                ListViewMap.AddAnnotation(circlePin);
                            }
                            circle = MKCircle.Circle(mapCenter, (int)Session.DistanceLimit * 1000);
                            ListViewMap.AddOverlay(circle);
                        }
                    }
                    else //change only circle radius
                    {
                        var center = new CLLocationCoordinate2D((double)latitude, (double)longitude);
                        circlePin = new MKPointAnnotation() { Title = "Center", Coordinate = center };
                        ListViewMap.AddAnnotation(circlePin);
                        circle = MKCircle.Circle(center, (int)Session.DistanceLimit * 1000);
                        ListViewMap.AddOverlay(circle);
                    }
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetResultStatus()
        {
            if (listLoading || mapSetting)
            {
                return;
            }

            if (totalResultCount is null)
            {
                NoResult.Hidden = true;
                ResultSet.Hidden = true;
                LoadPrevious.Hidden = true;
                LoadNext.Hidden = true;
                return;
            }

            if (totalResultCount == 0)
            {
                if ((bool)Settings.IsMapView)
                {
                    NoResult.Hidden = true;
                }
                else
                {
                    NoResult.Hidden = false;
                }
                ResultSet.Hidden = true;
                LoadPrevious.Hidden = true;
                LoadNext.Hidden = true;

                return;
            }

            if (totalResultCount > 1)
            {
                ResultSet.Text = LangEnglish.ResultsCount.Replace("[num]", totalResultCount.ToString());
                ResultSet.Hidden = false;
            }
            else
            {
                ResultSet.Text = LangEnglish.ResultCount;
                ResultSet.Hidden = false;
            }

            if (totalResultCount > Constants.MaxResultCount)
            {
                ResultSet.Text = LangEnglish.ResultSetInterval.Replace("[start]", Session.ResultsFrom.ToString())
            .Replace("[end]", (Session.ResultsFrom + listProfiles.Count - 1).ToString()) + " " + ResultSet.Text;
            }

            if (totalResultCount > Session.ResultsFrom - 1 + Constants.MaxResultCount)
            {
                LoadNext.Hidden = false;
                LoadNext.Enabled = true;
                LoadNext.Alpha = 1;
            }
            else
            {
                LoadNext.Hidden = true;
            }

            if (Session.ResultsFrom - 1 > 0)
            {
                LoadPrevious.Hidden = false;
                LoadPrevious.Enabled = true;
                LoadPrevious.Alpha = 1;
            }
            else
            {
                LoadPrevious.Hidden = true;
            }
        }

        private void StartLoaderAnim()
        {
            /*if (animPulldown)
            {
                return;
            }*/
            RefreshDistance.Enabled = false;
            LoaderCircle.Hidden = false;

            CABasicAnimation rotationAnimation = CABasicAnimation.FromKeyPath("transform.rotation");
            rotationAnimation.To = NSNumber.FromDouble(Math.PI * 2);
            rotationAnimation.RepeatCount = int.MaxValue;
            rotationAnimation.Duration = Constants.loaderAnimTime;
            LoaderCircle.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
            RefreshDistance.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
            //ReloadPulldown.Layer.AddAnimation(rotationAnimation, "rotationAnimation");

            //animPulldown = true;
        }

        private void StopLoaderAnim()
        {
            if (refresh.Refreshing)
            {
                refresh.EndRefreshing();
            }
            RefreshDistance.Enabled = true;
            LoaderCircle.Hidden = true;

            RefreshDistance.Layer.RemoveAllAnimations(); 
            LoaderCircle.Layer.RemoveAllAnimations();
            //ReloadPulldown.Layer.RemoveAllAnimations(); //should keep the rotation value
            //animPulldown = false;
            //HidePulldown();
        }

        private void MenuChatList_Touch(object sender, EventArgs e)
        {
            c.AnimateRipple(RippleMain, 3);
        }

        private void MenuChatList_Click(object sender, EventArgs e)
        {
            CommonMethods.OpenPage("ChatListActivity", 2);
        }

        /*
        private void HidePulldown()
        {
            UIView.Animate(duration: Constants.tweenTime, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
            {
                ReloadPulldown.Alpha = 0;
            }, completion: () => { });
        }*/

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
 