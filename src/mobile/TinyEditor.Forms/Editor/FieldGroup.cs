using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TinyEditor.Controls
{
    public class FieldGroup
    {
        public TableSection View { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public ObservableCollection<EditableField> Fields { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}

