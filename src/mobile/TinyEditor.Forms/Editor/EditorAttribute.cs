using System;
using System.Collections.Generic;

namespace TinyEditor
{

    public interface IEditorRelation
    {
        IEnumerable<object> Values { get; }
        object FindItem(object value);
    }


    public class EditorAttribute : Attribute
    {
        public string Title { get; set; }
        public string Group { get; set; }
        public string PlaceHolder { get; set; }
        public bool Excluded { get; set; }
        public bool Readonly { get; set; }
        public int Order { get; set; } = 99999;
        public Type RelationTo { get; set; }
        public Type CustomHandler { get; set; }

        public EditorAttribute(string title, string group) : this(title)
        {
            Group = group;
        }

        public EditorAttribute(string title)
        {
            Title = title;
        }
    }
}

