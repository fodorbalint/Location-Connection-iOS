# Changelog - Location Connection iOS
1.4 - 8 June

New:
- A dark appearance is created for this display mode on iOS 13.
- A border is placed around the thumbnails on the map.
- The back button of the image editor is placed on the left side.
- In the tutorial, you can now tap the left or the right side of a frame to move to the next/previous one.

Bug fixes:
- Notifications are only received if the user logs in again after enabling notifications.
- Tapping the like and hide buttons repeatedly results in error.
- If location cannot be acquired, the program hangs, and only manual refresh will load the list.
- When opening a page from the start with the map visible, upon returning the map's area changes when it should not.
- When pictures are loading on the map, they get enlarged the first time.
- The tile list has scrollable empty space at the bottom when closing/opening the app, and when rotating the device on a profile page.
- The chat window does not refresh when closing the app and opening it.
- If on the registration page the description text contains ;, upon leaving the page and returning to it the text will be cut off.
- When rearranging pictures and dragging them outside their area, they will appear under succeeding form elements.
- If a phone is rotated to landscape, the tutorial will show iPad screenshots.
- When unchecking location/distance sharing with friends, the "No one" option does not get checked.

1.3 - 4 May

A tutorial has been added to the help center.

Bug fixes:
- On iPhone X and possibly newer devices clicking on a chat on the chat list page does not open up the chat, only when pressed very shortly.
- When opening a profile with no distance shown, and navigating to the next which has a distance, its text appears right next to the location time label or aligned left. It should align right at all times.
- When starting location updates, your match is not immediately notified.
- When rotating the device, the list of users is aligned into 2 columns with space between instead of remaining in 3 columns. On the profile edit page, screen rotation does not realign the user's own pictures. Profile view pictures are misaligned too after being scrolled.
- PNG images cannot be uploaded.
- On smaller screens in landscape orientation the login text fields are compressed.
- When pressing the chat button on someone's profile who just deleted their account, the app crashes.
- When logging in on another device, and pressing on the chat list button, an empty page will appear instead of logging the user out immediately.

1.2 - 21 April

An image editor was created for cropping images before uploading.

Bug fixes:
- On devices with rounded corners the navigation buttons cover the profile text if the map is not visible.
- When changing device orientation, and reloading the list, the pictures are either smaller or larger than they should be now.
- Dialog about enabling notifications at program start. This should only come up when a user logs in or registers.
- When no location is set, the refresh indicator (after pulling down the list) does not stop.
- When clicking into the Introduction text field, the popping up keyboard may entirely cover it.
- When pressing the Reset button on the registration page, the page does not scroll to the beginning.
- The chat crashes on iOS 12 when displaying own message.
- When blocking a user from their page opened from the chat window, the app crashes.
- When the map is visible while filtering by distance from current location, turning off location on the profile edit page results in the map disappearing. The correct behavior is to keep the map, but switching to filtering according to address/coordinates.
- On the location history page when the list is empty, waiting for a first update results in a crash.
- When receiving a message with notification permission off, and enabling it afterwards from iOS Settings, upon returning to the app an empty message will appear.

1.1 - 28 March

Push notifications are implemented.

Furthermore, the following bugs have been fixed:
- When a logged in user turns off location, and logs out, location updates does not restart. Location updates are still going after a user logs in who is not using location.
- A user's own pictures appear in small resolution on their page.
- Depending on the network speed, the program may hang at startup for logged-in users, if the results are loading faster than the app's startup time.
- When a user is logged out, notifications are still delivered, and this results in an error.
- For lists containing more than 100 profiles, background loading does not take into consideration that you may hide people from the list.
- When distance filters are open with the map open, switching to search does not remove the circle. (In search mode there is no distance filtering.)
- When you open a your match's profile from the chat page, its status does not update should they unmatch or rematch you.
- When you unmatch from someone, you are still receiving their location updates if they started it.
- When pressing Go in the search field or distance field, pictures on the map are not appearing.
- On tablet, if you enter non-number characters in the distance text field, the app crashes.
- If you enter an invalid address, and then reload the list, the address is not reverted back to the previous valid value.
- When a message appears that no location was set, it does not disappear automatically once it is set.

The default map type is now set to street instead of satellite, for better visibility.
