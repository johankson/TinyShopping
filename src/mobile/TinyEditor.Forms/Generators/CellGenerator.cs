using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TinyEditor.Controls;
using TinyEditor.Forms.Cells;
using Xamarin.Forms;

namespace TinyEditor.Controls
{
    public class CellGenerator : BaseGenerator
    {
        public CellGenerator()
        {
        }

        public override Element GetEditor(EditableField field, object parent)
        {
            Element view = null;
            if (view == null)
            {
                if (field.PropertyData.Readonly)
                {
                    view = new TextCell()
                    {
                        Text = ObjectEditor.Translate(field.PropertyData.Title),
                        BindingContext = field,
                        Detail = field.Value.ToString()
                    };
                    view.SetBinding(TextCell.DetailProperty, nameof(field.Value));

                }
                else
                {
                    var type = field.SourceProperty.PropertyType;
                    if (field.PropertyData.RelationTo != null)
                    {
                        var inst = Activator.CreateInstance(field.PropertyData.RelationTo);
                        if (inst is IEditorRelation rel)
                        {
                            view = new RelationCell(rel)
                            {
                                Text = ObjectEditor.Translate(field.PropertyData.Title)
                            };
                            view.SetBinding(DateCell.ValueProperty, nameof(field.Value));
                        }
                    }
                    else if (type == typeof(string) || type == typeof(int))
                    {
                        view = new EntryCell()
                        {
                            Label = ObjectEditor.Translate(field.PropertyData.Title)
                        };
                        view.SetBinding(EntryCell.TextProperty, nameof(field.Value));
                    }
                    else if (type == typeof(bool))
                    {
                        view = new SwitchCell()
                        {
                            Text = ObjectEditor.Translate(field.PropertyData.Title)
                        };
                        view.SetBinding(SwitchCell.OnProperty, nameof(field.Value));
                    }
                    else if (type == typeof(DateTime))
                    {
                        view = new DateCell()
                        {
                            Text = ObjectEditor.Translate(field.PropertyData.Title)
                        };
                        view.SetBinding(DateCell.ValueProperty, nameof(field.Value));
                    }
                    if (view != null)
                        view.BindingContext = field;

                }

            }
            return view as Element;
        }

        private TableRoot table;

        public override View GetRootElement(object obj)
        {
            var tbl = new TableView();

            table = new TableRoot();
            tbl.Root = table;
            return tbl;
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
                    Title = name
                };


                ret.Fields = new ObservableCollection<EditableField>();
                table.Add(ret.View as TableSection);

                FieldGroups.Add(ret);
            }
            return ret;
        }

        public override void AddEditorToRoot(EditableField field)
        {
            var grp = GetGroup(field.PropertyData.Group);
            grp.Fields.Add(field);
            (grp.View as TableSection).Add(field.EditorView as Cell);
        }
    }

    public interface IEditorGenerator
    {
        View GenerateEditor(ref object item);
        //void AddSection(FieldGroup ret);
        Element GetEditor(EditableField field, object obj);
        //GroupView GetGroup(FieldGroup group);
    }

    public abstract class BaseGenerator : IEditorGenerator
    {

        public ObservableCollection<FieldGroup> FieldGroups { get; internal set; }

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
            if (FieldGroups == null)
                FieldGroups = new ObservableCollection<FieldGroup>();
            FieldGroups.Clear();

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
