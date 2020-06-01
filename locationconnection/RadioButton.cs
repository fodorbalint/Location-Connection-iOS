using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LocationConnection
{
	[Register("RadioButton"), DesignTimeVisible(true)]
	public class RadioButton : UIButton
	{
		private bool _Checked;
		public bool Checked { get => GetChecked(); set => SetChecked(value); }
		private string controlName;
		private BaseActivity context;

		public RadioButton(IntPtr p) : base(p)
		{
			SetImage(UIImage.FromBundle("RadioUnchecked"), UIControlState.Normal);
			SetImage(UIImage.FromBundle("RadioUnchecked"), UIControlState.Highlighted);
		}

		private void SetChecked(bool value)
		{
			_Checked = value;
			if (value)
			{
				SetImage(UIImage.FromBundle("RadioChecked"), UIControlState.Normal);
				SetImage(UIImage.FromBundle("RadioChecked"), UIControlState.Highlighted);
			}
			else
			{
				SetImage(UIImage.FromBundle("RadioUnchecked"), UIControlState.Normal);
				SetImage(UIImage.FromBundle("RadioUnchecked"), UIControlState.Highlighted);
			}
		}

		private bool GetChecked() {
			return _Checked;
		}

		public void SetContext(string controlName, BaseActivity context)
		{
			this.controlName = controlName;
			this.context = context;
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			if (!_Checked)
			{
				_Checked = true;
				SetImage(UIImage.FromBundle("RadioChecked"), UIControlState.Normal);
				SetImage(UIImage.FromBundle("RadioChecked"), UIControlState.Highlighted);

				switch (controlName)
				{
					case "UseGeoNo":
						((ListActivity)context).UseGeo_Click(false);
						break;
					case "UseGeoYes":
						((ListActivity)context).UseGeo_Click(true);
						break;
					case "DistanceSourceCurrent":
						((ListActivity)context).DistanceSource_Click(true);
						break;
					case "DistanceSourceAddress":
						((ListActivity)context).DistanceSource_Click(false);
						break;
				}
			}			
		}
	}
}