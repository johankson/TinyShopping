using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace TinyEditor.Controls
{
    public abstract class BaseGenerator : IEditorGenerator
    {

        public List<EditableField> AllFields { get; internal set; }

        internal object DataItem { get; set; }

        public bool ShowExcudedAsText
        {
            get;
            set;
        } = false;

        public virtual View GenerateEditor(ref object item)
        {
            DataItem = item;
            var ret = GetRootElement(item);
            PopulateFields();
            return ret;
        }

        public virtual void PopulateFields()
        {
            var type = DataItem.GetType();
            var fields = type.GetRuntimeProperties().Where(d => d.CanWrite);
            AllFields = new List<EditableField>();


            foreach (var field in fields.Select(d => new EditableField(d, DataItem, this)).OrderBy(d => d.PropertyData.Order))
            {

                if (field.PropertyData.Excluded && !ShowExcudedAsText)
                    continue;
                if (field.EditorView != null)
                {
                    AddEditorToRoot(field);
                    AllFields.Add(field);
                }
            }

        }

        public abstract View GetRootElement(object obj);

        public abstract Element GetEditor(EditableField field, object obj);

        public abstract void AddEditorToRoot(EditableField field);

    }
}
