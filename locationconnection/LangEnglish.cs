using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LocationConnection
{
	class LangEnglish
    {
        public const string versionInfo = "Version: 15. March 2020\n\nCreated by Balint Fodor\n\nContact: fodorbalint@gmail.com\nlocationconnection.me\n\nPrivacy policy:\nhttps://locationconnection.me/?page=privacy\n\nSource code:\nhttps://github.com/fodorbalint?tab=repositories\n\n";

        public static string ConfirmAction = "Confirm action";
        public static string DialogOK = "OK";
        public static string DialogCancel = "Cancel";
        public static string DialogYes = "Yes";
        public static string DialogNo = "No";
        public static string SnackOK = "OK";

        public static string ShowReceived = "Show";
        public static string LocationUpdatesFromStart = "is now sending you location updates every";
        public static string LocationUpdatesFromEnd = "has stopped the location updates.";

        public const string MenuReport = "Report...";
        public const string MenuBlock = "Block...";
        public const string UserReported = "User reported.";

        public const string BlockDialogText = "This will permanently hide the user while you are logged in to your account.\nThey will not show up in search lists, nor can you find them on the list of hidden people.\nThis action cannot be undone.\nAre you sure you want to continue?";
        public const string ReportDialogText = "This will send a nofitication to the developer, who will review it, and take action if necessary.\nAre you sure you want to continue?";

        // Main

        public const string LogoText = "Location Connection";
        public const string LoginEmail = "Email/username:";
        public const string LoginPassword = "Password:";
        public const string LoginDone = "Login";
        public const string ResetPassword = "Forgot your password?";
        public const string ResetEmail = "Enter your email:";
        public const string ResetSend = "Send reset link";
        public const string RegisterButton = "Register";
        public const string ListButton = "See people nearby";

        public const string ResetEmailSent = "If the address has an account, email has been sent.";
        public const string LoginEmailEmpty = "Username must be filled in.";
        public const string LoginPasswordShort = "Password must be at least 6 characters long.";

        // Register

        public const string Sex = "I am a:";
        public static readonly string[] SexEntries = new string[] {
            "Please select...",
            "Woman",
            "Man"
        };
        public const string Email = "Email:";
        public const string Password = "Password:";
        public const string ConfirmPassword = "Confirm password:";
        public const string Username = "Username:";
        public const string CheckAvailability = "Check availability";
        public const string Name = "Name:";
        public const string Images = "Select images";
        public const string Description = "Introduction:";
        public const string UseLocation = "Use location";
        public const string LocationExplanation = "Location enables you to find people in your area.\nWhen sharing your location with others, you will have to manually update it on your profile page every time you want to make changes.\nThese settings can be changed later on your profile edit page.";

        public const string LocationShare = "Share my location with:";
        public const string LocationShareAll = "Everyone";
        public const string LocationShareLike = "People I liked";
        public const string LocationShareMatch = "People I matched with";
        public const string LocationShareFriend = "My friends";
        public const string LocationShareNone = "No one";

        public const string DistanceShare = "Share my distance with:";
        public const string DistanceShareAll = "Everyone";
        public const string DistanceShareLike = "People I liked";
        public const string DistanceShareMatch = "People I matched with";
        public const string DistanceShareFriend = "My friends";
        public const string DistanceShareNone = "No one";

        public const string EulaLabel = "By registering, I agree to the Terms of Use below:";
        public const string EulaText = "Loading...";

        public const string Register = "Register";
        public const string Reset = "Reset";
        public const string Cancel = "Cancel";

        public const string ImagesUploading = "Wait until the picture finished uploading.";
        public const string ImagesDeleting = "Wait until picture deletion is finished.";
        public const string ImagesProgressText = "Uploading... (This may take up to a minute.)";
        public const string ImagesProgressTextPercent = "Uploading:";
        public const string ImagesRearrange = "Press and hold an image to rearrange.";

        public const string MaxNumImages = "The maximum number of pictures is";
        public const string ImageExists = "This image already exists.";
        public const string SexEmpty = "Please specify your sex.";
        public const string EmailEmpty = "Email must be filled in.";
        public const string EmailWrong = "Wrong email format";
        public const string PasswordShort = "Password must be at least 6 characters long.";
        public const string ConfirmPasswordNoMatch = "Passwords do not match.";
        public const string UsernameEmpty = "Username must be filled in.";
        public const string NameEmpty = "Name must be filled in.";
        public const string ImagesEmpty = "At least one image must be uploaded.";
        public const string DescriptionEmpty = "Introduction must be filled in.";

        public const string UsernameAvailable = "Username available.";

        //Profile Edit

        public const string EditSex = "I want to find";
        public const string Women = "Women";
        public const string Men = "Men";

        public const string EditAccountData = "Edit account details";
        public const string EditChangePassword = "Change password";
        public const string EditLocationSettings = "Change location settings";
        public const string EditMoreOptions = "More options";

        public const string EditOldPassword = "Current password:";
        public const string EditNewPassword = "New password:";
        public const string EditConfirmPassword = "Confirm new password:";

        public const string EditSave = "Save";
        public const string DeactivateAccount = "Deactivate your account";
        public const string ActivateAccount = "Activate your account";
        public const string DeleteAccount = "Delete your account";

        public const string LastImageToDelete = "Last picture cannot be deleted.";
        public const string UsernameSame = "Username not changed.";
        public const string PasswordNotChanged = "New password is same as old.";
        public const string SettingsUpdated = "Update successful.";

        public const string DialogDeactivate = "Are you sure you want to deactivate your account?\n\nIt will no longer be visible and searchable.\nYour matches will still see your name and your small picture, but they neither can go into your page.\nYou can still like or hide people, and get a match.\n\nYour account can be activated again anytime.";
        public const string DialogDelete = "Are you sure you want to delete your account?\nThis cannot be undone.";
        public const string Deactivated = "Your account is now inactive";
        public const string Activated = "Your account is now active";

        //User-fault error messages from server

        public const string LoginFailed = "Login failed.";
        public const string LoggedOut = "You logged in on another device.";
        public const string UsernameExists = "Username already exists.";
        public const string EmailExists = "Email already exists.";
        public const string WrongImageExtension = "Only jpg, jpeg and png files are allowed.";
        public const string NotAnImage = "The uploaded file is not an image.";
        public const string PictureTooLarge = "The uploaded image exceeds the 20 MB limit.";
        public const string PasswordMismatch = "Incorrect password. Log out to reset it.";
        public const string UserPassive = "[name] is not active.";

        // List

        public const string NotLoggedIn = "Not logged in.";
        public const string MenuLogOut = "Log out";
        public const string MenuLogIn = "Log in";
        public const string MenuRegister = "Register";
        public const string MenuSettings = "Settings";
        public const string MenuLocation = "Location history";
        public const string MenuHelpCenter = "Help Center";
        public const string MenuAbout = "About";

        public const string ListType = "List:";
        public const string SortBy = "Sort by:";

        public static readonly string[] SearchInEntries = new string[] {
            "All",
            "Username",
            "Name",
            "Introduction"
        };
        public static readonly string[] SearchInEntries_values = new string[] {
            "all",
            "username",
            "name",
            "bio"
        };
        public static readonly string[] ListTypeEntries = new string[] {
            "All",
            "Undecided",
            "Friends",
            "Matches",
            "Liked",
            "Liked by",
            "Hid",
            "Hid by"
        };
        public static readonly string[] ListTypeEntries_values = new string[] {
            "public",
            "undecided",
            "friends",
            "matches",
            "liked",
            "likedby",
            "hid",
            "hidby"
        };
        public static readonly string[] ListTypeEntriesNotLoggedIn = new string[] {
            "All",
            "Women",
            "Men"
        };
        public static readonly string[] ListTypeEntriesNotLoggedIn_values = new string[] {
            "public",
            "women",
            "men"
        };

        public static string UseGeoNo = "No location filter";
        public static string UseGeoYes = "Within distance from:";
        public static string DistanceSourceCurrent = "Current location";
        public static string DistanceSourceAddress = "Address/coordinates";
        public static string DistanceUnit = "km";
        public static string GeoFilterNoLocation = "No location was set.\nTurn off geographic filtering, acquire current location, or set an address.";
        public static string LocationNotInUse = "Location is not in use.";
        public static string MinDistanceMessage = "Distance must be at least 1 km.";
        public static string MaxDistanceMessage = "The maximum distance is 20015 km.";
        public static string MapStreet = "Street";
        public static string MapSatellite = "Satellite";

        public static string ConvertingAddress = "Converting address...";
        public static string AddressNoResult = "Address not found.";
        public static string OverQueryLimit = "Query limit reached.\nConverting address to coordinates is a paid service, read about current limitations in the Help Center.\nTry again later, or enter coordinates.";
        public static string LoggingIn = "Logging in...";
        public static string GettingLocation = "Getting location...";
        public static string LoadingList = "Loading...";
        public static string NoResult = "Nobody matched your search criteria.";
        public static string ResultsCount = "[num] results.";
        public static string ResultCount = "1 result.";
        public static string ResultSetInterval = "[start] - [end] of";
        public static string SettingMap = "Setting map...";
        public static string NoLocationSet = "Location was not set or acquired.";
        public static string MapViewNoLocation = "Showing map requires location access, or you need to set an address.\n\nDo you want to turn on location now?";
        public static string MapViewNoUseLocation = "Do you want to enable location?";
        public static string LocationDisabledButUsingLocation = "Your setting to use location is on, but permission is not granted.\nLocation is not acquired.";
        public static string LocationNotGranted = "Location permission was not granted.";

        public static string NoNetwork = "No Internet connection.";
        public static string NetworkTimeout = "Internet connection timed out.";
        public static string ErrorEncountered = "Error encountered";
        public static string ErrorNotificationSent = "Developer has been notified.";
        public static string ErrorNotificationToSend = "Developer will be notified on app restart.";

        // Profile View

        public static string SendLocation = "Update location";
        public static string EditSelf = "Edit profile";
        public static string ShortSecond = "sec";
        public static string ShortSeconds = "sec";
        public static string Second = "second";
        public static string Seconds = "seconds";
        public static string ShortMinute = "min";
        public static string ShortMinutes = "min";
        public static string Minute = "minute";
        public static string Minutes = "minutes";
        public static string Hour = "hour";
        public static string Hours = "hours";
        public static string Day = "day";
        public static string Days = "days";
        public static string Ago = "ago";
        public static string Now = "Now";
        public static string NowSmall = "now";

        public static string ProfileViewLocation = "Location";
        public static string ProfileViewDistance = "Distance";
        public static string ProfileViewAway = "away";

        public static string LocationUpdated = "Location updated.";
        public static string LocationNotUpdated = "Location could not be updated.";
        public static string LocationNotAcquired = "Location is not yet acquired.";

        public static string IsAMatch = "To hide [name], unmatch [sex] first.";
        public static string SexHim = "him";
        public static string SexHer = "her";

        public static string DialogMatch = "It is a match!" + Environment.NewLine + "Chat now?";

        // Chat List

        public static string NoMatch = "You don't have any matches.";
        public static string ChatListMatch = "match.";
        public static string ChatListMatches = "matches.";

        // Chat One

        public static string ChatOneEnableNotifications = "Enabling notifications is necessary for instant messaging." + Environment.NewLine + "Do you want to give permission now?" + Environment.NewLine + Environment.NewLine + "In-app and background notification preferences can be changed in Settings.";
        public static string DialogDontAsk = "Don't ask again";
        public static string DialogGoToSettings = "Go to Settings";

        public static string MatchNotFound = "Match not found.";
        public static string ChatOneDataLoading = "Wait until data is loaded.";        
        public static string Matched = "Matched";
        public static string Unmatched = "Unmatched";

        public static string MenuStartLocationUpdates = "Start sending location updates";
        public static string MenuStopLocationUpdates = "Stop sending location updates";
        public static string MenuAddFriend = "Add to friends";
        public static string MenuRemoveFriend = "Remove from friends";
        public static string MenuUnmatch = "Unmatch";

        public static string ChangeUpdateCriteria = "For real-time updates, location refresh frequency must be at most 60 seconds. Apply this setting now?";
        public static string LocationUpdatesToStart = "Location updates started.";
        public static string LocationUpdatesToEnd = "Location updates stopped.";
        public static string FriendAdded = "Friend added.";
        public static string FriendRemoved = "Friend removed.";

        public static string DialogUnmatchMatched = "Are you sure you want to unmatch from [name]? If [sex] unmatches you, your conversations will not be recoverable.";
        public static string DialogUnmatchUnmatched = "Are you sure you want to unmatch from [name]? Since [sex] already unmatched you, your conversation history will be deleted.";
        public static string DialogUnmatchDeleted = "Are you sure you want to unmatch? Your conversation history will be deleted.";
        public static string SexHe = "he";
        public static string SexShe = "she";

        public static string NoMessages = "No messages yet.";
        public static string MessageStatusSent = "Sent";
        public static string MessageStatusSeen = "Seen";
        public static string MessageStatusRead = "Read";

        public static string EnableLocationLevelMatch = "To send location to [name], enable location sharing with matches, or add [sex] to your friends.";
        public static string EnableLocationLevelFriend = "To send location to [name], enable location sharing with friends.";

        // Location history

        public static string NoLocationRecords = "No records yet.";

        // Help center

        public static string HelpCenterFormCaption = "Ask a question or send feedback";
        public static string HelpCenterFormSend = "Send";
        public static string HelpCenterSent = "Message sent.";

        // Settings

        public static string NotificationsText = "Notifications";
        public static string NotificationsInApp = "App in use";
        public static string NotificationsBackground = "Background";
        public static string NotificationsNewMatchText = "New match";
        public static string NotificationsNewMessageText = "New message";
        public static string NotificationsUnmatchText = "Unmatch";
        public static string NotificationsRematchText = "Re-match";

        public static string DisplaySection = "Display";
        public static string MapIconSizeText = "Profile icon size on map";
        public static string MapRatioText = "Map height on profile page";

        public static string LocationText = "Location";
        public static string BackgroundLocation = "Update in background";
        public static string InAppLocationRate = "In-app refresh frequency";
        public static string InAppLocationRateExplanation = "Used for your device's location history, and determines the frequency for sending location updates to a match.";
        public static string BackgroundLocationRate = "Background refresh frequency";

        public static string LocationHistory = "See your 24-hour location history";

        public static string SettingsFormCaption = "Report an error";
        public static string SettingsFormSend = "Send";
        public static string ProgramLogIncluded = "System log attached.";
        public static string SeeProgramLog = "See log";
        public static string SettingsSent = "Message sent.";
    }
}