//scrollview shows scrollbar if content is at least 11 dp taller

using Foundation;
using System;
using UIKit;
using CoreLocation;
using MapKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.Timers;
using CoreAnimation;
using CoreGraphics;

namespace LocationConnection
{
    public partial class ProfileViewActivity : BaseActivity, IUIScrollViewDelegate
    {
        public static LocationManager Manager { get; set; }
        Profile displayUser;
		nint paddingSelfPage = 10;
		nint paddingProfilePage = 64;
		nfloat percentProgresssWidth = 75;
		float counterCircleSize = 9;
		List<UIImageView> counterCircles;
		MKPointAnnotation thisMarker;
		byte? pageType;
		UIView spacerLeft, spacerRight;
		int imageIndex;
		UIView pressTarget;

		bool scrollValid;

		System.Timers.Timer refreshTimer;
		int refreshFrequency = 1000;
		Task imageLoading;
        System.Threading.CancellationTokenSource cts;

		public ProfileViewActivity (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            try {
                base.ViewDidLoad();

                c.AddViews(Snackbar, Snackbar.SnackText, Snackbar.SnackButton);

				MenuBlock.SetTitle(LangEnglish.MenuBlock, UIControlState.Normal);
				MenuReport.SetTitle(LangEnglish.MenuReport, UIControlState.Normal);

				c.HideMenu(MenuLayer, MenuContainer, false);

				ProfileViewScroll.Scrolled += ProfileViewScroll_Scrolled;
			    EditSelf.SetTitle(LangEnglish.EditSelf, UIControlState.Normal);
				SendLocation.SetTitle(LangEnglish.SendLocation, UIControlState.Normal);
				ProfileImageScroll.Delegate = this;

				SendLocation.Layer.MasksToBounds = true;
				EditSelf.Layer.MasksToBounds = true;

			    var tap = new UITapGestureRecognizer();
			    tap.AddTarget(() => ProfileImageScroll_Click(tap));
			    ProfileImageScroll.AddGestureRecognizer(tap);

			    MapStreet.SetTitle(LangEnglish.MapStreet, UIControlState.Normal);
			    MapSatellite.SetTitle(LangEnglish.MapSatellite, UIControlState.Normal);
			    ProfileViewMap.MapType = (MKMapType)Settings.ProfileViewMapType;
				if (Settings.ProfileViewMapType == (int)MKMapType.Standard)
				{
					MapStreet.BackgroundColor = UIColor.FromName("MapButtonActive");
					MapSatellite.BackgroundColor = UIColor.FromName("MapButtonPassive");
				}
				else
				{
					MapStreet.BackgroundColor = UIColor.FromName("MapButtonPassive");
					MapSatellite.BackgroundColor = UIColor.FromName("MapButtonActive");
				}
			    ProfileViewMap.Delegate = new CustomAnnotationView(this);

			    MapStreet.TouchUpInside += MapStreet_Click;
			    MapSatellite.TouchUpInside += MapSatellite_Click;

                EditSelfBack.TouchUpInside += EditSelfBack_Click;
                SendLocation.TouchUpInside += SendLocation_Click;
                EditSelf.TouchUpInside += EditSelf_Click;
                BackButton.TouchUpInside += BackButton_Click;

                PreviousButton.TouchUpInside += PreviousButton_Click;
                NextButton.TouchUpInside += NextButton_Click;
                HideButton.TouchUpInside += HideButton_Click;
                LikeButton.TouchUpInside += LikeButton_Click;

                EditSelfBack.TouchDown += EditBackButton_Touch;
			    BackButton.TouchDown += Button_Touch;
                PreviousButton.TouchDown += Button_Touch;
			    NextButton.TouchDown += Button_Touch;
			    LikeButton.TouchDown += Button_Touch;
			    HideButton.TouchDown += Button_Touch;

				MenuIcon.TouchUpInside += MenuIcon_Click;
				MenuLayer.TouchDown += MenuLayer_TouchDown;

				MenuReport.TouchUpInside += MenuReport_Click;
				MenuBlock.TouchUpInside += MenuBlock_Click;

				RoundBottom_Base = RoundBottom;
			    Snackbar_Base = Snackbar;
			    BottomConstraint_Base = BottomConstraint;
				SnackTopConstraint_Base = SnackTopConstraint;
				SnackBottomConstraint_Base = SnackBottomConstraint; 
			    ScrollBottomConstraint_Base = ScrollBottomConstraint;
				ViewportConstraint_Base = ViewportConstraint;
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

				ProfileImageScroll.ContentOffset = new CoreGraphics.CGPoint(0, 0);
				imageIndex = 0;

                if (pageType is null) //activity can be resumed by pressing a back button from chat one
                {
					pageType = IntentData.profileViewPageType;
				}
				c.CW("ProfileViewActivity ViewWillAppear pageType " + pageType + " viewindex " + ListActivity.viewIndex + " absolutestartindex " + ListActivity.absoluteStartIndex);

				switch (pageType)
				{
					case Constants.ProfileViewType_Self:
						c.Collapse(MenuIcon);

						if ((bool)Session.UseLocation && c.IsLocationEnabled() && !(locMgr is null))
						{
							locMgr.LocationUpdated += LocMgr_LocationUpdated;
						}
						ShowEditSpacer();


						EditSelfHeader.Hidden = false;
						EditSelfBack.Hidden = false;
						SendLocation.Hidden = false;
                        EditSelf.Hidden = false;

                        if ((bool)Session.UseLocation && c.IsLocationEnabled())
                        {
							SendLocation.Enabled = true;
							SendLocation.Alpha = 1;
                        }
                        else
                        {
							SendLocation.Enabled = false;
							SendLocation.Alpha = 0.5f;
                        }


						BackButton.Hidden = true;

						c.SetLeftMargin(Name, paddingSelfPage);
						c.SetLeftMargin(Username, paddingSelfPage);

						PreviousButton.Hidden = true;
						NextButton.Hidden = true;
						c.CollapseX(HideButton);
						c.CollapseX(LikeButton);

						Session.CurrentMatch = null;

						LoadSelf();
						HideNavigationSpacer();
						
						break;

					case Constants.ProfileViewType_List:
                        if (c.IsLoggedIn())
                        {
							c.Expand(MenuIcon);
						}
                        else
                        {
							c.Collapse(MenuIcon);
                        }

						HideEditSpacer();

						EditSelfHeader.Hidden = true;
						EditSelfBack.Hidden = true;
						SendLocation.Hidden = true;
						EditSelf.Hidden = true;

						BackButton.Hidden = false;

						c.SetLeftMargin(Name, paddingProfilePage);
						c.SetLeftMargin(Username, paddingProfilePage);

						PreviousButton.Hidden = false;
						NextButton.Hidden = false;
						if (c.IsLoggedIn())
						{
							c.ExpandX(HideButton);
							c.ExpandX(LikeButton);
						}
						else
						{
							c.CollapseX(HideButton);
							c.CollapseX(LikeButton);
						}

						if (ListActivity.viewProfiles.Count > Constants.MaxResultCount)
						{
							c.LogActivity("Error: ListActivity.viewProfiles.Count is greater than " + Constants.MaxResultCount + ": " + ListActivity.viewProfiles.Count);
						}						

                        if (ListActivity.viewIndex >= ListActivity.viewProfiles.Count) //when blocking a user from chat window, but returning to profileview list
                        {
							CommonMethods.OpenPage(null, 0);
							return;
                        }

						PrevLoadAction();
						NextLoadAction();

						displayUser = ListActivity.viewProfiles[ListActivity.viewIndex];

						Session.CurrentMatch = null;

						LoadUser();
						break;

					case Constants.ProfileViewType_Standalone: //coming from chat, we already know this is a match, Userrelation=3.
						c.Expand(MenuIcon);

						if (!(IntentData.targetID is null)) //when pressing back button from ChatOne, IntentData.targetID is null.
						{
							LoadStandalone((int)IntentData.targetID);
							IntentData.targetID = null;
						}
						break;
					default:
						break;
				}

				refreshTimer = new Timer();
				refreshTimer.Interval = refreshFrequency;
				refreshTimer.Elapsed += RefreshTimer_Elapsed;
				refreshTimer.Start();

				scrollValid = true;
			}
			catch (Exception ex)
			{
				c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
		{
			base.ViewWillTransitionToSize(toSize, coordinator);
			scrollValid = false;
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();

			ProfileImageContainer.LayoutIfNeeded();

			if (!scrollValid)
			{
				ProfileImageScroll.ContentOffset = new CGPoint(imageIndex * ProfileImageScroll.Frame.Width, 0);
				scrollValid = true;
			}

			c.SetShadow(MenuContainer, 0, 0, 10);

			SetOvalShadow(PreviousButton, 0, 6);
			SetOvalShadow(HideButton, 0, 6);
			SetOvalShadow(LikeButton, 0, 6);
			SetOvalShadow(NextButton, 0, 6);

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

			if (pageType == Constants.ProfileViewType_Self && (bool)Session.UseLocation && c.IsLocationEnabled() && !(locMgr is null))
			{
				locMgr.LocationUpdated -= LocMgr_LocationUpdated;
			}

			refreshTimer.Stop();

			if (ProfileViewMap.MapType != (MKMapType)Settings.ProfileViewMapType)
			{
				Settings.ProfileViewMapType = (int)ProfileViewMap.MapType;
				c.SaveSettings();
			}
		}

		public void SetOvalShadow(UIView view, float x, float y)
        {
			UIBezierPath path = UIBezierPath.FromOval(view.Bounds);
			view.Layer.MasksToBounds = false;
			view.Layer.ShadowColor = UIColor.Gray.CGColor;
			view.Layer.ShadowOffset = new CoreGraphics.CGSize(x, y);
			view.Layer.ShadowOpacity = 0.35f;
			view.Layer.ShadowPath = path.CGPath;
		}

		public void ClearShadow(UIView view)
		{
			view.Layer.ShadowOpacity = 0;
		}

		private void ProfileViewScroll_Scrolled(object sender, EventArgs e)
		{
			if (ProfileViewScroll.ContentOffset.Y <= 0)
			{
				ClearShadow(BackButton);
			}
			else
			{
				SetOvalShadow(BackButton, 0, 6);
			}
		}

		private void ProfileImageScroll_Click(UITapGestureRecognizer recognizer)
		{
			var location = recognizer.LocationInView(ProfileImageScroll);

			int newImageIndex;

			if (location.X - ProfileImageScroll.ContentOffset.X >= ProfileImageScroll.Frame.Width / 2)
			{
				if (imageIndex == counterCircles.Count - 1)
				{
					newImageIndex = 0;
				}
				else
				{
					newImageIndex = imageIndex + 1;
				}
			}
			else
			{
				if (imageIndex == 0)
				{
					newImageIndex = counterCircles.Count - 1;
				}
				else
				{
					newImageIndex = imageIndex - 1;
				}
			}

			ProfileImageScroll.ContentOffset = new CoreGraphics.CGPoint(ProfileImageScroll.Frame.Height * newImageIndex, 0);

			var circle = counterCircles[imageIndex];
			circle.Image = UIImage.FromBundle("counterCircle.png");
			circle = counterCircles[newImageIndex];
			circle.Image = UIImage.FromBundle("counterCircle_selected.png");

			imageIndex = newImageIndex;
		}

		[Export("scrollViewDidScroll:")]
		public void Scrolled(UIScrollView scrollView)
		{
            if (!scrollValid)
            {
				return;
            }
			int newImageIndex = (int)Math.Round(scrollView.ContentOffset.X / scrollView.Frame.Width);

			if (newImageIndex != imageIndex)
			{
				var circle = counterCircles[imageIndex];
				circle.Image = UIImage.FromBundle("counterCircle.png");
				circle = counterCircles[newImageIndex];
				circle.Image = UIImage.FromBundle("counterCircle_selected.png");

				imageIndex = newImageIndex;
			}
		}

		private void EditBackButton_Touch(object sender, EventArgs e)
        {
			c.AnimateRipple(RippleProfileView, 2);
        }

		private void Button_Touch(object sender, EventArgs e)
        {
            if (!rippleRunning)
            {
				pressTarget = (UIView)sender;
				if (pressTarget == BackButton || pressTarget == PreviousButton || pressTarget == HideButton)
				{
					AnimateRipple(RippleImagePrev, 3);
				}
				else
				{
					AnimateRipple(RippleImageNext, 3);
				}
			}
        }

		private void AnimateRipple(UIView RippleImage, byte zoom)
        {
			foreach (NSLayoutConstraint constraint in View.Constraints)
			{
				if (constraint.FirstItem == RippleImage)
				{
					View.RemoveConstraint(constraint);
				}
			}
			RippleImage.CenterXAnchor.ConstraintEqualTo(pressTarget.CenterXAnchor).Active = true;
			RippleImage.CenterYAnchor.ConstraintEqualTo(pressTarget.CenterYAnchor).Active = true;
			View.LayoutIfNeeded();

			c.AnimateRipple(RippleImage, zoom);
		}

		private void ShowEditSpacer()
		{
			c.ExpandY(EditSpacer);
		}

		private void HideEditSpacer()
		{
			c.CollapseY(EditSpacer);
		}

		private void LoadStandalone(int targetID)
		{
			HideEditSpacer();

			EditSelfHeader.Hidden = true;
			EditSelfBack.Hidden = true;
			SendLocation.Hidden = true;
			EditSelf.Hidden = true;

			BackButton.Hidden = false;

			c.SetLeftMargin(Name, paddingProfilePage);
			c.SetLeftMargin(Username, paddingProfilePage);

			PreviousButton.Hidden = true;
			NextButton.Hidden = true;
			c.CollapseX(HideButton);
			c.ExpandX(LikeButton);

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

			string responseString = c.MakeRequestSync("action=getuserdata&ID=" + Session.ID + "&target=" + targetID
		+ "&SessionID=" + Session.SessionID + "&Latitude=" + latitudeStr + "&Longitude=" + longitudeStr); //if we used await c.MakeRequest here, the OnResume would return, and the map would set to world view before user data is loaded.
			if (responseString.Substring(0, 2) == "OK")
			{
				responseString = responseString.Substring(3);
				ServerParser<Profile> parser = new ServerParser<Profile>(responseString);
				displayUser = parser.returnCollection[0];

				UserLocationData data = GetLocationData(targetID);

                if (data != null)
                {
					displayUser.Latitude = data.Latitude;
					displayUser.Longitude = data.Longitude;
					displayUser.LocationTime = data.LocationTime;
					if (!(displayUser.Distance is null))
					{
						float distance;
						if (!Constants.SafeLocationMode)
						{
							distance = CalculateDistance((double)Session.Latitude, (double)Session.Longitude, data.Latitude, data.Longitude);
						}
						else
						{
							distance = CalculateDistance((double)Session.LatestLatitude, (double)Session.LatestLongitude, data.Latitude, data.Longitude);
						}

						displayUser.Distance = distance;
					}
				}

				LoadUser();
			}
			else if (responseString.Substring(0, 6) == "ERROR_") // UserPassive, MatchNotFound or UserNotAvailable 
			{
				Session.SnackMessage = c.GetLang(responseString.Substring(6));
				CommonMethods.OpenPage(null, 0);
			}
			else
			{
				c.ReportError(responseString);
			}
		}

		private void LoadSelf()
		{
			try
			{
				c.RemoveSubviews(ProfileImageScroll);

				if (!(counterCircles is null))
				{
					for (int i = 0; i < counterCircles.Count; i++)
					{
						counterCircles[i].RemoveFromSuperview();
					}
				}
				counterCircles = new List<UIImageView>();

				Username.Text = Session.Username;
				Name.Text = Session.Name;
				ProfileViewDescription.Text = Session.Description;
				SetPercentProgress((float)Session.ResponseRate);
				ResponseRate.Text = Math.Round((float)Session.ResponseRate * 100).ToString() + "%";
				LastActiveDate.Text = c.GetTimeDiffStr(Session.LastActiveDate, true);
				DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((long)Session.RegisterDate).ToLocalTime();
				if (dt.Date == DateTime.Today)
				{
					RegisterDate.Text = dt.ToString("HH:mm");
				}
				else
				{
					RegisterDate.Text = dt.ToString("d MMMM yyyy");
				}

				LoadEmptyPictures(Session.Pictures.Length);

				SetMap();

				AddCircles(Session.Pictures.Length);

				cts = new System.Threading.CancellationTokenSource();
				imageLoading = Task.Run(() =>
				{
					for (int i = 0; i < Session.Pictures.Length; i++)
					{
						LoadPicture(Session.ID.ToString(), Session.Pictures[i], i, true);
					}
				}, cts.Token);

			}
			catch (Exception ex)
			{
				c.ReportErrorSilent(ex.Message + System.Environment.NewLine + ex.StackTrace);
			}
		}

		private void LoadUser()
        {
			try
			{
				c.CW("Loaduser ID " + displayUser.ID);

				if (c.IsLoggedIn())
				{
					switch (displayUser.UserRelation)
					{
						case 0: //default
							LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_like.png"), UIControlState.Normal);
							LikeButton.UserInteractionEnabled = true;
							c.ExpandX(LikeButton);

							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Normal);
							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Highlighted);
							c.ExpandX(HideButton);
							break;
						case 1: //hid
							LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_like.png"), UIControlState.Normal);
							LikeButton.UserInteractionEnabled = true;
							c.CollapseX(LikeButton);

							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_reinstate.png"), UIControlState.Normal);
							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_reinstate.png"), UIControlState.Highlighted);
							c.ExpandX(HideButton);
							break;
						case 2: //liked
							LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_liked.png"), UIControlState.Normal);
							LikeButton.UserInteractionEnabled = false;
							c.ExpandX(LikeButton);

							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Normal);
							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Highlighted);
							c.ExpandX(HideButton);
							break;
						case 3: //match
						case 4: //friend
							LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Normal);
							LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Highlighted);
							LikeButton.UserInteractionEnabled = true;
							c.ExpandX(LikeButton);

							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Normal);
							HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Highlighted);
							c.CollapseX(HideButton);
							break;
						default:
							break;
					}
				}

				c.RemoveSubviews(ProfileImageScroll);

				if (!(counterCircles is null))
				{
					for (int i = 0; i < counterCircles.Count; i++)
					{
						counterCircles[i].RemoveFromSuperview();
					}
				}
				counterCircles = new List<UIImageView>();

				Username.Text = displayUser.Username;
				Name.Text = displayUser.Name;
				ProfileViewDescription.Text = displayUser.Description;
				SetPercentProgress(displayUser.ResponseRate);
				ResponseRate.Text = Math.Round(displayUser.ResponseRate * 100).ToString() + "%";
				LastActiveDate.Text = c.GetTimeDiffStr(displayUser.LastActiveDate, true);
				DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(displayUser.RegisterDate).ToLocalTime();
				if (dt.Date == DateTime.Today)
				{
					RegisterDate.Text = dt.ToString("HH:mm");
				}
				else
				{
					RegisterDate.Text = dt.ToString("d MMMM yyyy");
				}

				LoadEmptyPictures(displayUser.Pictures.Length);
                               				
				SetMap();

				AddCircles(displayUser.Pictures.Length);

				cts = new System.Threading.CancellationTokenSource();
				imageLoading = Task.Run(() =>
				{                    
					for (int i = 0; i < displayUser.Pictures.Length; i++)
					{
						LoadPicture(displayUser.ID.ToString(), displayUser.Pictures[i], i, false);
					}
				}, cts.Token);
			}
			catch (Exception ex)
			{
				c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}		

		private void SetPercentProgress(float responseRate)
		{
			c.SetWidth(PercentProgress, percentProgresssWidth * responseRate);
			byte red = (byte)(192 - (byte)Math.Round(192 * responseRate));
			byte green = (byte)Math.Round(192 * responseRate);
			byte blue = 0;
			UIColor color = UIColor.FromRGB(red, green, blue);
			PercentProgress.BackgroundColor = color;
		}

        private void SetMap()
        {
            try
            {
				if (pageType == Constants.ProfileViewType_Self)
				{
					if (Session.Latitude != null && Session.Longitude != null && Session.LocationTime != null) //location available
					{
						ProfileViewMap.ShowsUserLocation = false;

						CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D((double)Session.Latitude, (double)Session.Longitude);
						MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 1000 * 2, 1000 * 2);
						ProfileViewMap.CenterCoordinate = mapCenter;
						ProfileViewMap.Region = mapRegion;

						if (!(thisMarker is null))
						{
							ProfileViewMap.RemoveAnnotation(thisMarker);
						}

						thisMarker = new MKPointAnnotation() { Title = "Center", Coordinate = mapCenter };
						ProfileViewMap.AddAnnotation(thisMarker);

						LocationTime.Text = LangEnglish.ProfileViewLocation + " " + c.GetTimeDiffStr(Session.LocationTime, false);
						c.Expand(LocationTime);
						ShowMap();
					}
					else
					{
						c.Collapse(LocationTime); //distance is not shown on self page		
						HideMap();
					}
					c.Collapse(DistanceText);
					HideNavigationSpacer();
				}
                else
                {
					if (displayUser.Latitude != null && displayUser.Longitude != null && displayUser.LocationTime != null) //location available
					{
						if (c.IsLocationEnabled())
						{
							ProfileViewMap.ShowsUserLocation = true;
						}
						else
						{
							ProfileViewMap.ShowsUserLocation = false;
						}


						CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D((double)displayUser.Latitude, (double)displayUser.Longitude);
						MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 1000 * 2, 1000 * 2);
						ProfileViewMap.CenterCoordinate = mapCenter;
						ProfileViewMap.Region = mapRegion;

						if (!(thisMarker is null))
						{
							ProfileViewMap.RemoveAnnotation(thisMarker);
						}

						thisMarker = new MKPointAnnotation() { Title = "Center", Coordinate = mapCenter };
						ProfileViewMap.AddAnnotation(thisMarker);

						LocationTime.Text = LangEnglish.ProfileViewLocation + " " + c.GetTimeDiffStr(displayUser.LocationTime, false);
						c.Expand(LocationTime);

						if (!(displayUser.Distance is null))
						{
                            //A default text of " " was given to DistanceText, otherwise the intrinsic width would be 0, which if removed by Expand(), would make DistanceText lose its intrinsic content size. (It would be stretched to appear next to LocationText)
                            //LocationText does not have this problem due to the TopInset and RightInset set to 10.
                            //If DistanceText is too long, LocationText will be compressed to display smaller font, or disappear completely.
							c.Expand(DistanceText);
							DistanceText.Text = displayUser.Distance + " km " + LangEnglish.ProfileViewAway;
						}
						else
						{
							c.Collapse(DistanceText);
						}
						ShowMap();
					}
					else
					{
						if (!(displayUser.Distance is null))
						{
							c.Collapse(LocationTime);
							c.Expand(DistanceText);
							DistanceText.Text = LangEnglish.ProfileViewDistance + " " + c.GetTimeDiffStr(displayUser.LocationTime, false) + ": " + displayUser.Distance + " km ";
						}
						else
						{
							c.Collapse(LocationTime);
							c.Collapse(DistanceText);
						}
						HideMap();
					}
					ShowNavigationSpacer();
				}
			}
			catch (Exception ex)
			{
				c.ReportErrorSilent(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		private void ShowMap()
		{
			c.SetHeight(ProfileViewMap, (float)(dpWidth * Settings.MapRatio));

			MapTopSeparator.Hidden = false;
			MapStreet.Hidden = false;
			MapSatellite.Hidden = false;
		}

		private void HideMap()
		{
			c.SetHeight(ProfileViewMap, 0);

			MapTopSeparator.Hidden = true;
			MapStreet.Hidden = true;
			MapSatellite.Hidden = true;
		}

		private void ShowNavigationSpacer()
		{
			c.ExpandY(NavigationSpacer);
		}

		private void HideNavigationSpacer()
		{
			c.CollapseY(NavigationSpacer);
		}

		private void AddCircles(int count)
		{
            if (spacerLeft is null)
            {
				spacerLeft = new UIView();
				ProfileImageContainer.AddSubview(spacerLeft);
				spacerLeft.TranslatesAutoresizingMaskIntoConstraints = false;

                spacerRight = new UIView();
				ProfileImageContainer.AddSubview(spacerRight);
				spacerRight.TranslatesAutoresizingMaskIntoConstraints = false;

				spacerLeft.WidthAnchor.ConstraintEqualTo(spacerRight.WidthAnchor).Active = true;
				spacerLeft.LeftAnchor.ConstraintEqualTo(ProfileImageContainer.LeftAnchor).Active = true;
				spacerRight.RightAnchor.ConstraintEqualTo(ProfileImageContainer.RightAnchor).Active = true;
			}

			for (int i = 0; i < count; i++)
			{
				UIImageView v = new UIImageView();

				ProfileImageContainer.AddSubview(v);

				v.Alpha = 0.8f;
				v.TranslatesAutoresizingMaskIntoConstraints = false;
				v.WidthAnchor.ConstraintEqualTo(counterCircleSize).Active = true;
				v.HeightAnchor.ConstraintEqualTo(counterCircleSize).Active = true;
				v.BottomAnchor.ConstraintEqualTo(ProfileImageScroll.BottomAnchor, -2).Active = true;

				if (i == 0)
				{
					v.LeftAnchor.ConstraintEqualTo(spacerLeft.RightAnchor).Active=true;
					v.Image = UIImage.FromBundle("counterCircle_selected.png");
				}
				else
				{
					v.LeftAnchor.ConstraintEqualTo(counterCircles[i - 1].RightAnchor, 1.5f).Active = true;
					v.Image = UIImage.FromBundle("counterCircle.png");
				}
				if (i == count - 1)
				{
					v.RightAnchor.ConstraintEqualTo(spacerRight.LeftAnchor).Active = true;
				}
				counterCircles.Add(v);
			}
		}

        private void LoadEmptyPictures(int count)
        {
			for (int index = 0; index < count; index++)
            {
				UIImageView ProfileImage = new UIImageView();

				ProfileImageScroll.AddSubview(ProfileImage);
				ProfileImage.TranslatesAutoresizingMaskIntoConstraints = false;

				ProfileImage.WidthAnchor.ConstraintEqualTo(ProfileImageScroll.HeightAnchor).Active = true;
				ProfileImage.HeightAnchor.ConstraintEqualTo(ProfileImageScroll.HeightAnchor).Active = true;
				ProfileImage.TopAnchor.ConstraintEqualTo(ProfileImageScroll.TopAnchor).Active = true;
				ProfileImage.BottomAnchor.ConstraintEqualTo(ProfileImageScroll.BottomAnchor).Active = true;
				ProfileImage.RightAnchor.ConstraintEqualTo(ProfileImageScroll.RightAnchor).Active = true;

				if (index == 0)
				{
					ProfileImage.LeftAnchor.ConstraintEqualTo(ProfileImageScroll.LeftAnchor).Active = true;
				}
				else
				{
					UIView prevImage = ProfileImageScroll.Subviews[index - 1];
					foreach (NSLayoutConstraint constraint in ProfileImageScroll.Constraints)
					{
						if (constraint.FirstItem == prevImage && constraint.FirstAttribute == NSLayoutAttribute.Right)
						{
							ProfileImageScroll.RemoveConstraint(constraint);
						}
					}
					ProfileImage.LeftAnchor.ConstraintEqualTo(prevImage.RightAnchor).Active = true;
				}

				ProfileImage.Image = UIImage.FromBundle(Constants.loadingImage);
			}
		}

		private void LoadPicture(string folder, string picture, int index, bool usecache)
		{
			//c.CW("LoadPicture " + folder + " " + picture + " " + index);
			try {
				UIImageView ProfileImage = null;
				InvokeOnMainThread(() => {
					ProfileImage = (UIImageView)ProfileImageScroll.Subviews[index];
				});

                if (usecache)
                {
					ImageCache im = new ImageCache(this);
					InvokeOnMainThread(() => {
						im.LoadImage(ProfileImage, folder, picture, true);
					});
				}
                else
                {
					string url;
					if (Constants.isTestDB)
					{
						url = Constants.HostName + Constants.UploadFolderTest + "/" + folder + "/" + Constants.LargeImageSize + "/" + picture;
					}
					else
					{
						url = Constants.HostName + Constants.UploadFolder + "/" + folder + "/" + Constants.LargeImageSize + "/" + picture;
					}

					UIImage im = CommonMethods.LoadFromUrl(url);

					if (im == null)
					{
						im = UIImage.FromBundle(Constants.noImageHD);
					}

					InvokeOnMainThread(() => {
						ProfileImage.Image = im;
					});
				}
			    
		    }
			catch (Exception ex)
			{
				c.CW(ex.Message + " " + ex.StackTrace);
			}
        }

		private void RefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			InvokeOnMainThread(() => {
				switch (pageType)
				{
					case Constants.ProfileViewType_Self:
						LastActiveDate.Text = c.GetTimeDiffStr(Session.LastActiveDate, true);

						if (Session.Latitude != null && Session.Longitude != null && Session.LocationTime != null)
						{
							if (ProfileViewMap.Frame.Height == 0) //after enabling location in profile edit, display map
							{
								SetMap();
							}
                            else
                            {
								LocationTime.Text = LangEnglish.ProfileViewLocation + " " + c.GetTimeDiffStr(Session.LocationTime, false);
							}
						}

						break;
					case Constants.ProfileViewType_List:
					case Constants.ProfileViewType_Standalone:
                        if (!(displayUser is null)) { //in case user is passive
							LastActiveDate.Text = c.GetTimeDiffStr(displayUser.LastActiveDate, true);
							if (displayUser.Latitude != null && displayUser.Longitude != null && displayUser.LocationTime != null)
							{
								LocationTime.Text = LangEnglish.ProfileViewLocation + " " + c.GetTimeDiffStr(displayUser.LocationTime, false);
							}
						}
						break;
				}
			});
		}

		private void MapStreet_Click(object sender, EventArgs e)
		{
			ProfileViewMap.MapType = MKMapType.Standard;
			MapStreet.BackgroundColor = UIColor.FromName("MapButtonActive");
			MapSatellite.BackgroundColor = UIColor.FromName("MapButtonPassive");
		}

		private void MapSatellite_Click(object sender, EventArgs e)
		{
			ProfileViewMap.MapType = MKMapType.Satellite;
			MapStreet.BackgroundColor = UIColor.FromName("MapButtonPassive");
			MapSatellite.BackgroundColor = UIColor.FromName("MapButtonActive");
		}

		private void BackButton_Click(object sender, EventArgs e)
		{
			CancelTask();

			if (Session.ListType == "hid")
			{
				Session.ResultsFrom = 1;
				Session.LastDataRefresh = null;
				ListActivity.listProfiles = null;
			}
			CommonMethods.OpenPage(null, 0);
		}

		private void EditSelfBack_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			BackButton_Click(null, null);
		}

		private async void SendLocation_Click(object sender, EventArgs e)
		{
            if (Session.LatestLatitude != null && Session.LatestLongitude != null)
            {
				if (await c.UpdateLocationSync(false))
				{
					Session.Latitude = Session.LatestLatitude;
					Session.Longitude = Session.LatestLongitude;
					Session.LocationTime = Session.LatestLocationTime;

					c.LogLocation(Session.LocationTime + "|" + ((double)Session.Latitude).ToString(CultureInfo.InvariantCulture) + "|" + ((double)Session.Longitude).ToString(CultureInfo.InvariantCulture) + "|2");

					UpdateLocationSelf();

					//c.Snack(LangEnglish.LocationUpdated);
				}
				else
				{
					c.Snack(LangEnglish.LocationNotUpdated);
				}
			}
            else
            {
				c.Snack(LangEnglish.LocationNotAcquired);
            }
		}

		private void EditSelf_Click(object sender, EventArgs e)
		{
			CancelTask();
			c.HideMenu(MenuLayer, MenuContainer, true);

			CommonMethods.OpenPage("ProfileEditActivity", 1);
		}

		private void PreviousButton_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			ListActivity.viewIndex--;
			ListActivity.absoluteIndex--;

			CancelTask();

			c.CW("PreviousButton_Click viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " viewProfiles.Count " + ListActivity.viewProfiles.Count + " listProfiles.Count " + ListActivity.listProfiles.Count);

			if (ListActivity.viewIndex >= 0)
			{
				PrevLoadAction();
				displayUser = ListActivity.viewProfiles[ListActivity.viewIndex];
				ProfileImageScroll.ContentOffset = new CoreGraphics.CGPoint(0, 0);
				imageIndex = 0;
				c.CW("Loading previous user");
				LoadUser();
			}
			else
			{
				ListActivity.absoluteIndex++;
				//loading may be in progress, new list will be shown.
				BackButton_Click(null, null);
			}
		}

		private void NextButton_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			ListActivity.viewIndex++;
			ListActivity.absoluteIndex++;

            CancelTask();

			c.CW("NextButton_Click viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " viewProfiles.Count " + ListActivity.viewProfiles.Count + " listProfiles.Count " + ListActivity.listProfiles.Count);

			if (ListActivity.viewIndex < ListActivity.viewProfiles.Count)
			{
				NextLoadAction();
				displayUser = ListActivity.viewProfiles[ListActivity.viewIndex];
				ProfileImageScroll.ContentOffset = new CoreGraphics.CGPoint(0, 0);
				imageIndex = 0;
				c.CW("Loading next user");
				LoadUser();
			}
			else
			{
				ListActivity.absoluteIndex--;
				//loading may be in progress, new list will be shown.
				BackButton_Click(null, null);
			}
		}

		private void PrevLoadAction()
		{
			//c.LogActivity("Prev viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " absoluteStartIndex " + ListActivity.absoluteStartIndex + " ResultsFrom " + Session.ResultsFrom + " view count " + ListActivity.viewProfiles.Count);
			c.CW("PrevLoadAction viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " absoluteStartIndex " + ListActivity.absoluteStartIndex + " ResultsFrom " + Session.ResultsFrom + " viewProfiles.Count " + ListActivity.viewProfiles.Count + " totalResultCount " + ListActivity.totalResultCount);
			if (ListActivity.viewIndex == 0 && ListActivity.absoluteFirstIndex > 0)
			{
				Session.ResultsFrom = ListActivity.absoluteIndex - Constants.MaxResultCount + 1;
				//c.LogActivity("Prev2 ResultsFrom " + Session.ResultsFrom);
				//c.CW("Prev2 ResultsFrom " + Session.ResultsFrom);
				ListActivity.addResultsBefore = true;
				if (Session.LastSearchType == Constants.SearchType_Filter)
				{
					Task.Run(() => ListActivity.thisInstance.LoadList());
				}
				else
				{
					Task.Run(() => ListActivity.thisInstance.LoadListSearch());
				}
			}
		}

		private void NextLoadAction()
		{
			//c.LogActivity("Next viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " absoluteStartIndex " + ListActivity.absoluteStartIndex + " ResultsFrom " + Session.ResultsFrom + " view count " + ListActivity.viewProfiles.Count);
			c.CW("NextLoadAction viewIndex " + ListActivity.viewIndex + " absoluteIndex " + ListActivity.absoluteIndex + " absoluteStartIndex " + ListActivity.absoluteStartIndex + " ResultsFrom " + Session.ResultsFrom + " viewProfiles.Count " + ListActivity.viewProfiles.Count + " totalResultCount " + ListActivity.totalResultCount);
			if (ListActivity.viewIndex == ListActivity.viewProfiles.Count - 1 && ListActivity.totalResultCount > ListActivity.absoluteIndex + 1) //list will be loaded
			{
				Session.ResultsFrom = ListActivity.absoluteIndex + 2;
				//c.LogActivity("Next2 ResultsFrom " + Session.ResultsFrom);
				//c.CW("Next2 ResultsFrom " + Session.ResultsFrom);
				ListActivity.addResultsAfter = true;
				if (Session.LastSearchType == Constants.SearchType_Filter)
				{
					Task.Run(() => ListActivity.thisInstance.LoadList());
				}
				else
				{
					Task.Run(() => ListActivity.thisInstance.LoadListSearch());
				}
			}
		}

		private async void LikeButton_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			if (pageType == Constants.ProfileViewType_Standalone)
			{
				IntentData.senderID = displayUser.ID; //we could have gotten on this profile page from another chat by clicking on a notification.
				BackButton_Click(null, null);
				return;
			}

			if (displayUser.UserRelation == 2)
			{
				return;
			}
			else if (displayUser.UserRelation != 3 && displayUser.UserRelation != 4) //not a match yet
			{
				LikeButton.UserInteractionEnabled = false;

				long unixTimestamp = c.Now();
				string responseString = await c.MakeRequest("action=like&ID=" + Session.ID + "&target=" + displayUser.ID
				+ "&time=" + unixTimestamp + "&SessionID=" + Session.SessionID);
				if (responseString.Substring(0, 2) == "OK")
				{
					string matchIDStr = responseString.Substring(3);
					if (matchIDStr != "")
					{
						Session.CurrentMatch = new MatchItem();
						Session.CurrentMatch.MatchID = int.Parse(matchIDStr);
						Session.CurrentMatch.TargetID = displayUser.ID;
						Session.CurrentMatch.TargetUsername = displayUser.Username;
						Session.CurrentMatch.TargetName = displayUser.Name;
						Session.CurrentMatch.TargetPicture = displayUser.Pictures[0];

						displayUser.UserRelation = 3;
						LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Normal);
						LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Highlighted);

						c.CollapseX(HideButton);

						c.DisplayCustomDialog("", LangEnglish.DialogMatch, LangEnglish.DialogYes, LangEnglish.DialogNo, alert => {
							CancelTask();
                            CommonMethods.OpenPage("ChatOneActivity", 1);
						}, null);
					}
					else
					{
						displayUser.UserRelation = 2;
						LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_liked.png"), UIControlState.Normal);
						LikeButton.UserInteractionEnabled = false;

						if (pageType == Constants.ProfileViewType_List)
						{
							NextButton_Click(null, null);
						}
					}
				}
				else
				{
					c.ReportError(responseString);
				}

				LikeButton.UserInteractionEnabled = true;
			}
			else // already a match, opening chat window
			{
				if (pageType == Constants.ProfileViewType_List) //a previously gotten match, we are coming from list, not chat
				{
					LikeButton.UserInteractionEnabled = false;

					string responseString = await c.MakeRequest("action=requestmatchid&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&target=" + displayUser.ID);
					if (responseString.Substring(0, 2) == "OK")
					{
                        if (responseString.Substring(3) == "") //deleted user
                        {
							c.Snack(LangEnglish.MatchNotFound);
							return;
                        }
						Session.CurrentMatch = new MatchItem();
						Session.CurrentMatch.MatchID = int.Parse(responseString.Substring(3));
						Session.CurrentMatch.TargetID = displayUser.ID;
						Session.CurrentMatch.TargetUsername = displayUser.Username;
						Session.CurrentMatch.TargetName = displayUser.Name;
						Session.CurrentMatch.TargetPicture = displayUser.Pictures[0];

						CancelTask();
						CommonMethods.OpenPage("ChatOneActivity", 1);
					}
					else
					{
						c.ReportError(responseString);
					}

					LikeButton.UserInteractionEnabled = true;
				}
				else
				{
					CancelTask();
					CommonMethods.OpenPage("ChatOneActivity", 1);
				}
			}
		}

		private async void HideButton_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			long unixTimestamp = c.Now();
			if (displayUser.UserRelation == 0 || displayUser.UserRelation == 2)
			{
				HideButton.UserInteractionEnabled = false;

				string responseString = await c.MakeRequest("action=hide&ID=" + Session.ID + "&target=" + displayUser.ID
				+ "&time=" + unixTimestamp + "&SessionID=" + Session.SessionID);
				if (responseString == "OK")
				{
					displayUser.UserRelation = 1;
					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_reinstate.png"), UIControlState.Normal);
					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_reinstate.png"), UIControlState.Highlighted);

					c.CollapseX(LikeButton);

					if (Session.ListType != "hid")
					{
						ListActivity.viewProfiles.RemoveAt(ListActivity.viewIndex);
						int listIndex = ListActivity.viewIndex - (ListActivity.absoluteStartIndex - ListActivity.absoluteFirstIndex); //we subtract from viewIndex the number of items that were loaded to add before listProfiles
						{
							if (listIndex >= 0 && listIndex < ListActivity.listProfiles.Count) // check the cases where an item was removed from the below or above added list
							{
								ListActivity.listProfiles.RemoveAt(listIndex);
							}
						}
						
						ListActivity.viewIndex--;
						ListActivity.absoluteIndex--;
						ListActivity.totalResultCount--;
						NextButton_Click(null, null);
					}
				}
				else if (responseString.Substring(0, 6) == "ERROR_") //IsAMatch
				{
					string sex = (displayUser.Sex == 0) ? LangEnglish.SexHer : LangEnglish.SexHim;
					c.Snack(c.GetLang(responseString.Substring(6)).Replace("[name]", displayUser.Name).Replace("[sex]", sex));
				}
				else
                {
                    c.ReportError(responseString);
				}

				HideButton.UserInteractionEnabled = true;
			}
			else if (displayUser.UserRelation == 1) //we are in Hid list
			{
				HideButton.UserInteractionEnabled = false; //repeated queries are OK

				string responseString = await c.MakeRequest("action=unhide&ID=" + Session.ID + "&target=" + displayUser.ID
				+ "&time=" + unixTimestamp + "&SessionID=" + Session.SessionID);
				if (responseString == "OK")
				{
					displayUser.UserRelation = 0;
					c.ExpandX(LikeButton);

					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Normal);
					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Highlighted);
				}
				else
				{
					c.ReportError(responseString);
				}

				HideButton.UserInteractionEnabled = true;
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

		private void MenuReport_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			c.DisplayCustomDialog(LangEnglish.ConfirmAction, LangEnglish.ReportDialogText, LangEnglish.DialogYes, LangEnglish.DialogNo, async alert =>
			{
				string responseString = await c.MakeRequest("action=reportprofileview&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&TargetID=" + displayUser.ID);
				if (responseString.Substring(0, 2) == "OK")
				{
					c.Snack(LangEnglish.UserReported);
				}
				else
				{
					c.ReportError(responseString);
				}
			}, null);			
		}

		private void MenuBlock_Click(object sender, EventArgs e)
		{
			c.HideMenu(MenuLayer, MenuContainer, true);

			c.DisplayCustomDialog(LangEnglish.ConfirmAction, LangEnglish.BlockDialogText, LangEnglish.DialogYes, LangEnglish.DialogNo, async alert =>
			{
				if (IsUpdatingTo(displayUser.ID))
				{
					RemoveUpdatesTo(displayUser.ID);
				}
				if (IsUpdatingFrom(displayUser.ID))
				{
					RemoveUpdatesFrom(displayUser.ID);
				}

				long unixTimestamp = c.Now();
				string responseString = await c.MakeRequest("action=blockprofileview&ID=" + Session.ID + "&SessionID=" + Session.SessionID + "&TargetID=" + displayUser.ID + "&time=" + unixTimestamp);
				if (responseString.Substring(0, 2) == "OK")
				{
					if (pageType == Constants.ProfileViewType_List)
                    {
						ListActivity.viewProfiles.RemoveAt(ListActivity.viewIndex);
						if (ListActivity.viewIndex >= 0 && ListActivity.viewIndex < ListActivity.listProfiles.Count)
						{
							ListActivity.listProfiles.RemoveAt(ListActivity.viewIndex);
						}
						ListActivity.viewIndex--;
						ListActivity.absoluteIndex--;
						ListActivity.totalResultCount--;

						NextButton_Click(null, null);
					}
                    else //standalone
                    {
						if (!(ListActivity.listProfiles is null))
						{
							for (int i = 0; i < ListActivity.listProfiles.Count; i++)
							{
								if (ListActivity.listProfiles[i].ID == displayUser.ID)
								{
									ListActivity.listProfiles.RemoveAt(i);
									break;
								}
							}
						}
						if (!(ListActivity.viewProfiles is null))
						{
							for (int i = 0; i < ListActivity.viewProfiles.Count; i++)
							{
								if (ListActivity.viewProfiles[i].ID == displayUser.ID)
								{
									ListActivity.viewProfiles.RemoveAt(i);
									break;
								}
							}
						}
						IntentData.blockedID = displayUser.ID;
						CommonMethods.OpenPage(null, 0); //Disabled chat window will show, with unmatch date given.
					}					
				}
				else
				{
					c.ReportError(responseString);
				}
			}, null);
		}

		public void AddNewMatch(int senderID, MatchItem item)
		{
			if (pageType != Constants.ProfileViewType_Self && displayUser.ID == senderID)
			{
				Session.CurrentMatch = item;
				ListActivity.viewProfiles[ListActivity.viewIndex].UserRelation = 3;

				LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Normal);
				LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Highlighted);

				c.CollapseX(HideButton);
			}
		}

		public void UpdateStatus(int senderID, bool isMatch, int? matchID)
		{
			if (pageType == Constants.ProfileViewType_List && displayUser.ID == senderID)
			{
				if (isMatch) //start userrelation 2
				{
					Session.CurrentMatch = new MatchItem();
					Session.CurrentMatch.MatchID = matchID;
					Session.CurrentMatch.TargetID = displayUser.ID;
					Session.CurrentMatch.TargetUsername = displayUser.Username;
					Session.CurrentMatch.TargetName = displayUser.Name;
					Session.CurrentMatch.TargetPicture = displayUser.Pictures[0];

					ListActivity.viewProfiles[ListActivity.viewIndex].UserRelation = 3;

					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Normal);
					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Highlighted);

					c.CollapseX(HideButton);
				}
				else //start userrelation 3 or 4.
				{
					ListActivity.viewProfiles[ListActivity.viewIndex].UserRelation = 2;

					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_liked.png"), UIControlState.Normal);
					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_liked.png"), UIControlState.Highlighted);

					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Normal);
					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Highlighted);

					c.ExpandX(HideButton);
				}
			}
            else if (pageType == Constants.ProfileViewType_Standalone)
            {
				AppDelegate.AddUpdateMatch(senderID, isMatch);
            }

            if (pageType == Constants.ProfileViewType_Standalone && displayUser.ID == senderID)
            {
				if (isMatch) //start userrelation 2
				{
					displayUser.UserRelation = 3;

					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Normal);
					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_chat_one.png"), UIControlState.Highlighted);

					c.CollapseX(HideButton);
				}
				else //start userrelation 3 or 4.
				{
					displayUser.UserRelation = 2;

					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_liked.png"), UIControlState.Normal);
					LikeButton.SetBackgroundImage(UIImage.FromBundle("ic_liked.png"), UIControlState.Highlighted);

					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Normal);
					HideButton.SetBackgroundImage(UIImage.FromBundle("ic_hide.png"), UIControlState.Highlighted);

					c.ExpandX(HideButton);
				}
			}
		}

		public void UpdateLocationStart(int senderID, string message)
		{
			if (pageType != Constants.ProfileViewType_Self && displayUser.ID == senderID)
			{
				c.Snack(message);
			}
			else
			{
				c.SnackAction(message, LangEnglish.ShowReceived, new Action(delegate () {
					if (pageType == Constants.ProfileViewType_Self && (bool)Session.UseLocation && c.IsLocationEnabled())
					{
						locMgr.LocationUpdated -= LocMgr_LocationUpdated;
					}

					ProfileImageScroll.ContentOffset = new CoreGraphics.CGPoint(0, 0);
					imageIndex = 0;

					pageType = Constants.ProfileViewType_Standalone;
					LoadStandalone(senderID);
				}));
			}
		}

		public void UpdateLocation(int senderID, long time, double latitude, double longitude)
		{
			if (pageType != Constants.ProfileViewType_Self && displayUser.ID == senderID)
			{
				displayUser.LastActiveDate = time;
				displayUser.Latitude = latitude;
				displayUser.Longitude = longitude;
				displayUser.LocationTime = time;

				LastActiveDate.Text = c.GetTimeDiffStr(time, true);

				CLLocationCoordinate2D location = new CLLocationCoordinate2D(latitude, longitude);
				ProfileViewMap.CenterCoordinate = location;

				if (!(thisMarker is null))
				{
					ProfileViewMap.RemoveAnnotation(thisMarker);
				}
				thisMarker = new MKPointAnnotation() { Title = "Center", Coordinate = location };
				ProfileViewMap.AddAnnotation(thisMarker);

				LocationTime.Text = LangEnglish.ProfileViewLocation + " " + c.GetTimeDiffStr(time, false);
				LocationTime.Hidden = false;

				if (!(displayUser.Distance is null))
				{
					float distance;
                    if (!Constants.SafeLocationMode)
                    {
						distance = CalculateDistance((double)Session.Latitude, (double)Session.Longitude, latitude, longitude);
					}
                    else
                    {
						distance = CalculateDistance((double)Session.LatestLatitude, (double)Session.LatestLongitude, latitude, longitude);
					}
					
					displayUser.Distance = distance;
					DistanceText.Text = distance + " km " + LangEnglish.ProfileViewAway;
				}
			}
		}

		private void LocMgr_LocationUpdated(object sender, LocationUpdatedEventArgs e)
		{
			if (!Constants.SafeLocationMode)
			{
				UpdateLocationSelf();
			}
		}

		public void UpdateLocationSelf()
		{
			LastActiveDate.Text = c.GetTimeDiffStr(Session.LastActiveDate, true);

			CLLocationCoordinate2D location = new CLLocationCoordinate2D((double)Session.Latitude, (double)Session.Longitude);
			ProfileViewMap.CenterCoordinate = location;

			if (!(thisMarker is null))
			{
				ProfileViewMap.RemoveAnnotation(thisMarker);
			}

			thisMarker = new MKPointAnnotation() { Title = "Center", Coordinate = location };
			ProfileViewMap.AddAnnotation(thisMarker);

			if (ProfileViewMap.Frame.Height == 0) //if this is the first location registered, display map
			{
				SetMap();
			}
            else
            {
				LocationTime.Text = LangEnglish.ProfileViewLocation + " " + c.GetTimeDiffStr(Session.LocationTime, false);
			}
			c.Expand(LocationTime);
		}

		private float CalculateDistance(double lat1, double long1, double lat2, double long2)
		{
			return (float)Math.Round(6371 * Math.Acos(
			Math.Cos(Math.PI / 180 * lat1) * Math.Cos(Math.PI / 180 * lat2) * Math.Cos(Math.PI / 180 * long2 - Math.PI / 180 * long1)
			+ Math.Sin(Math.PI / 180 * lat1) * Math.Sin(Math.PI / 180 * lat2)
			), 1);
		}

		private void CancelTask()
		{
			if (!(imageLoading is null) && !imageLoading.IsCompleted)
			{
				cts.Cancel();
			}
		}
	}
}