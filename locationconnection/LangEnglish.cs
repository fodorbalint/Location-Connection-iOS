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
        public const string versionInfo = "Version: 28. March 2020\n\nCreated by Balint Fodor\n\nContact: fodorbalint@gmail.com\nlocationconnection.me\n\nPrivacy policy:\nhttps://locationconnection.me/?page=legal#privacy\n\nSource code:\nhttps://github.com/fodorbalint?tab=repositories";

        public const string ConfirmAction = "Confirm action";
        public const string DialogOK = "OK";
        public const string DialogCancel = "Cancel";
        public const string DialogYes = "Yes";
        public const string DialogNo = "No";
        public const string SnackOK = "OK";

        public const string ShowReceived = "Show";
        public const string LocationUpdatesFromStart = "is now sending you location updates every";
        public const string LocationUpdatesFromEnd = "has stopped the location updates.";

        public const string MenuReport = "Report...";
        public const string MenuBlock = "Block...";
        public const string UserReported = "User reported.";

        public const string ReportDialogText = "This will send a nofitication to the developer, who will review it, and take action if necessary.\nAre you sure you want to continue?";
        public const string BlockDialogText = "This will permanently hide the user while you are logged in to your account.\nThey will not show up in search lists, nor can you find them on the list of hidden people.\nThis action cannot be undone.\nAre you sure you want to continue?";

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
        public const string EmailExplanation = "A confirmation email will be sent. Should you ever forget your password, you can use this e-mail to reset it.";
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
        public const string UsernameBackslash = "Last character of username cannot be \\";
        public const string NameBackslash = "Last character of name cannot be \\";
        public const string DescriptionBackslash = "Last character of introduction cannot be \\";

        public const string UsernameAvailable = "Username available.";

        public const string ImageEditorLabel = "Align the image with the frame to crop it. Use two fingers to zoom.";

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
        public const string Deactivated = "Your account is now inactive.";
        public const string Activated = "Your account is now active.";

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
        public const string MatchNotFound = "Match not found.";
        public const string UserNotAvailable = "User not available.";

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

        public const string UseGeoNo = "No location filter";
        public const string UseGeoYes = "Within distance from:";
        public const string DistanceSourceCurrent = "Current location";
        public const string DistanceSourceAddress = "Address/coordinates";
        public const string DistanceUnit = "km";
        public const string GeoFilterNoLocation = "No location was set.\nTurn off geographic filtering, acquire current location, or set an address.";
        public const string LocationNotInUse = "Location is not in use.";
        public const string WrongNumberFormat = "Wrong number format.";
        public const string MinDistanceMessage = "Distance must be at least 1 km.";
        public const string MaxDistanceMessage = "The maximum distance is 20015 km.";
        public const string MapStreet = "Street";
        public const string MapSatellite = "Satellite";

        public const string ConvertingAddress = "Converting address...";
        public const string AddressNoResult = "Address not found.";
        public const string OverQueryLimit = "Query limit reached.\nConverting address to coordinates is a paid service, read about current limitations in the Help Center.\nTry again later, or enter coordinates.";
        public const string LoggingIn = "Logging in...";
        public const string GettingLocation = "Getting location...";
        public const string LoadingList = "Loading...";
        public const string NoResult = "Nobody matched your search criteria.";
        public const string ResultsCount = "[num] results.";
        public const string ResultCount = "1 result.";
        public const string ResultSetInterval = "[start] - [end] of";
        public const string SettingMap = "Setting map...";
        public const string NoLocationSet = "Location was not set or acquired.";
        public const string MapViewNoLocation = "Showing map requires location access, or you need to set an address.\n\nDo you want to turn on location now?";
        public const string MapViewNoUseLocation = "Do you want to enable location?";
        public const string LocationDisabledButUsingLocation = "Your setting to use location is on, but permission is not granted.\nLocation is not acquired.";
        public const string LocationNotGranted = "Location permission was not granted.";

        public const string NoNetwork = "No Internet connection.";
        public const string NetworkTimeout = "Internet connection timed out.";
        public const string ErrorEncountered = "Error encountered";
        public const string ErrorNotificationSent = "Developer has been notified.";
        public const string ErrorNotificationToSend = "Developer will be notified on app restart.";

        // Profile View

        public const string SendLocation = "Update location";
        public const string EditSelf = "Edit profile";
        public const string ShortSecond = "sec";
        public const string ShortSeconds = "sec";
        public const string Second = "second";
        public const string Seconds = "seconds";
        public const string ShortMinute = "min";
        public const string ShortMinutes = "min";
        public const string Minute = "minute";
        public const string Minutes = "minutes";
        public const string Hour = "hour";
        public const string Hours = "hours";
        public const string Day = "day";
        public const string Days = "days";
        public const string Ago = "ago";
        public const string Now = "Now";
        public const string NowSmall = "now";

        public const string ProfileViewLocation = "Location";
        public const string ProfileViewDistance = "Distance";
        public const string ProfileViewAway = "away";

        public const string LocationUpdated = "Location updated.";
        public const string LocationNotUpdated = "Location could not be updated.";
        public const string LocationNotAcquired = "Location is not yet acquired.";

        public const string IsAMatch = "To hide [name], unmatch [sex] first.";
        public const string SexHim = "him";
        public const string SexHer = "her";

        public static string DialogMatch = "It is a match!" + Environment.NewLine + "Chat now?";

        // Chat List

        public const string NoMatch = "You don't have any matches.";
        public const string ChatListMatch = "match.";
        public const string ChatListMatches = "matches.";

        // Chat One

        public static string ChatOneEnableNotifications = "Enabling notifications is necessary for instant messaging." + Environment.NewLine + "Do you want to give permission now?" + Environment.NewLine + Environment.NewLine + "In-app and background notification preferences can be changed in Settings.";
        public const string DialogDontAsk = "Don't ask again";
        public const string DialogGoToSettings = "Go to Settings";

        public const string ChatOneDataLoading = "Wait until data is loaded.";        
        public const string Matched = "Matched";
        public const string Unmatched = "Unmatched";

        public const string MenuStartLocationUpdates = "Start sending location updates";
        public const string MenuStopLocationUpdates = "Stop sending location updates";
        public const string MenuAddFriend = "Add to friends";
        public const string MenuRemoveFriend = "Remove from friends";
        public const string MenuUnmatch = "Unmatch";

        public const string ChangeUpdateCriteria = "For real-time updates, location refresh frequency must be at most 60 seconds. Apply this setting now?";
        public const string LocationUpdatesToStart = "Location updates started.";
        public const string LocationUpdatesToEnd = "Location updates stopped.";
        public const string FriendAdded = "Friend added.";
        public const string FriendRemoved = "Friend removed.";

        public const string DialogUnmatchMatched = "Are you sure you want to unmatch from [name]? If [sex] unmatches you, your conversations will not be recoverable.";
        public const string DialogUnmatchUnmatched = "Are you sure you want to unmatch from [name]? Since [sex] already unmatched you, your conversation history will be deleted.";
        public const string DialogUnmatchDeleted = "Are you sure you want to unmatch? Your conversation history will be deleted.";
        public const string SexHe = "he";
        public const string SexShe = "she";

        public const string NoMessages = "No messages yet.";
        public const string MessageStatusSent = "Sent";
        public const string MessageStatusSeen = "Seen";
        public const string MessageStatusRead = "Read";

        public const string EnableLocationLevelMatch = "To send location to [name], enable location sharing with matches, or add [sex] to your friends.";
        public const string EnableLocationLevelFriend = "To send location to [name], enable location sharing with friends.";

        // Location history

        public const string NoLocationRecords = "No records yet.";

        // Help center

        public const string HelpCenterFormCaption = "Ask a question or send feedback";
        public const string HelpCenterFormSend = "Send";
        public const string HelpCenterSent = "Message sent.";

        // Settings

        public const string NotificationsText = "Notifications";
        public const string NotificationsInApp = "App in use";
        public const string NotificationsBackground = "Background";
        public const string NotificationsNewMatchText = "New match";
        public const string NotificationsNewMessageText = "New message";
        public const string NotificationsUnmatchText = "Unmatch";
        public const string NotificationsRematchText = "Re-match";

        public const string DisplaySection = "Display";
        public const string MapIconSizeText = "Profile icon size on map";
        public const string MapRatioText = "Map height on profile page";

        public const string LocationText = "Location";
        public const string BackgroundLocation = "Update in background";
        public const string InAppLocationRate = "In-app refresh frequency";
        public const string InAppLocationRateExplanation = "Used for your device's location history, and determines the frequency for sending location updates to a match.";
        public const string BackgroundLocationRate = "Background refresh frequency";

        public const string LocationHistory = "See your 24-hour location history";

        public const string SettingsFormCaption = "Report an error";
        public const string SettingsFormSend = "Send";
        public const string ProgramLogIncluded = "System log attached.";
        public const string SeeProgramLog = "See log";
        public const string SettingsSent = "Message sent.";
    }
}