using Xamarin.Forms;

namespace TinyEditor.Controls
{
    public interface IEditorGenerator
    {
        View GenerateEditor(ref object item);
        //void AddSection(FieldGroup ret);
        Element GetEditor(EditableField field, object obj);
        //GroupView GetGroup(FieldGroup group);
    }
}
