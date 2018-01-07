using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyEditor;
using TinyShopping.Core.Net.Interface;
using TinyShopping.Core.Services;

namespace TinyShopping.ApplicationModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ShoppingListRelation : IEditorRelation
    {
        private readonly ShoppingService _service;
        private IList<ShoppingList> _list;

        public ShoppingListRelation()
        {
            _service = new Core.Services.ShoppingService();
        }

        public object FindItem(object value)
        {
            if (value is int id)
            {
                return _list.FirstOrDefault(d => d.Id == id);
            }
            else if (value is IShoppingList lst)
            {
                return _list.FirstOrDefault(d => d.Id == lst.Id);
            }
            return null;
        }

        public async Task<IEnumerable<object>> GetValues()
        {
            var ret = await _service.GetShoppingLists();
            return ret.OfType<object>();
        }
    }



    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ShoppingList : IShoppingList
    {
        public int Id { get; set; }

        [Editor("Created", "List data", Readonly = true)]
        public System.DateTime Created { get; set; } = System.DateTime.Now;

        bool completed;
        [Editor("Marked as complete", "List data")]
        public bool Completed
        {
            get
            {
                return completed;
            }

            set
            {
                completed = value;
                if (value == true)
                    Done = System.DateTime.Now;
            }
        }

        [Editor("Completed at", "List data", Readonly = true)]
        public System.DateTime Done { get; set; }

        [Editor("Name", "List data", Order = 1)]
        public string Name { get; set; }


        public override string ToString()
        {
            return Name;
        }

        public int StoreID { get; set; }
    }
}
