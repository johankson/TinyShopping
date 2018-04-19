using Xamarin.Forms;

namespace TinyEditor.Controls
{
    public interface IEditorGenerator
    {
        View GenerateEditor(ref object item);
        Element GetEditor(EditableField field, object obj);
    }
}
