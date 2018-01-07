using System.Reflection;

namespace TinyEditor
{
    public static class EditorExtensions
    {
        //private static Type AttrType = typeof(EditorAttribute);
        public static EditorAttribute GetEditorAttribute(this PropertyInfo prp)
        {
            var ret = prp.GetCustomAttribute<EditorAttribute>();
            if (ret == null)
            {
                ret = new EditorAttribute(prp.Name, "Basic")
                {
                    Excluded = true
                };
            }
            return ret;
        }
    }
}

