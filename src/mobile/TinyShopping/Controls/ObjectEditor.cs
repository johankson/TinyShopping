using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace TinyShopping.Controls
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

        public bool ShowExcudedAsText
        {
            get;
            set;
        } = true;

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
                name = "Extra";
            var ret = FieldGroups.FirstOrDefault(d => d.Name.Equals(name));
            if (ret == null)
            {
                ret = new FieldGroup()
                {
                    Name = name
                };
                ret.View = new TableSection()
                {
                    Title = name
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
            var fields = type.GetProperties().Where(d => d.CanWrite);
            AllFields = new List<EditableField>();
            if (FieldGroups == null)
                FieldGroups = new ObservableCollection<FieldGroup>();
            FieldGroups.Clear();

            foreach (var field in fields.Select(d => new EditableField(d, DataItem)).OrderBy(d=>d.PropertyData.Order))
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

