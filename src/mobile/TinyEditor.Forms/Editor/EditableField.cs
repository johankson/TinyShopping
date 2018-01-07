using System;
using System.ComponentModel;
using System.Reflection;
using TinyEditor.Forms.Cells;
using Xamarin.Forms;

namespace TinyEditor
{
    public class EditableField : INotifyPropertyChanged
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
            PropertyData = attr;
        }

        public EditorAttribute PropertyData { get; internal set; }

        private Cell view;

        public event PropertyChangedEventHandler PropertyChanged;

        public Cell EditorCell
        {
            get
            {
                if (parent is INotifyPropertyChanged pchange)
                {
                    pchange.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName==SourceProperty.Name) {
                            PropertyChanged.Invoke(this,new PropertyChangedEventArgs("Value"));
                        }
                    };
                }
                if (view == null)
                {
                    if (PropertyData.Readonly)
                    {
                        view = new TextCell()
                        {
                            Text = PropertyData.Title,
                            BindingContext = this,
                            Detail = Value.ToString()
                        };
                        view.SetBinding(TextCell.DetailProperty, nameof(Value));

                    }
                    else
                    {
                        if (SourceProperty.PropertyType == typeof(string))
                        {
                            view = new EntryCell()
                            {
                                Label = PropertyData.Title
                            };
                            view.SetBinding(EntryCell.TextProperty, nameof(Value));
                        }
                        if (SourceProperty.PropertyType == typeof(bool))
                        {
                            view = new SwitchCell()
                            {
                                Text = PropertyData.Title
                            };
                            view.SetBinding(SwitchCell.OnProperty, nameof(Value));
                        }
                        else if (SourceProperty.PropertyType == typeof(DateTime))
                        {
                            view = new DateCell()
                            {
                                Text = PropertyData.Title
                            };
                            view.SetBinding(DateCell.ValueProperty, nameof(Value));
                        }
                        if (view != null)
                            view.BindingContext = this;

                    }

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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }


        internal PropertyInfo SourceProperty { get; set; }
    }
}

