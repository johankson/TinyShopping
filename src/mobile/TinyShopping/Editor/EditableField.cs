using System;
using System.Reflection;
using Xamarin.Forms;

namespace TinyShopping.Controls
{
    public class EditableField
    {
        public EditableField()
        {

        }

        private object parent;

        public EditableField(PropertyInfo propery, object parent)
        {
            this.parent = parent;
            var attr = propery.GetEditorAttribute();
            SourceProperty = propery;
            Group = attr.Group;
            Title = attr.Title;
            Excluded = attr.Excluded;
        }

        private Cell view;
        public Cell EditorCell
        {
            get
            {
                if (view == null)
                {
                    view = new EntryCell()
                    {
                        Label = Title,
                    };

                    if (SourceProperty.PropertyType == typeof(bool))
                    {
                        view = new SwitchCell()
                        {
                            Text = Title
                        };
                        view.SetBinding(SwitchCell.OnProperty, nameof(Value));
                    }
                    else if (SourceProperty.PropertyType == typeof(DateTime))
                    {
                        var vc = new ViewCell()
                        {
                        };
                        view = vc;
                        vc.View = new DatePicker();
                    }
                    view.BindingContext = this;
                    if (view is EntryCell)
                        view.SetBinding(EntryCell.TextProperty, nameof(Value));
                        
                }
                return view;
            }
        }

        public object Value
        {
            get
            {
                return SourceProperty.GetValue(parent, null);
            }

            set
            {
                SourceProperty.SetValue(parent, value);
            }
        }

        public string Title { get; set; }
        public string Placeholder { get; set; }
        public bool Excluded { get; set; }
        internal string Group { get; set; }
        internal PropertyInfo SourceProperty { get; set; }
    }
}

