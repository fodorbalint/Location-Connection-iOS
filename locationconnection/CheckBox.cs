using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LocationConnection
{
	[Register("CheckBox"), DesignTimeVisible(true)]
	public class CheckBox : UIButton
	{
		private bool _Checked;
		public bool Checked { get => GetChecked(); set => SetChecked(value); }

		private bool _Enabled;
		public new bool Enabled { get => GetEnabled(); set => SetEnabled(value); }

		private string controlName;
		private BaseActivity context;

		public CheckBox(IntPtr p) : base(p)
		{
			SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Normal);
			SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Highlighted);
		}

		private void SetChecked(bool value)
		{
			_Checked = value;

			if (_Enabled)
            {
				if (value)
				{
					SetImage(UIImage.FromBundle("CheckboxChecked"), UIControlState.Normal);
					SetImage(UIImage.FromBundle("CheckboxChecked"), UIControlState.Highlighted);
				}
				else
				{
					SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Normal);
					SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Highlighted);
				}
			}
		}

		private bool GetChecked()
		{
			return _Checked;
		}

		private void SetEnabled(bool value)
		{
			_Enabled = value;
			if (value)
			{
                if (_Checked)
                {
					SetImage(UIImage.FromBundle("CheckboxChecked"), UIControlState.Normal);
					SetImage(UIImage.FromBundle("CheckboxChecked"), UIControlState.Highlighted);
				}
                else
                {
					SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Normal);
					SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Highlighted);
				}				
			}
			else
			{
				SetImage(UIImage.FromBundle("CheckboxDisabled"), UIControlState.Normal);
				SetImage(UIImage.FromBundle("CheckboxDisabled"), UIControlState.Highlighted);
			}
		}

		private bool GetEnabled()
		{
			return _Enabled;
		}

		public void SetContext(string controlName, BaseActivity context)
		{
			this.controlName = controlName;
			this.context = context;
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

            if (_Enabled)
            {
				if (!_Checked)
				{
					_Checked = true;
					SetImage(UIImage.FromBundle("CheckboxChecked"), UIControlState.Normal);
					SetImage(UIImage.FromBundle("CheckboxChecked"), UIControlState.Highlighted);
				}
				else
				{
					_Checked = false;
					SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Normal);
					SetImage(UIImage.FromBundle("Checkbox"), UIControlState.Highlighted);
				}
			}
			
		}
	}
}