using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace TinyEditor.Controls
{

    public class ObjectEditor : TableView
    {
        public static readonly BindableProperty DataItemProperty =
            BindableProperty.Create("DataItem", typeof(object), typeof(object), null, propertyChanged: InitObject);

        private static void InitObject(BindableObject bindable, object oldValue, object newValue)
        {
            var that = bindable as ObjectEditor;
            if (that != null && newValue != null)
                that.DataItem = newValue;
        }

        public static readonly BindableProperty ShowExcudedAsTextProperty =
            BindableProperty.Create("ShowExcudedAsText", typeof(bool), typeof(object), false, propertyChanged: ChangeShowExcluded, validateValue: CheckBook);

        private static void ChangeShowExcluded(BindableObject bindable, object oldValue, object newValue)
        {
            var that = bindable as ObjectEditor;
            if (newValue is bool b)
            {
                if (that.ShowExcudedAsText != b)
                {
                    that.ShowExcudedAsText = b;
                    if (that.DataItem != null)
                        that.PopulateFields();
                }
            }
        }

        private static Func<string, string> DefaultTranslator => (arg) => { return arg; };

        public static Func<string, string> Translate { get; set; } = DefaultTranslator;

        private static bool CheckBook(BindableObject bindable, object value)
        {
            var that = bindable as ObjectEditor;
            return value != null && value is bool;
        }

        public bool ShowExcudedAsText
        {
            get;
            set;
        } = false;

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

        private FieldGroup GetGroup(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = ObjectEditor.Translate("Extra");
            var ret = FieldGroups.FirstOrDefault(d => d.Name.Equals(name));
            if (ret == null)
            {
                ret = new FieldGroup()
                {
                    Name = name
                };
                ret.View = new TableSection()
                {
                    Title = ObjectEditor.Translate(name)
                };
                ret.Fields = new ObservableCollection<EditableField>();
                Root.Add(ret.View);
                FieldGroups.Add(ret);
            }
            return ret;
        }

        private void PopulateFields()
        {
            var type = DataItem.GetType();
            var fields = type.GetRuntimeProperties().Where(d => d.CanWrite);
            AllFields = new List<EditableField>();
            if (FieldGroups == null)
                FieldGroups = new ObservableCollection<FieldGroup>();
            FieldGroups.Clear();

            foreach (var field in fields.Select(d => new EditableField(d, DataItem)).OrderBy(d => d.PropertyData.Order))
            {
                if (field.PropertyData.Excluded && !ShowExcudedAsText)
                    continue;
                if (field.EditorCell != null)
                {
                    AllFields.Add(field);
                    var grp = GetGroup(field.PropertyData.Group);
                    grp.Fields.Add(field);
                    grp.View.Add(field.EditorCell);
                }
            }

        }

        public ObservableCollection<FieldGroup> FieldGroups { get; internal set; }

        public List<EditableField> AllFields { get; internal set; }
    }
}

