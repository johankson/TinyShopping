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
            _client = new ShoppingAPI(new Uri("http://192.168.119.131:5000"), new UnsafeCredentials(), new TinyCache.TinyCacheDelegationHandler());
        }

        public async Task<IList<ShoppingList>> GetShoppingLists()
        {
            var data =  await _client.GetShoppingListsAsync();
            return data;
        }

        public async Task<IList<Item>> GetItems(int shoppingListId)
        {
            var data = await _client.GetListItemsAsync(shoppingListId);
            return data;
        }
    }
}
