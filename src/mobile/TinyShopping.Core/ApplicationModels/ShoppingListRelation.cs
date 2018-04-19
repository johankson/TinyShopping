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
            if (value is string id)
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
}
