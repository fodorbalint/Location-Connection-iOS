using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace LocationConnection
{
    public class DropDownList : UIPickerViewModel
    {
        private string[] entries;
        public string pickerName;
        private float width;
        private BaseActivity context;

        public DropDownList(string[] entries, string pickerName, float width, BaseActivity context)
        {
            this.entries = entries;
            this.pickerName = pickerName;
            this.width = width;
            this.context = context;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 2;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return entries.Length;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            switch(pickerName)
            {   
                case "SearchIn":
                    ((ListActivity)context).SearchIn_ItemSelected();
                    break;
                case "ListType":
                    ((ListActivity)context).ListType_ItemSelected();
                    break;

            }
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            return width + 5; //repeated list appears on the right side below this value. Compnent may be 0 or 1, but only component == 0 has effect.
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 25f;
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            UILabel label = new UILabel(new RectangleF(0, 0, width, 25f));
            label.TextColor = UIColor.Black;
            label.Font = UIFont.SystemFontOfSize(16f);
            label.Text = entries[row];
            label.TextAlignment = UITextAlignment.Left;
            return label;
        }
    }
}