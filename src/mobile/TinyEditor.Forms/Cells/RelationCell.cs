using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace TinyEditor.Forms.Cells
{
    public class RelationCell : ViewCell
    {
        private StackLayout Root = new StackLayout()
        {
            Orientation = StackOrientation.Horizontal
        };
        public Label TextLabel = new Label()
        {
            HorizontalOptions = LayoutOptions.StartAndExpand,
            VerticalOptions = LayoutOptions.Center
        };

        public Picker PickerView = new Picker()
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center
        };

        private IEditorRelation _relation;

        public RelationCell(IEditorRelation rel)
        {
            _relation = rel;
            View = Root;
            Root.Children.Add(TextLabel);
            Root.Children.Add(PickerView);
            TextLabel.BindingContext = this;
            PickerView.BindingContext = this;
            TextLabel.SetBinding(Label.TextProperty, nameof(Text));
            PickerView.SetBinding(Picker.SelectedItemProperty, nameof(Value));
            PopulateValues();
        }

        public void PopulateValues()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var values = await _relation.GetValues();
                foreach (var val in values)
                {
                    PickerView.Items.Add(val.ToString());
                }
            });
        }


        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(object), typeof(DateCell), null, BindingMode.TwoWay);

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(DateCell), null, BindingMode.TwoWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }


}
