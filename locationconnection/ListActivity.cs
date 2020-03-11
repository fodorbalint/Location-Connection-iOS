/*
background location battery drain

interval 5 min, accuracy 10m
12:32 99%
14:17 97%
20:36 91%
22:50 89%
23:59 88% (last update 23:40 - did it just stop, or I logged in on another device?)

not running
8:51 87%
13:02 86%
17:50 85%
00:00 84%

interval 5 min, accuracy 1000m
(right off charger)
4:32 100%

Medium importance

Dark mode support
When snackbar is disappearing, a new snack will not be visible for long.
Once pin and current location markers appeared on iphone at the same time
Overlapping pictures on map may flicker
Disable in-app purchases from provisioning profile (must not be shown in App Store)

Not important / can't solve

Background notifications do not work
Implement match not found error message on server
Restart backgroud location on reboot, worth it?
null error sometimes in locationmanager and customannotationview, error in truncatelocationlog, when no location was acquired in 24 hours, and in UserSearchList. (caught)
ProfileView: Edit update successful snack does not animate when disappearing, same with in-app notifications
Does backgroud location always work even if permission is only authorizedWhenInUse? Otherwise permission need to be requested twice. / Dialog appearing every 3 days about background location being used when permission is AuthorizedAlways. (Feb 22 11:00, Feb 26. 23:03)
Native crash when pressing Cancel from RegisterActivity into ListActivity. Has something to do with SetCollectionViewLayout, seems to only occur in simulator.

Android:
Important: chat get updated from other person's conversation
gray map circle line instead of black
remove profiles when map circle radius changes.
do we need paddigTop, paddingBottom on ImagesProgressText
ChatList: second line showing in an entry when the line is long (screenshot taken.
remove date when showing log
Profile edit: rename done button to save
Profile edit: womentext and mentext have to be vertically centered to their switches
Profile edit: more space above save button
PE: write "press and hold an image to rearrange"
ImageFrameLayout: LargeImageSize folder is used, but small can do.
Align map buttons and separator relative to map in Location History
Edit back button requires smaller ripple
Help center: change separator constaints to align with scrollview
Help center / settings: remove focus from messageedit when it is hidden.
Location changed to Uselocation in RegisterActivity. ResetPassword on MainActivity also needs to be changed.
Is string LocationRationale used?
can profileview load the list in background, because ListActivity.active = false?
disable like button when it is liked.
Chat one: when textfield expands, scroll position doesn't remain at the bottom.
Chat one: limit number of lines for input?
ChatOne line 334: necessary to add event handler again? (or event handler should not be added at start)
ChatOne: make chat window using table for memory concerns
Login failed snack remains in ListActivity after logging in
deleted profileview line 916 917 addcircles
ProfileView: right margin to EditSelf button
ProfileView: delete strokealpha from ic_hide
rename ic_refresh to reinstate (and only one tag is necessary with two closed paths)
ProfileView: ic_liked, ic_chat_one: remove unnecessary round background
ProfileView: change separator constraints to ProfileImageContainer, same with map
ProfileView: profileview xml line 75 color value used, not style.
ProfileView: no space between name and stats when name is long?
ProfileView: line 27 background necessary?
RegisterActivity: more spacing below No one
RegisterActivity: strings: replace Done with Register
imageframelayout line 250 changed.
change settingsdefault.hostname to constants
GetScreenMetrics() at start of RegisterActivity needed, in case the device was rotated before.
RegisterActivity: inserted RegisterActivity line 536 requestfocus
RegisterCommonMethods: Rewrite line 226 to use islocationenabled()
Help center form send button: wrap content instead of 100 px width
Snack on ListActivity if settings cannot be saved by network error.
imageDeleting variable in ImageFrameLayout.cs
Reset_Click changed, condition added.
inernal server error on 28 feb from Android
Location History: draw most recent lines on top
handle autohorization errors in Reporterrorsilent
Location History: remove circle as part of addcircle
animate filters expanding or collapsing
new logo
chat one with list
ProfileView editself: small ripple
when enabling location, profileview map does not show immediately (Does "Location x seconds ago" appear?)
disable background location rate slider when background location is off
remove onlocationresult logs
token file can be declared in baseactivity
not initialized error results in crash on chatlistactivity
unmatching notificaion in chatOne does not hide Start location updates menu. And it shows "Show" button unnecessarily
update profileview refreshtimer_elapsed with null checking on displayuser
In chat one, match notification, show button only if we are in another chat.
Null checking AddUpdateMatch in ChatReceiver
Show Session.SnackMessage in BaseActivity
Nowms function not needed
Rewrite chatReceiver, senderID-s needed in messages?
ChatOne: message that user is passive when clicking on header, if user was passive to start with.
Eliminate updating layout every second in Profile View
make selector for android 6 to use png images instead of vector.
UpdateLocationSetting : MapViewSecond necessary? OnResume will be called anyway which sets the map. Now first time we have no location yet. Test logged in user enabling location by clicking on map.
line 1229 correct Settings.GeoSourceOther to Session.GeoSourceOther, otherwise the list loading will use the set coordinates instead of the current one.
mapSet always have to be set to false in LoadResults (otherwise a logged in user who previously saw the map, but now uses current location will see the old map when swithing back)
DistanceSource_Click: list is not loading after UpdateLocationSetting(); (user turned off location, and clicked on other address, but now wants to use own location again.) If OnResume is called, is location acquired?
Session.LocationTime = null; only has to be set after PM granted, not in UpdateLocationSetting (for the above example.)
DistanceSource_Click on current location when uselocation is off (but pm on) should start location updates
disable all buttons that performs a network request when clicked
Remove onlocationresult logs
Link to source code in About alert

Admin:
selected cell gets edited by pressing esc after exiting editing mode
data too long for column error when editing boolean field
editable table headers

----------

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
    <color key="backgroundColor" colorSpace="custom" customColorSpace="genericGamma22GrayColorSpace" white="1" alpha="1"/> ----- Use desired color -----
</view>
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
<outlet property="SnackTopConstraint" destination="Snackbar12" id="name-outlet-Snackbar12"/> ----- For keyboard window resizing -----
<outlet property="SnackBottomConstraint" destination="Snackbar13" id="name-outlet-Snackbar13"/>
<outlet property="ScrollBottomConstraint" destination="Register4" id="name-outlet-Register4"/> ----- For pages with ScrollView ----

RoundBottom_Base = RoundBottom;
Snackbar_Base = Snackbar;
BottomConstraint_Base = BottomConstraint;
SnackTopConstraint_Base = SnackTopConstraint; ----- For keyboard window resizing -----
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
        private string loginSessionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "loginsession.txt");

        private static bool backgroundNotificationsSet;        
        public static ListActivity thisInstance;
        private bool autoLogin;

        private bool filtersOpen;
        private bool searchOpen;
        private bool distanceFiltersOpen;

        private bool listLoading;
        private bool mapSetting;
        private bool mapToSet;
        private bool mapSet;
        public static nint? totalResultCount;
        UserSearchListAdapter adapter;
        GridLayout gridLayout;
        private bool usersLoaded;
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
        private static int absoluteFirstIndex; //absolute position of first element in the list. Changes as list is expanded backwards.

        //if we get location before logging in is completed, we do not need to request it again. 
        private double? localLatitude;
        private double? localLongitude;
        private long? localLocationTime;

        //pull down refresh icon
        /*private nfloat? startY;
        private nfloat loaderHeight = 50;
        private nfloat maxY = 186;
        private nfloat diff;*/
        private float loaderAnimTime = 1.3f;
        //private bool animPulldown;

        private bool animRunning; //for menu

        private UIRefreshControl refresh;

        private bool distanceLimitChangedByCode;
        private bool distanceSourceAddressTextChanging;
        private Timer ProgressTimer;

        Stopwatch stw;

        public ListActivity(IntPtr handle) : base(handle)
        {
            stw = new Stopwatch();
            stw.Start();
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                if (!backgroundNotificationsSet)
                {
                    UIApplication.Notifications.ObserveDidEnterBackground((sender, args) => {

                        c.CW("Entered background");
                        c.LogActivity("Entered background");

                        isAppForeground = false;

                        if (!string.IsNullOrEmpty(locationUpdatesTo))
                        {
                            EndLocationShare();
                        }
                        locationUpdatesTo = null; //stop real-time location updates when app goes to background
                        locationUpdatesFrom = null;

                        if ((!c.IsLoggedIn() || !(bool)Session.UseLocation || !(bool)Session.BackgroundLocation || !c.IsLocationEnabled()) && !(locMgr is null))
                        {
                            locMgr.StopLocationUpdates();
                        }
                    });

                    UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => {
                        c.CW("Entered foreground");
                        c.LogActivity("Entered foreground");

                        isAppForeground = true;

                        if (Session.UseLocation == true && c.IsLocationEnabled() && !(locMgr is null)) //user might have logged out, and Session is nulled.
                        {
                            locMgr.StartLocationUpdates();
                        }
                    });

                    backgroundNotificationsSet = true;
                }
                
                //c.CW("Stopwatch " + stw.ElapsedMilliseconds + " ViewDidLoad");

                thisInstance = this;
                c.LoadSettings(false); //overwrites DisplaySize

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

                if (File.Exists(c.errorFile))
                {
                    string url = "action=reporterror&ID=" + Session.ID + "&SessionID=" + Session.SessionID;
                    string content = "Content=" + c.UrlEncode(File.ReadAllText(c.errorFile) + System.Environment.NewLine
                        + "IOS version: " + "-----" + " " + "-----" + " " + System.Environment.NewLine + "-----" + System.Environment.NewLine + File.ReadAllText(CommonMethods.logFile));
                    string responseString = c.MakeRequestSync(url, "POST", content);
                    if (responseString == "OK")
                    {
                        File.Delete(c.errorFile);
                    }
                }

                //IsPlayServicesAvailable();
                //CreateNotificationChannel();

                if (!c.IsLoggedIn() && File.Exists(loginSessionFile))
                {
                    autoLogin = true;
                }
                else
                {
                    autoLogin = false;
                }

                if (autoLogin)
                {
                    c.LogActivity("Autologin");
                    Task.Run(async () =>
                    {
                        Session.LastDataRefresh = null;
                        Session.LocationTime = null;

                        string str = File.ReadAllText(loginSessionFile);
                        string[] strarr = str.Split(";");

                        string url = "action=loginsession&ID=" + strarr[0] + "&SessionID=" + strarr[1];

                        if (File.Exists(deviceTokenFile))
                        {
                            if (bool.Parse(File.ReadAllText(tokenUptoDateFile)) == false)
                            {
                                url += "&token=" + File.ReadAllText(deviceTokenFile) + "&ios=1";
                            }
                        }

                        string responseString = c.MakeRequestSync(url);
                        if (responseString.Substring(0, 2) == "OK")
                        {
                            if (File.Exists(deviceTokenFile))
                            {
                                File.WriteAllText(tokenUptoDateFile, "True");
                            }
                            c.LoadCurrentUser(responseString);

                            InvokeOnMainThread(() => {

                                SetLoggedInMenu();
                                LoggedInLayout();
                                SetViews();

                                c.CW("Autologin logged in views set");
                                c.LogActivity("Autologin logged in views set");
                            });

                            CheckIntent();

                            c.CW("Autologin uselocation " + Session.UseLocation + " enabled " + c.IsLocationEnabled());
                            c.LogActivity("Autologin uselocation " + Session.UseLocation + " enabled " + c.IsLocationEnabled());


                            if ((bool)Session.UseLocation)
                            {
                                if (c.IsLocationEnabled())
                                {
                                    if ((bool)Session.BackgroundLocation)
                                    {
                                        locMgr.RestartLocationUpdates();
                                    }

                                    if (!(localLatitude is null) && !(localLongitude is null) && !(localLocationTime is null)) //this has to be more recent than the loaded data
                                    {
                                        c.CW("Autologin updating location");
                                        c.LogActivity("Autologin updating location");

                                        if (Session.LocationTime is null || c.Now() - Session.LocationTime > Session.InAppLocationRate) //a location update could have happened just after login. 12 ms between views set and this point.
                                        {
                                            Session.Latitude = localLatitude;
                                            Session.Longitude = localLongitude;
                                            Session.LocationTime = localLocationTime;

                                            await c.UpdateLocationSync();
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
                                        //first location update will load the list, otherwise the Getting location... message will remain in the status bar.
                                        c.CW("Autologin location unavailable");
                                        c.LogActivity("Autologin location unavailable");
                                    }
                                }
                                else
                                {
                                    c.CW("Autologin LocationDisabledButUsingLocation");
                                    c.LogActivity("Autologin LocationDisabledButUsingLocation");

                                    InvokeOnMainThread(() =>
                                    {
                                        c.SnackIndef(LangEnglish.LocationDisabledButUsingLocation);
                                    });

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
                                c.CW("Autologin logged in, not using location");
                                c.LogActivity("Autologin logged in, not using location");

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
                        else if (responseString.Substring(0, 6) == "ERROR_")
                        {
                            InvokeOnMainThread(() =>
                            {
                                try
                                {
                                    LoggedOutLayout();
                                    if (!(RefreshDistance is null) && !(ReloadPulldown is null) && !(LoaderCircle is null))
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
                                        File.Delete(loginSessionFile);
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
                                    if (!(RefreshDistance is null) && !(ReloadPulldown is null) && !(LoaderCircle is null))
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
                        //InitLocationUpdates();

                        //----- start loader circle and statustext
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
                ReloadPulldown.Alpha = 0;

                MenuLogOut.SetTitle(LangEnglish.MenuLogOut, UIControlState.Normal);
                MenuLogIn.SetTitle(LangEnglish.MenuLogIn, UIControlState.Normal);
                MenuRegister.SetTitle(LangEnglish.MenuRegister, UIControlState.Normal);
                MenuSettings.SetTitle(LangEnglish.MenuSettings, UIControlState.Normal);
                MenuLocation.SetTitle(LangEnglish.MenuLocation, UIControlState.Normal);
                MenuHelpCenter.SetTitle(LangEnglish.MenuHelpCenter, UIControlState.Normal);
                MenuAbout.SetTitle(LangEnglish.MenuAbout, UIControlState.Normal);

                HideMenu(false);

                MapStreet.SetTitle(LangEnglish.MapStreet, UIControlState.Normal);
                MapSatellite.SetTitle(LangEnglish.MapSatellite, UIControlState.Normal);
                MapStreet.Hidden = true;
                MapSatellite.Hidden = true;
                ListViewMap.MapType = (MKMapType)Settings.ListMapType;
                RippleMain.Alpha = 0;

                usersLoaded = false;
                mapToSet = false;
                listLoading = false;

                filtersOpen = true;
                searchOpen = true;
                distanceFiltersOpen = true;

                UITextField.Notifications.ObserveTextFieldTextDidChange(ChangesDetected);

                StatusImage.TouchUpInside += StatusImage_Click;
                OpenFilters.TouchUpInside += OpenFilters_Click;
                OpenSearch.TouchUpInside += OpenSearch_Click;
                ListView.TouchUpInside += ListView_Click;
                MapView.TouchUpInside += MapView_Click;
                MenuIcon.TouchUpInside += MenuIcon_Click;
                MenuLayer.TouchDown += MenuLayer_TouchDown;

                MenuLogOut.TouchUpInside += MenuLogOut_Click;
                MenuLogIn.TouchUpInside += MenuLogIn_Click;
                MenuRegister.TouchUpInside += MenuRegister_Click;
                MenuSettings.TouchUpInside += MenuSettings_Click;
                MenuLocation.TouchUpInside += MenuLocation_Click;
                MenuHelpCenter.TouchUpInside += MenuHelpCenter_Click;
                MenuAbout.TouchUpInside += MenuAbout_Click;

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
                UserSearchListScroll.RefreshControl = refresh;

                ListViewMap.Delegate = new CustomAnnotationView(this);
                MapStreet.TouchUpInside += MapStreet_Click;
                MapSatellite.TouchUpInside += MapSatellite_Click;

                LoadPrevious.TouchUpInside += LoadPrevious_Click;
                LoadNext.TouchUpInside += LoadNext_Click;

                MenuChatList.TouchUpInside += MenuChatList_Click;
                MenuChatList.TouchDown += MenuChatList_Touch;

                RoundBottom_Base = RoundBottom;
                Snackbar_Base = Snackbar;
                BottomConstraint_Base = BottomConstraint;
                SnackBottomConstraint_Base = SnackBottomConstraint;
                SnackTopConstraint_Base = SnackTopConstraint;
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

                //c.CW("Stopwatch " + stw.ElapsedMilliseconds + " ViewWillAppear");

                

                if (File.Exists(c.locationLogFile))
                {
                    TruncateLocationLog();
                }
                TruncateSystemLog();

                c.CW("ViewWillAppear logged in: " + c.IsLoggedIn());
                c.LogActivity("ViewWillAppear logged in: " + c.IsLoggedIn());

                if (c.IsLoggedIn())
                {
                    LoggedInLayout();
                    SetLoggedInMenu();
                }
                else
                {
                    LoggedOutLayout();
                    SetLoggedOutMenu();
                }

                SetViews();

                if (!(listProfiles is null) && !(newListProfiles is null))
                {
                    if (absoluteIndex < absoluteStartIndex)
                    {
                        do
                        {
                            absoluteStartIndex -= Constants.MaxResultCount;
                        } while (absoluteStartIndex > absoluteIndex);
                        //c.LogActivity("onresume got low range: absoluteStartIndex " + absoluteStartIndex + " absoluteFirstIndex " + absoluteFirstIndex + " index " + (absoluteStartIndex - absoluteFirstIndex));
                        //c.CW("onresume got low range: absoluteStartIndex " + absoluteStartIndex + " absoluteFirstIndex " + absoluteFirstIndex + " index " + (absoluteStartIndex - absoluteFirstIndex));
                        listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, Constants.MaxResultCount);
                        absoluteFirstIndex = absoluteStartIndex;
                        viewProfiles = listProfiles;
                    }
                    else if (absoluteIndex >= absoluteStartIndex + listProfiles.Count)
                    {
                        do
                        {
                            absoluteStartIndex += Constants.MaxResultCount;
                        } while (absoluteStartIndex <= absoluteIndex);
                        absoluteStartIndex -= Constants.MaxResultCount;

                        //c.LogActivity("onresume got high range: absoluteStartIndex " + absoluteStartIndex + " absoluteFirstIndex " + absoluteFirstIndex + " index " + (absoluteStartIndex - absoluteFirstIndex));
                        //c.CW("onresume got high range: absoluteStartIndex " + absoluteStartIndex + " absoluteFirstIndex " + absoluteFirstIndex + " index " + (absoluteStartIndex - absoluteFirstIndex));
                        if (absoluteStartIndex - absoluteFirstIndex + Constants.MaxResultCount - 1 > viewProfiles.Count - 1)
                        {
                            listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, viewProfiles.Count + absoluteFirstIndex - absoluteStartIndex);
                        }
                        else
                        {
                            listProfiles = viewProfiles.GetRange(absoluteStartIndex - absoluteFirstIndex, Constants.MaxResultCount);
                        }
                        absoluteFirstIndex = absoluteStartIndex;
                        viewProfiles = listProfiles;
                    }
                    Session.ResultsFrom = absoluteStartIndex + 1;
                }
                newListProfiles = null;

                GetScreenMetrics();
                gridLayout = new GridLayout(3, 2f, DpWidth);
                UserSearchList.SetCollectionViewLayout(gridLayout, false);
                adapter = new UserSearchListAdapter(this, 3, 2f, DpWidth);
                UserSearchList.DataSource = adapter;

                if (!(listProfiles is null))
                {
                    adapter.items = listProfiles;

                    UserSearchList.ReloadData();
                    UserSearchList.LayoutIfNeeded();
                    c.SetHeight(UserSearchList, UserSearchList.ContentSize.Height);
                    /*Task.Run(() => {
                        LoadImages();
                    });*/
                }
                else
                {
                    adapter.items = new List<Profile>();
                }
                usersLoaded = true;

                c.CW("Location permission status: " + CLLocationManager.Status);
                c.LogActivity("Location permission status: " + CLLocationManager.Status);

                if (c.IsLoggedIn())
                {
                    if ((bool)Session.UseLocation && !c.IsLocationEnabled())
                    {
                        c.CW("ViewWillAppear LocationDisabledButUsingLocation " + Session.UseLocation + " " + c.IsLocationEnabled());
                        c.LogActivity("ViewWillAppear LocationDisabledButUsingLocation " + Session.UseLocation + " " + c.IsLocationEnabled());
                        c.SnackIndef(LangEnglish.LocationDisabledButUsingLocation);
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

                long unixTimestamp = c.Now();

                if ((bool)Session.UseLocation && c.IsLocationEnabled() && locMgr is null)
                {
                    isAppForeground = true;

                    firstLocationAcquired = false;

                    ResultSet.Hidden = false;
                    ResultSet.Text = LangEnglish.GettingLocation;

                    locMgr = new LocationManager(this);
                    locMgr.StartLocationUpdates(); //when autologin, background location is not yet enabled at this point
                }
                else //no location or alread started, loading list
                {
                    c.CW("ViewWillAppear no location or updates started: " + locationUpdating);
                    c.LogActivity("ViewWillAppear no location or updates started: " + locationUpdating);
                    LoadListStartup();
                }

                if ((!(bool)Session.UseLocation || !c.IsLocationEnabled()) && !((bool)Session.GeoFilter && (bool)Session.GeoSourceOther)) {
                    if ((bool)Settings.IsMapView)
                    {
                        ListView_Click(null, null);
                    }
                }

                c.CW("ViewWillAppear end");
                c.LogActivity("ViewWillAppear end");
            }
            catch (Exception ex)
            {
                c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void LoadListStartup () {

            long unixTimestamp = c.Now();

            if (autoLogin)
            {
                c.CW("LoadListStartup autologin, locationUpdating " + locationUpdating + ", locationtime: " + Session.LocationTime);
                c.LogActivity("LoadListStartup autologin, locationUpdating " + locationUpdating + ", locationtime: " + Session.LocationTime);

                localLatitude = Session.Latitude;
                localLongitude = Session.Longitude;
                localLocationTime = Session.LocationTime;
            }
            else
            {
                c.CW("LoadListStartup not autologin, logged in: " + c.IsLoggedIn() + ", locationtime: " + Session.LocationTime + " Settings.IsMapView " + Settings.IsMapView + " mapToSet " + mapToSet);
                c.LogActivity("LoadListStartup not autologin, logged in: " + c.IsLoggedIn() + ", locationtime: " + Session.LocationTime + " Settings.IsMapView " + Settings.IsMapView + " mapToSet " + mapToSet);

                if (Session.LastDataRefresh is null || Session.LastDataRefresh < unixTimestamp - Constants.DataRefreshInterval)
                {
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
                else //show no result label only if list is not being reloaded, and set map with the results loaded while being in ProfileView / Set map after location authorization
                {
                    if ((bool)Settings.IsMapView || mapToSet)
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

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            c.SetShadow(MenuContainer, 0, 0, 10); //clipsToBounds / clipsSubviews and Layer.MaskToBound are exclusive. The outer view has the shadow, while the inner view of the same size clips the content.          

            c.SetRoundShadow(MapStreet, 1, 1, 1, 2, true);
            c.SetRoundShadow(MapSatellite, 1, 1, 1, 2, false);            

            MapStreet.Layer.CornerRadius = 2;
            MapStreet.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MinXMaxYCorner;
            MapSatellite.Layer.CornerRadius = 2;
            MapSatellite.Layer.MaskedCorners = CACornerMask.MaxXMinYCorner | CACornerMask.MaxXMaxYCorner;            
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
            c.CW("ListActivity ViewWillDisappear SaveSettings");
            c.LogActivity("ListActivity ViewWillDisappear SaveSettings");
            c.SaveSettings();
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            c.SetHeight(UserSearchList, toSize.Width / UserSearchList.Frame.Width * UserSearchList.Frame.Height);
            adapter.UpdateItemSize(toSize.Width);
            gridLayout.UpdateCellSize(toSize.Width);

            foreach (UIView view in UserSearchList.Subviews)
            {
                if (view is UICollectionViewCell)
                {
                    UIImageView image = (UIImageView)view.Subviews[0].Subviews[0];
                    image.Frame = new CGRect(new CGPoint(0, 0), new CGSize(gridLayout.ItemSize.Width, gridLayout.ItemSize.Width));
                }
            }

            base.ViewWillTransitionToSize(toSize, coordinator);
        }

        private void LoggedInLayout()
        {
            StatusImage.Hidden = false;
            StatusText.Hidden = true;
            string url;
            url = Constants.HostName + Constants.UploadFolder + "/" + Session.ID + "/" + Constants.SmallImageSize + "/" + Session.Pictures[0];

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
                ListView.SetBackgroundImage(UIImage.FromBundle("ic_listall_pressed.png"), UIControlState.Normal);
                MapView.SetBackgroundImage(UIImage.FromBundle("ic_map.png"), UIControlState.Normal);
                Settings.IsMapView = false;
            }
            else
            {
                UserSearchList.Hidden = true;
                ListViewMap.Hidden = false;
                MapStreet.Hidden = false;
                MapSatellite.Hidden = false;
                ListView.SetBackgroundImage(UIImage.FromBundle("ic_listall.png"), UIControlState.Normal);
                MapView.SetBackgroundImage(UIImage.FromBundle("ic_map_pressed.png"), UIControlState.Normal);
            }

            if (Settings.ListMapType == (int)MKMapType.Standard)
            {
                MapStreet.BackgroundColor = UIColor.FromRGBA(204, 204, 204, 184);
                MapSatellite.BackgroundColor = UIColor.FromRGBA(255, 255, 255, 184);
            }
            else
            {
                MapStreet.BackgroundColor = UIColor.FromRGBA(255, 255, 255, 184); 
                MapSatellite.BackgroundColor = UIColor.FromRGBA(204, 204, 204, 184);
            }

            if (!(bool)Settings.SearchOpen)
            {
                c.CollapseY(SearchLayout);
                searchOpen = false;
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("ic_search.png"), UIControlState.Normal);
            }
            else
            {
                //if (!searchInShown)
                //{
                Session.LastSearchType = Constants.SearchType_Search;
                c.ExpandY(SearchLayout);
                searchOpen = true;
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("ic_search_pressed.png"), UIControlState.Normal);
                    //searchInClicked++;
                    //searchInShown = true;
                //}
            }

            SearchTerm.Text = Session.SearchTerm;

            DropDownList entries = new DropDownList(LangEnglish.SearchInEntries, "SearchIn", 100, this);
            SearchIn.Model = entries;

            int index = LangEnglish.SearchInEntries_values.ToList().IndexOf(Session.SearchIn);
            SearchIn.Select(index, 0, false);

            if (!(bool)Settings.FiltersOpen)
            {
                c.CollapseY(FilterLayout);
                filtersOpen = false;
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("ic_filters.png"), UIControlState.Normal);
            }
            else
            {
                Session.LastSearchType = Constants.SearchType_Filter;
                c.ExpandY(FilterLayout);
                filtersOpen = true;
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("ic_filters_pressed.png"), UIControlState.Normal);
            }

            if (!(bool)Settings.GeoFiltersOpen)
            {
                c.CollapseY(DistanceFilters);
                distanceFiltersOpen = false;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("ic_expand.png"), UIControlState.Normal);
            }
            else
            {
                c.ExpandY(DistanceFilters);
                distanceFiltersOpen = true;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("ic_collapse.png"), UIControlState.Normal);
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
                    SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("ic_lightning36_pressed.png"), UIControlState.Normal);
                    SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("ic_chat_two36.png"), UIControlState.Normal);
                    SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("ic_registered36.png"), UIControlState.Normal);
                    break;
                case "ResponseRate":
                    SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("ic_lightning36.png"), UIControlState.Normal);
                    SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("ic_chat_two36_pressed.png"), UIControlState.Normal);
                    SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("ic_registered36.png"), UIControlState.Normal);
                    break;
                case "RegisterDate":
                    SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("ic_lightning36.png"), UIControlState.Normal);
                    SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("ic_chat_two36.png"), UIControlState.Normal);
                    SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("ic_registered36_pressed.png"), UIControlState.Normal);
                    break;
            }

            if (Session.OrderBy == "desc")
            {
                //TooltipCompat.SetTooltipText(OrderBy, res.GetString(Resource.String.Descending));
                //OrderBy.TooltipText = res.GetString(Resource.String.Descending); //threw an error in Google's test, found out it is not supported proir to API 26.
                OrderBy.SetBackgroundImage(UIImage.FromBundle("ic_descending.png"), UIControlState.Normal);
            }
            else
            {
                //TooltipCompat.SetTooltipText(OrderBy, res.GetString(Resource.String.Ascending));
                //OrderBy.TooltipText = res.GetString(Resource.String.Ascending);
                OrderBy.SetBackgroundImage(UIImage.FromBundle("ic_ascending.png"), UIControlState.Normal);
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
                //distanceSourceAddressTextChanging = true;
                DistanceSourceAddressText.Text = Session.OtherAddress;
                //distanceSourceAddressTextChanging = false;
            }
            else if (!(Session.OtherLatitude is null) && !(Session.OtherLongitude is null))
            {
                //distanceSourceAddressTextChanging = true;
                DistanceSourceAddressText.Text = ((double)Session.OtherLatitude).ToString(CultureInfo.InvariantCulture) + ", " + ((double)Session.OtherLongitude).ToString(CultureInfo.InvariantCulture);
                //distanceSourceAddressTextChanging = false;
            }

            //distanceLimitChangedByCode = true;
            DistanceLimit.Value = (int)Session.DistanceLimit;
            DistanceLimitInput.Text = Session.DistanceLimit.ToString();
            //distanceLimitChangedByCode = false;
        }

        private void StatusImage_Click(object sender, EventArgs e)
        {
            IntentData.profileViewPageType = "self";

            CommonMethods.OpenPage("ProfileViewActivity", 1);
        }

        private void OpenFilters_Click(object sender, EventArgs e)
        {
            if (!filtersOpen)
            {
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("ic_filters_pressed.png"), UIControlState.Normal);
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("ic_search"), UIControlState.Normal);
                c.ExpandY(FilterLayout);
                if (searchOpen)
                {
                    c.CollapseY(SearchLayout);
                    searchOpen = false;
                }
                else
                {
                    UIView.Animate(tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                }
                filtersOpen = true;
                Settings.FiltersOpen = true;
                Settings.SearchOpen = false;
                Session.LastDataRefresh = null;
                if (Session.LastSearchType == Constants.SearchType_Search)
                {
                    Session.ResultsFrom = 1;
                    //recenterMap = true;
                    Task.Run(() => LoadList());
                }
            }
            else
            {
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("ic_filters"), UIControlState.Normal);
                c.CollapseY(FilterLayout);
                UIView.Animate(tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                filtersOpen = false;
                Settings.FiltersOpen = false;
            }
            //FilterLayout.ClipsToBounds = true;
        }

        private void OpenSearch_Click(object sender, EventArgs e)
        {
            if (!searchOpen)
            {
                OpenFilters.SetBackgroundImage(UIImage.FromBundle("ic_filters"), UIControlState.Normal);
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("ic_search_pressed.png"), UIControlState.Normal);
                c.ExpandY(SearchLayout);
                if (filtersOpen)
                {
                    c.CollapseY(FilterLayout);
                    filtersOpen = false;
                }
                else
                {
                    UIView.Animate(tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                }
                
                searchOpen = true;
                Settings.FiltersOpen = false;
                Settings.SearchOpen = true;
                Session.LastDataRefresh = null;
                if (Session.LastSearchType == Constants.SearchType_Filter)
                {
                    Session.ResultsFrom = 1;
                    //recenterMap = true;
                    Task.Run(() => LoadListSearch());
                }

            }
            else
            {
                OpenSearch.SetBackgroundImage(UIImage.FromBundle("ic_search.png"), UIControlState.Normal);
                c.CollapseY(SearchLayout);
                UIView.Animate(tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                searchOpen = false;
                Settings.SearchOpen = false;
            }
        }

        private void ListView_Click(object sender, EventArgs e)
        {
            ListView.SetBackgroundImage(UIImage.FromBundle("ic_listall_pressed.png"),UIControlState.Normal);
            MapView.SetBackgroundImage(UIImage.FromBundle("ic_map.png"), UIControlState.Normal);

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
            c.CW("CheckLocationSettings UseLocation " + Session.UseLocation + " IsLocationEnabled " + c.IsLocationEnabled());
            c.LogActivity("CheckLocationSettings UseLocation " + Session.UseLocation + " IsLocationEnabled " + c.IsLocationEnabled());
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
                            UpdateLocationSetting();

                            firstLocationAcquired = false;

                            ResultSet.Hidden = false;
                            ResultSet.Text = LangEnglish.GettingLocation;

                            if (locMgr is null)
                            {
                                locMgr = new LocationManager(this);
                            }
                            locMgr.StartLocationUpdates(); //will load the list or set the map only
                        }, alert => {
                            mapToSet = false;
                            SetDistanceSourceAddress();
                        });
                    }
                }
                c.CW("CheckLocationSettings return false");
                return false;
            }
            else
            {
                c.CW("CheckLocationSettings return true");
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
            c.CW("LocationManager_AuthorizationChanged " + e.Status + " logged in " + c.IsLoggedIn() +  " distanceSourceCurrentClicked " + distanceSourceCurrentClicked + " mapToSet " + mapToSet);
            c.LogActivity("LocationManager_AuthorizationChanged " + e.Status + " logged in " + c.IsLoggedIn() + " distanceSourceCurrentClicked " + distanceSourceCurrentClicked + " mapToSet " + mapToSet);
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
                        //it resets to address by itself
                        DistanceSourceCurrent.Checked = true;
                        DistanceSourceAddress.Checked = false;
                        Session.GeoSourceOther = false;
                        Session.LastDataRefresh = null;
                        distanceSourceCurrentClicked = false;
                    }
                    Session.LocationTime = null;
                    UpdateLocationSetting();
                }
                else
                {
                    Session.UseLocation = true;
                    Session.LocationTime = null;

                    //OnResume will be called which starts location updates and refreshing the list

                    if (distanceSourceCurrentClicked)
                    {
                        //it resets to address by itself
                        Session.LocationTime = null; //making sure, last location will be requested in OnResume (even if PM was off, a last location value could have existed, if PM was on before.) 
                        DistanceSourceCurrent.Checked = true;
                        DistanceSourceAddress.Checked = false;
                        Session.GeoSourceOther = Settings.GeoSourceOther = false;
                        Session.LastDataRefresh = null;
                        distanceSourceCurrentClicked = false;
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
            else if (e.Status != CLAuthorizationStatus.NotDetermined)
            {
                mapToSet = false;
                Session.UseLocation = false;
                SetDistanceSourceAddress();
                c.Snack(LangEnglish.LocationNotGranted); //in the dialog the user choose to turn on location, but now denied it. Message needs to be shown.
            }
        }

        public void UpdateLocationSetting()
        {
            string url = "action=profileedit&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&UseLocation=True";
            string responseString = c.MakeRequestSync(url);
            if (responseString.Substring(0, 2) == "OK")
            {
                Session.UseLocation = true;
                
                if (distanceSourceCurrentClicked)
                {
                    Session.LastDataRefresh = null;
                    distanceSourceCurrentClicked = false;
                }
            }
            else
            {
                c.ReportError(responseString);
            }
        }

        private void MapViewSecond()
        {
            c.CW("MapViewSecond usersloaded " + usersLoaded + " Settings.IsMapView " + Settings.IsMapView + " mapSet" + mapSet + " mapToSet " + mapToSet);
            c.LogActivity("MapViewSecond usersloaded " + usersLoaded + " Settings.IsMapView " + Settings.IsMapView +" mapSet" + mapSet + " mapToSet " + mapToSet);
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

                    MapView.SetBackgroundImage(UIImage.FromBundle("ic_map_pressed.png"), UIControlState.Normal);
                    ListView.SetBackgroundImage(UIImage.FromBundle("ic_listall.png"), UIControlState.Normal);

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
            ShowMenu();
        }

        private void MenuLayer_TouchDown(object sender, EventArgs e)
        {
            HideMenu(true);
        }

        private void ShowMenu()
        {
            if (!animRunning)
            {
                animRunning = true;

                MenuLayer.Hidden = false;
                c.Expand(MenuContainer);
                UIView.Animate(tweenTime, () => {
                    View.LayoutIfNeeded();
                }, () => {
                    MenuContainer.UserInteractionEnabled = true;
                    animRunning = false;
                });
            }
        }

        private void HideMenu(bool presented)
        {
            MenuContainer.UserInteractionEnabled = false;
            
            if (presented)
            {
                if (!animRunning)
                {
                    animRunning = true;

                    UIView.Animate(tweenTime, () => {
                        MenuContainer.Alpha = 0;
                    }, () => {
                        MenuLayer.Hidden = true;
                        c.Collapse(MenuContainer);
                        MenuContainer.Alpha = 1;
                        animRunning = false;
                    });
                }
            }
            else
            {
                MenuLayer.Hidden = true;
                c.Collapse(MenuContainer);
            }
        }

        private void MenuLogOut_Click(object sender, EventArgs e)
        {
            HideMenu(true);
            IntentData.logout = true;
            CommonMethods.OpenPage("MainActivity", 1);
        }

        private void MenuLogIn_Click(object sender, EventArgs e)
        {
            HideMenu(true);
            CommonMethods.OpenPage("MainActivity", 1);
        }

        private void MenuRegister_Click(object sender, EventArgs e)
        {
            HideMenu(true);
            CommonMethods.OpenPage("RegisterActivity", 1);
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
            HideMenu(true);
            CommonMethods.OpenPage("SettingsActivity", 1);
        }

        private void MenuLocation_Click(object sender, EventArgs e)
        {
            HideMenu(true);
            //c.LogLocationAlert();
            CommonMethods.OpenPage("LocationActivity", 1);
        }

        private void MenuHelpCenter_Click(object sender, EventArgs e)
        {
            HideMenu(true);
            CommonMethods.OpenPage("HelpCenterActivity", 1);
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            HideMenu(true);
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
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadListSearch());
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            c.CW("textFieldShouldReturn " + textField);
            View.EndEditing(true); //calls EditingEnded too
            if (textField == DistanceSourceAddressText)
            {
                GetAddressCoordinates();
            }
            else if (textField == DistanceLimitInput)
            {
                int progress = int.Parse(DistanceLimitInput.Text);
                if (progress > Constants.MaxGoogleMapDistance)
                {
                    return false;
                }
                if (progress < 1)
                {
                    return false;
                }
                SetDistanceLimit();

                recenterMap = false;
                Task.Run(() => LoadList());
            }
            return true;
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
            c.CW("textFieldDidEndEditing: " + textField);
            if (textField == DistanceSourceAddressText)
            {
                MatchCoordinates(true);
                DistanceSourceAddressText.BackgroundColor = null;
                //DistanceSourceAddressText.Background.ClearColorFilter();
            }
            else if (textField == DistanceLimitInput)
            {
                int progress = int.Parse(DistanceLimitInput.Text);
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
            SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("ic_lightning36_pressed.png"), UIControlState.Normal);
            SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("ic_chat_two36.png"), UIControlState.Normal);
            SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("ic_registered36.png"), UIControlState.Normal);
            Session.SortBy = "LastActiveDate";
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        private void SortBy_ResponseRate_Click(object sender, EventArgs e)
        {
            SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("ic_lightning36.png"), UIControlState.Normal);
            SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("ic_chat_two36_pressed.png"), UIControlState.Normal);
            SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("ic_registered36.png"), UIControlState.Normal);
            Session.SortBy = "ResponseRate";
            Session.ResultsFrom = 1;
            recenterMap = true;
            Task.Run(() => LoadList());
        }

        private void SortBy_RegisterDate_Click(object sender, EventArgs e)
        {
            SortBy_LastActiveDate.SetBackgroundImage(UIImage.FromBundle("ic_lightning36.png"), UIControlState.Normal);
            SortBy_ResponseRate.SetBackgroundImage(UIImage.FromBundle("ic_chat_two36.png"), UIControlState.Normal);
            SortBy_RegisterDate.SetBackgroundImage(UIImage.FromBundle("ic_registered36_pressed.png"), UIControlState.Normal);
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
                OrderBy.SetBackgroundImage(UIImage.FromBundle("ic_ascending.png"), UIControlState.Normal);
            }
            else
            {
                Session.OrderBy = "desc";
                //OrderBy.TooltipText = res.GetString(Resource.String.Descending);
                OrderBy.SetBackgroundImage(UIImage.FromBundle("ic_descending.png"), UIControlState.Normal);
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
                UIView.Animate(tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                distanceFiltersOpen = true;
                Settings.GeoFiltersOpen = true;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("ic_collapse.png"), UIControlState.Normal);
                //DistanceFiltersOpenClose.TooltipText = res.GetString(Resource.String.DistanceFiltersClose);
            }
            else
            {
                c.CollapseY(DistanceFilters);
                UIView.Animate(tweenTime, () => { View.LayoutIfNeeded(); }, () => { });
                distanceFiltersOpen = false;
                Settings.GeoFiltersOpen = false;
                DistanceFiltersOpenClose.SetBackgroundImage(UIImage.FromBundle("ic_expand.png"), UIControlState.Normal);
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
                if ((bool)Settings.IsMapView && !(bool)Session.UseLocation)
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
                c.CW("DistanceSource_Click IsLocationEnabled " + c.IsLocationEnabled() + " UseLocation " + Session.UseLocation);
                c.LogActivity("DistanceSource_Click IsLocationEnabled " + c.IsLocationEnabled() + " UseLocation " + Session.UseLocation);
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
                    c.CW("DistanceSource_click UseLocation off IsMapView " + Settings.IsMapView + " IsOwnLocationAvailable " + c.IsOwnLocationAvailable());
                    c.DisplayCustomDialog("", LangEnglish.MapViewNoUseLocation, LangEnglish.DialogYes, LangEnglish.DialogNo, alert => {
                        UpdateLocationSetting();

                        c.CW("DistanceSource_click after updated location setting GeoSourceOther " + Session.GeoSourceOther);

                        Session.LastDataRefresh = null;
                        firstLocationAcquired = false;

                        ResultSet.Hidden = false;
                        ResultSet.Text = LangEnglish.GettingLocation;

                        if (locMgr is null)
                        {
                            locMgr = new LocationManager(this);
                        }
                        locMgr.StartLocationUpdates(); //will load the list

                    }, alert => {
                        SetDistanceSourceAddress();
                    });
                }
            }
            else
            {
                DistanceSourceCurrent.Checked = false;
                c.ExpandY(DistanceSourceAddressText);
                c.ExpandY(AddressOK);
            }

            Session.GeoSourceOther = DistanceSourceAddress.Checked;
            c.CW("DistanceSource_click GeoSourceOther set " + Session.GeoSourceOther);
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
                    DistanceSourceAddressText.BackgroundColor = UIColor.FromRGB(204, 255, 204);
                        //.Background.SetColorFilter(Color.Rgb(20, 224, 0), PorterDuff.Mode.SrcAtop);
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

		private void DistanceLimit_ValueChanged(object sender, EventArgs e)
        {
            //c.CW("DistanceLimit_ValueChanged distanceLimitChangedByCode " + distanceLimitChangedByCode);
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
            int progress = int.Parse(DistanceLimitInput.Text);//------------ need to set it

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
            //DistanceLimitInput.ClearFocus();
        }

        private void UserSearchList_ItemClick(UITapGestureRecognizer recognizer)
        {
            View.EndEditing(true);

            var location = recognizer.LocationInView(UserSearchList);
            var indexPath = UserSearchList.IndexPathForItemAtPoint(location);

            if (indexPath != null)
            {
                absoluteIndex = indexPath.Row + (int)Session.ResultsFrom - 1;
                absoluteStartIndex = (int)Session.ResultsFrom - 1;

                IntentData.profileViewPageType = "list";
                viewIndex = indexPath.Row;
                absoluteIndex = viewIndex + (int)Session.ResultsFrom - 1;
                absoluteStartIndex = (int)Session.ResultsFrom - 1;

                CommonMethods.OpenPage("ProfileViewActivity", 1);

                //c.LogActivity("UserSearchList_ItemClick viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " absoluteStartIndex " + ListActivity.absoluteStartIndex + " ResultsFrom " + Session.ResultsFrom + " view count " + ListActivity.viewProfiles.Count);
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
            //c.CW("UserSearchList_Touch state: " + recognizer.State);

            var location = recognizer.LocationInView(UserSearchList);
            if (recognizer.State == UIGestureRecognizerState.Began)
            {
                //c.CW("UserSearchList_Touch begin " + location.X + " " + location.Y);
                UserSearchList_Down(location.Y);
            }

            if (recognizer.State == UIGestureRecognizerState.Changed)
            {
                //c.CW("UserSearchList_Touch moved " + location.X + " " + location.Y);
                UserSearchList_Move(location.Y);
            }

            if (recognizer.State != (UIGestureRecognizerState.Cancelled | UIGestureRecognizerState.Failed | UIGestureRecognizerState.Possible))
            {
                //c.CW("UserSearchList_Touch moved2 " + offset.X + " " + offset.Y);
            }

            if (recognizer.State == UIGestureRecognizerState.Ended)
            {
                //c.CW("UserSearchList_Touch ended " + location.X + " " + location.Y);
                UserSearchList_Up();
            }
        }

        public void UserSearchList_Down(nfloat y)
        {
            c.CW("UserSearchList_Down scrollY " + UserSearchList.ContentOffset.Y + " y " + y);
            if (listLoading || UserSearchList.ContentOffset.Y != 0)
            {
                return;
            }
            startY = y;
        }

        public void UserSearchList_Move(nfloat y)
        {

            if (startY is null)
            {
                return;
            }
            diff = y - (nfloat)startY;
            c.CW("UserSearchList_Move y " + y + " startY " + startY + " diff " + diff);
            if (diff >= 0 && diff < maxY)
            {
                ReloadPulldown.Center = new CGPoint(ReloadPulldown.Center.X, -loaderHeight / 2 + diff / 2);
                ReloadPulldown.Alpha = diff / maxY;

                CGAffineTransform t = CGAffineTransform.MakeRotation((nfloat)(diff / maxY * 2 * Math.PI));
                ReloadPulldown.Transform = t;

                c.CW("Rotate: " + (diff / maxY * 2 * Math.PI));
            }
            else if (diff >= maxY)
            {
                c.CW("Max, center: " + -loaderHeight / 2 + maxY / 2);
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
            c.CW("UserSearchList_Up startY " + startY + " diff " + diff);
            if (startY is null)
            {
                return;
            }
            if (diff >= maxY)
            {
                Session.ResultsFrom = 1;
                recenterMap = true;
                c.CW("UserSearchList_Up loading list");
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
                UIView.Animate(duration: tweenTime, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
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
            MapStreet.BackgroundColor = UIColor.FromRGBA(204, 204, 204, 184);
            MapSatellite.BackgroundColor = UIColor.FromRGBA(255, 255, 255, 184);
        }

        private void MapSatellite_Click(object sender, EventArgs e)
        {
            ListViewMap.MapType = MKMapType.Satellite;
            MapStreet.BackgroundColor = UIColor.FromRGBA(255, 255, 255, 184);
            MapSatellite.BackgroundColor = UIColor.FromRGBA(204, 204, 204, 184);
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

            Session.ResultsFrom = Session.ResultsFrom + Constants.MaxResultCount;
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
                c.CW("LoadList listLoading: " + listLoading);
                c.LogActivity("LoadList listLoading: " + listLoading);
                //c.CW("LoadList listLoading " + listLoading);
                if (listLoading)
                {
                    return;
                }

                //last check
                if ((bool)Session.GeoFilter && (!(bool)Session.GeoSourceOther && !c.IsOwnLocationAvailable() || (bool)Session.GeoSourceOther && !c.IsOtherLocationAvailable()))
                {
                    InvokeOnMainThread(() => {
                        SetResultStatus();
                        c.CW("Exiting loadlist GeoFilter " + Session.GeoFilter + " GeoSourceOther " + Session.GeoSourceOther
                            + " own location " + c.IsOwnLocationAvailable() + " other location " + c.IsOtherLocationAvailable());
                        c.LogActivity("Exiting loadlist GeoFilter " + Session.GeoFilter + " GeoSourceOther " + Session.GeoSourceOther
                            + " own location " + c.IsOwnLocationAvailable() + " other location " + c.IsOtherLocationAvailable());
                        c.SnackIndef(LangEnglish.GeoFilterNoLocation);
                        /*if (ReloadPulldown.Alpha == 1)
                        {
                            HidePulldown();
                        }*/
                    });
                    return;
                }

                InvokeOnMainThread(() => { SetDistanceLimit(); });

                listLoading = true;

                InvokeOnMainThread(() => {
                    StartLoaderAnim();
                    ResultSet.Text = LangEnglish.LoadingList; //makes reloadpulldown and snackbar jump back to its constraint value
                    ResultSet.Hidden = false;
                    LoadNext.Hidden = true;
                    LoadPrevious.Hidden = true;
                });

                Session.LastSearchType = Constants.SearchType_Filter;

                string latitudeStr = (Session.Latitude is null) ? "" : ((double)Session.Latitude).ToString(CultureInfo.InvariantCulture);
                string longitudeStr = (Session.Longitude is null) ? "" : ((double)Session.Longitude).ToString(CultureInfo.InvariantCulture);
                string otherLatitudeStr = (Session.OtherLatitude is null) ? "" : ((double)Session.OtherLatitude).ToString(CultureInfo.InvariantCulture);
                string otherLongitudeStr = (Session.OtherLongitude is null) ? "" : ((double)Session.OtherLongitude).ToString(CultureInfo.InvariantCulture);

                LoadResults("action=list&ID=" + Session.ID + "&SessionID=" + Session.SessionID +
                    "&Latitude=" + latitudeStr + "&Longitude=" + longitudeStr +
                    "&ListType=" + Session.ListType + "&SortBy=" + Session.SortBy + "&OrderBy=" + Session.OrderBy + "&GeoFilter=" + Session.GeoFilter +
                    "&GeoSourceOther=" + Session.GeoSourceOther + "&OtherLatitude=" + otherLatitudeStr + "&OtherLongitude=" + otherLongitudeStr +
                    "&OtherAddress=" + c.UrlEncode(Session.OtherAddress) + "&DistanceLimit=" + Session.DistanceLimit + "&ResultsFrom=" + Session.ResultsFrom);
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
                c.CW("LoadListSearch listLoading: " + listLoading);
                c.LogActivity("LoadListSearch listLoading: " + listLoading);
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

                string latitudeStr = (Session.Latitude is null) ? "" : ((double)Session.Latitude).ToString(CultureInfo.InvariantCulture);
                string longitudeStr = (Session.Longitude is null) ? "" : ((double)Session.Longitude).ToString(CultureInfo.InvariantCulture);

                LoadResults("action=listsearch&ID=" + Session.ID + "&SessionID=" + Session.SessionID
                    + "&Latitude=" + latitudeStr + "&Longitude=" + longitudeStr + "&ListType=" + Session.ListType
                    + "&SortBy=" + Session.SortBy + "&OrderBy=" + Session.OrderBy + "&SearchTerm=" + c.UrlEncode(Session.SearchTerm)
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
                        newListProfiles = parser.returnCollection;
                        viewProfiles = new List<Profile>(viewProfiles.Concat(newListProfiles));
                        //c.LogActivity("LoadResults add after absoluteFirstIndex " + absoluteFirstIndex);
                    }
                    else if (addResultsBefore)
                    {
                        newListProfiles = parser.returnCollection;
                        viewProfiles = new List<Profile>(newListProfiles.Concat(viewProfiles));
                        viewIndex += newListProfiles.Count;
                        absoluteFirstIndex -= newListProfiles.Count;
                        //c.LogActivity("LoadResults add before absoluteFirstIndex " + absoluteFirstIndex);
                    }
                    else
                    {
                        viewProfiles = listProfiles = parser.returnCollection;
                        adapter.items = listProfiles;
                        absoluteFirstIndex = absoluteStartIndex = (int)Session.ResultsFrom - 1;
                        //c.LogActivity("LoadResults list loading absoluteFirstIndex " + absoluteFirstIndex);
                        newListProfiles = null;
                    }

                    InvokeOnMainThread(() =>
                    {
                        NoResult.Hidden = true;
                        UserSearchList.ReloadData();
                        UserSearchList.LayoutIfNeeded();
                        c.SetHeight(UserSearchList, UserSearchList.ContentSize.Height);
                    });
                    //LoadImages();
                }
                else //no result
                {
                    listProfiles = new List<Profile>();
                    viewProfiles = null;
                    adapter.items = listProfiles;
                    InvokeOnMainThread(() =>
                    {
                        UserSearchList.ReloadData();
                        UserSearchList.LayoutIfNeeded();
                        c.SetHeight(UserSearchList, UserSearchList.ContentSize.Height);
                        //((UIScrollView)UserSearchList.Superview).ContentSize = UserSearchList.ContentSize;
                    });
                }

                usersLoaded = true;
                listLoading = false;

                InvokeOnMainThread(() =>
                {
                    SetResultStatus();
                });

                mapSet = false;
                if (((bool)Settings.IsMapView || mapToSet) && newListProfiles is null)
                {
                    SetMap();
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
                    LoadPrevious.Alpha = 1f;
                    LoadNext.Enabled = true;
                    LoadNext.Alpha = 1f;
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

            c.CW("Setting map");
            c.LogActivity("Setting map");

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
                result = MoveMap(Session.Latitude, Session.Longitude);
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
                MapView.SetBackgroundImage(UIImage.FromBundle("ic_map_pressed.png"), UIControlState.Normal);
                ListView.SetBackgroundImage(UIImage.FromBundle("ic_listall.png"), UIControlState.Normal);

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

                        if ((bool)Session.GeoFilter)
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
            }
            else
            {
                LoadNext.Hidden = true;
            }

            if (Session.ResultsFrom - 1 > 0)
            {
                LoadPrevious.Hidden = false;
                LoadPrevious.Enabled = true;
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
            rotationAnimation.Duration = loaderAnimTime;
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
            UIView.Animate(duration: tweenTime, delay: 0, options: UIViewAnimationOptions.CurveLinear, animation: () =>
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
 