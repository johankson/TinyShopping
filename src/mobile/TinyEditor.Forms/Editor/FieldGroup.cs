using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TinyEditor.Controls
{
    public class FieldGroup
    {
        public object View { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public ObservableCollection<EditableField> Fields { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public abstract class GroupView : Element, IEditorGroup
    {
        public abstract void Add(Element elm);
    }

    public interface IEditorGroup
    {
        void Add(Element elm);
    }
}

