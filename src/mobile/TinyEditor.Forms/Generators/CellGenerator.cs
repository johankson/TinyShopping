using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public ObservableCollection<FieldGroup> FieldGroups { get; internal set; }


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
            if (FieldGroups == null)
                FieldGroups = new ObservableCollection<FieldGroup>();
            FieldGroups.Clear();
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
}
