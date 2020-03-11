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
    [Register ("SettingsActivity")]
    partial class SettingsActivity
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch BackgroundLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BackgroundLocationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider BackgroundLocationRate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BackgroundLocationRateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BackgroundLocationRateValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckMatchBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckMatchInApp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckMessageBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckMessageInApp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckRematchBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckRematchInApp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckUnmatchBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.CheckBox CheckUnmatchInApp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.InsetLabel DisplaySection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider InAppLocationRate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel InAppLocationRateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel InAppLocationRateValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LocationHistoryButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.InsetLabel LocationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider MapIconSize { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MapIconSizeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MapIconSizeValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider MapRatio { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MapRatioLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MapRatioValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MessageContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView MessageEdit { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MessageSend { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsInApp { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsNewMatchLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsNewMessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsRematchLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsUnmatchLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ProgramLogButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ProgramLogIncluded { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RippleSettings { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RoundBottom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ScrollBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView Settings_Scroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SettingsBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SettingsFormCaption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SettingsHeaderText { get; set; }

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
            if (BackgroundLocation != null) {
                BackgroundLocation.Dispose ();
                BackgroundLocation = null;
            }

            if (BackgroundLocationLabel != null) {
                BackgroundLocationLabel.Dispose ();
                BackgroundLocationLabel = null;
            }

            if (BackgroundLocationRate != null) {
                BackgroundLocationRate.Dispose ();
                BackgroundLocationRate = null;
            }

            if (BackgroundLocationRateLabel != null) {
                BackgroundLocationRateLabel.Dispose ();
                BackgroundLocationRateLabel = null;
            }

            if (BackgroundLocationRateValue != null) {
                BackgroundLocationRateValue.Dispose ();
                BackgroundLocationRateValue = null;
            }

            if (BottomConstraint != null) {
                BottomConstraint.Dispose ();
                BottomConstraint = null;
            }

            if (CheckMatchBackground != null) {
                CheckMatchBackground.Dispose ();
                CheckMatchBackground = null;
            }

            if (CheckMatchInApp != null) {
                CheckMatchInApp.Dispose ();
                CheckMatchInApp = null;
            }

            if (CheckMessageBackground != null) {
                CheckMessageBackground.Dispose ();
                CheckMessageBackground = null;
            }

            if (CheckMessageInApp != null) {
                CheckMessageInApp.Dispose ();
                CheckMessageInApp = null;
            }

            if (CheckRematchBackground != null) {
                CheckRematchBackground.Dispose ();
                CheckRematchBackground = null;
            }

            if (CheckRematchInApp != null) {
                CheckRematchInApp.Dispose ();
                CheckRematchInApp = null;
            }

            if (CheckUnmatchBackground != null) {
                CheckUnmatchBackground.Dispose ();
                CheckUnmatchBackground = null;
            }

            if (CheckUnmatchInApp != null) {
                CheckUnmatchInApp.Dispose ();
                CheckUnmatchInApp = null;
            }

            if (DisplaySection != null) {
                DisplaySection.Dispose ();
                DisplaySection = null;
            }

            if (InAppLocationRate != null) {
                InAppLocationRate.Dispose ();
                InAppLocationRate = null;
            }

            if (InAppLocationRateLabel != null) {
                InAppLocationRateLabel.Dispose ();
                InAppLocationRateLabel = null;
            }

            if (InAppLocationRateValue != null) {
                InAppLocationRateValue.Dispose ();
                InAppLocationRateValue = null;
            }

            if (LocationHistoryButton != null) {
                LocationHistoryButton.Dispose ();
                LocationHistoryButton = null;
            }

            if (LocationLabel != null) {
                LocationLabel.Dispose ();
                LocationLabel = null;
            }

            if (MapIconSize != null) {
                MapIconSize.Dispose ();
                MapIconSize = null;
            }

            if (MapIconSizeLabel != null) {
                MapIconSizeLabel.Dispose ();
                MapIconSizeLabel = null;
            }

            if (MapIconSizeValue != null) {
                MapIconSizeValue.Dispose ();
                MapIconSizeValue = null;
            }

            if (MapRatio != null) {
                MapRatio.Dispose ();
                MapRatio = null;
            }

            if (MapRatioLabel != null) {
                MapRatioLabel.Dispose ();
                MapRatioLabel = null;
            }

            if (MapRatioValue != null) {
                MapRatioValue.Dispose ();
                MapRatioValue = null;
            }

            if (MessageContainer != null) {
                MessageContainer.Dispose ();
                MessageContainer = null;
            }

            if (MessageEdit != null) {
                MessageEdit.Dispose ();
                MessageEdit = null;
            }

            if (MessageSend != null) {
                MessageSend.Dispose ();
                MessageSend = null;
            }

            if (NotificationsBackground != null) {
                NotificationsBackground.Dispose ();
                NotificationsBackground = null;
            }

            if (NotificationsInApp != null) {
                NotificationsInApp.Dispose ();
                NotificationsInApp = null;
            }

            if (NotificationsLabel != null) {
                NotificationsLabel.Dispose ();
                NotificationsLabel = null;
            }

            if (NotificationsNewMatchLabel != null) {
                NotificationsNewMatchLabel.Dispose ();
                NotificationsNewMatchLabel = null;
            }

            if (NotificationsNewMessageLabel != null) {
                NotificationsNewMessageLabel.Dispose ();
                NotificationsNewMessageLabel = null;
            }

            if (NotificationsRematchLabel != null) {
                NotificationsRematchLabel.Dispose ();
                NotificationsRematchLabel = null;
            }

            if (NotificationsUnmatchLabel != null) {
                NotificationsUnmatchLabel.Dispose ();
                NotificationsUnmatchLabel = null;
            }

            if (ProgramLogButton != null) {
                ProgramLogButton.Dispose ();
                ProgramLogButton = null;
            }

            if (ProgramLogIncluded != null) {
                ProgramLogIncluded.Dispose ();
                ProgramLogIncluded = null;
            }

            if (RippleSettings != null) {
                RippleSettings.Dispose ();
                RippleSettings = null;
            }

            if (RoundBottom != null) {
                RoundBottom.Dispose ();
                RoundBottom = null;
            }

            if (ScrollBottomConstraint != null) {
                ScrollBottomConstraint.Dispose ();
                ScrollBottomConstraint = null;
            }

            if (Settings_Scroll != null) {
                Settings_Scroll.Dispose ();
                Settings_Scroll = null;
            }

            if (SettingsBack != null) {
                SettingsBack.Dispose ();
                SettingsBack = null;
            }

            if (SettingsFormCaption != null) {
                SettingsFormCaption.Dispose ();
                SettingsFormCaption = null;
            }

            if (SettingsHeaderText != null) {
                SettingsHeaderText.Dispose ();
                SettingsHeaderText = null;
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