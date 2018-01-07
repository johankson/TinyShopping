using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyShopping.Core.Net;
using TinyShopping.Core.Net.Models;

namespace TinyShopping.Core.services
{
    public class ShoppingService
    {
        private IShoppingAPI _client;

        public ShoppingService()
        {
            //_client = new ShoppingAPI(new Uri("http://localhost:5000"), new UnsafeCredentials(), new TinyCache.TinyCacheDelegationHandler());
            _client = new ShoppingAPI(new Uri("http://localhost:5000"), new UnsafeCredentials());
        }

        public async Task<IList<ShoppingList>> GetShoppingLists()
        {
            var data = await _client.GetShoppingListsAsync();
            return data;
        }

        public async Task AddItem(Item item)
        {
            await _client.AddListItemAsync(item);
        }

        public async Task AddList(ShoppingList item)
        {
            await _client.AddShoppingListAsync(item);
        }

        public async Task<IList<Item>> GetListItems(int listId)
        {
            var data = await _client.GetListItemsAsync(listId);
            return data;
        }

        public async Task Delete(ShoppingList shoppingList)
        {
            await _client.DeleteShoppingListAsync(shoppingList.Id.Value);
        }
    }
}
