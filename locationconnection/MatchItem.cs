using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LocationConnection
{
	public class MatchItem
	{
		public int? MatchID;
		public bool? Active;
		public long? MatchDate;
		public long? UnmatchDate;
		public string[] Chat;
		public int? TargetID;
		public byte? Sex;
		public string TargetUsername;
		public string TargetName;
		public string TargetPicture;
		public bool? ActiveAccount;
		public bool? Friend;
	}
}