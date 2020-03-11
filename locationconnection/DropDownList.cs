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
        //private UILabel label;
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

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (component == 0)
                return entries[row];
            else
                return row.ToString();
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            //label.Text = $": {entries[pickerView.SelectedRowInComponent(0)]}, {pickerView.SelectedRowInComponent(1)}";
            switch(pickerName)
            {
                case "SearchIn":
                    Console.WriteLine("----------picker: searchin");
                    ((ListActivity)context).SearchIn_ItemSelected();
                    break;
                case "ListType":
                    Console.WriteLine("----------picker: listtype");
                    ((ListActivity)context).ListType_ItemSelected();
                    break;
                case "Sex":
                    //((RegisterActivity)context).Sex_ItemSelected();
                    break;

            }
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
            if (component == 0)
                return width + 5;
            else
                return width + 5;//repeated label shows on the right below this value.
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
            //return base.GetView(pickerView, row, component, view);
        }
    }
}