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
    [Register ("LocationHistoryListCell")]
    partial class LocationHistoryListCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        LocationConnection.InsetLabel LocationHistory_Label { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LocationHistory_Label != null) {
                LocationHistory_Label.Dispose ();
                LocationHistory_Label = null;
            }
        }
    }
}