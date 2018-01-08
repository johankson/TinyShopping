using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyShopping.ApplicationModels;
using TinyShopping.Core.ApplicationModels.RestHelper;
using TinyShopping.Core.Net;
using TinyShopping.Core.Net.Interface;


namespace TinyShopping.Core.Services
{
    public class ShoppingService
    {
        private IShoppingAPI _client;

        public ShoppingService()
        {
            //_client = new ShoppingAPI(new Uri("http://localhost:5000"), new UnsafeCredentials(), new TinyCache.TinyCacheDelegationHandler());
            _client = new ShoppingAPI(new Uri("http://192.168.1.131:5000"), new UnsafeCredentials());
        }

        private const string LISTKEY = "shoppingLists";
        private const string ITEMSKEY = "listsItems";

        private string ItemListKey(int listid) {
            return ITEMSKEY + listid;
        }

        public async Task<IList<ShoppingList>> GetShoppingLists()
        {
            var data = await TinyCache.TinyCache.RunAsync(LISTKEY, async () =>
            {
                var ret = await _client.GetShoppingListsAsync();
                return ret.OrderByDescending(d => d.Created).ToModel();
            });
            return data;
        }

        public async Task AddItem(Item item)
        {
            await _client.AddListItemAsync(item.ToRest());
            TinyCache.TinyCache.Remove(ItemListKey(item.ListId));
        }

        public async Task AddList(ShoppingList item)
        {
            await _client.AddShoppingListAsync(item.ToRest());
            TinyCache.TinyCache.Remove(LISTKEY);
        }

        public async Task UpdateItem(Item item)
        {
            await _client.UpdateListItemAsync(item.Id, item.ToRest());
            TinyCache.TinyCache.Remove(ItemListKey(item.ListId));
        }

        public async Task<IList<Item>> GetListItems(int listId)
        {
            var data = await TinyCache.TinyCache.RunAsync(ItemListKey(listId), async () =>
            {
                var ret = await _client.GetListItemsAsync(listId);
                return ret.ToModel();
            });
            return data;
        }

        public async Task DeleteItem(Item item)
        {
            await _client.DeleteListItemAsync(item.Id);
            TinyCache.TinyCache.Remove("listItems"+item.ListId);
        }

        public async Task Delete(ShoppingList shoppingList)
        {
            await _client.DeleteShoppingListAsync(shoppingList.Id);
        }

        public async Task UpdateList(ShoppingList shoppingList)
        {
            await _client.UpdateShoppingListAsync(shoppingList.Id, shoppingList.ToRest());
        }
    }
}
