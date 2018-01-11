using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace TinyEditor.Controls
{

    public class ObjectEditor : ContentView
    {
        public static readonly BindableProperty DataItemProperty =
            BindableProperty.Create("DataItem", typeof(object), typeof(object), null, propertyChanged: InitObject);

        private static void InitObject(BindableObject bindable, object oldValue, object newValue)
        {
            var that = bindable as ObjectEditor;
            if (that != null && newValue != null)
                that.DataItem = newValue;
        }

        public IEditorGenerator Generator { get; set; }

        private static Func<string, string> DefaultTranslator => (arg) => { return arg; };

        public static Func<string, string> Translate { get; set; } = DefaultTranslator;

        private static bool CheckBook(BindableObject bindable, object value)
        {
            var that = bindable as ObjectEditor;
            return value != null && value is bool;
        }

        object dataItem;
        public object DataItem
        {
            get
            {
                return dataItem;
            }

            set
            {
                if (dataItem != value)
                {
                    dataItem = value;
                    PopulateFields();
                }
            }
        }

        private void PopulateFields()
        {
            Content = null;
            Content = Generator.GenerateEditor(ref dataItem);
        }


    }
}

