using System;
using System.ComponentModel;
using System.Reflection;
using TinyEditor.Controls;
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

        public EditableField(PropertyInfo propery, object parent, IEditorGenerator generator)
        {
            this.parent = parent;
            Generator = generator;
            var attr = propery.GetEditorAttribute();
            SourceProperty = propery;
            PropertyData = attr;
        }

        public EditorAttribute PropertyData { get; internal set; }

        private Element view;

        public event PropertyChangedEventHandler PropertyChanged;

        public Element EditorView
        {
            get
            {
                if (view==null) {
                    if (parent is INotifyPropertyChanged pchange)
                    {
                        pchange.PropertyChanged += (sender, e) =>
                        {
                            if (e.PropertyName == SourceProperty.Name)
                            {
                                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Value"));
                            }
                        };
                    }
                    view = Generator.GetEditor(this, parent);
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
        public IEditorGenerator Generator { get; internal set; }
    }
}

