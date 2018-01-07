using System;
using Xamarin.Forms;

namespace TinyEditor.Forms.Cells
{
    public class DateCell : ViewCell
    {
        private StackLayout Root = new StackLayout()
        {
            Orientation = StackOrientation.Horizontal,
            Padding = new Thickness(10,3)
        };

        public Label TextLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.StartAndExpand,
            VerticalOptions = LayoutOptions.Center
        };

        public DatePicker DatePickerView = new DatePicker()
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center
        };

        public DateCell()
        {
            View = Root;
            Root.Children.Add(TextLabel);
            Root.Children.Add(DatePickerView);
            TextLabel.BindingContext = this;
            DatePickerView.BindingContext = this;
            TextLabel.SetBinding(Label.TextProperty, nameof(Text));
            DatePickerView.SetBinding(DatePicker.DateProperty, nameof(Value));

        }

        public DateTime Value
        {
            get;
            set;
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(DateCell), null, BindingMode.TwoWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create("Value", typeof(DateTime), typeof(DateCell), DateTime.Now, propertyChanged: InitValue);

        private static void InitValue(BindableObject bindable, object oldValue, object newValue)
        {
            var that = bindable as DateCell;
            if (that != null && newValue is DateTime dt)
                that.Value = dt;
        }
    }
}
