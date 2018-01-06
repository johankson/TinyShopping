using System;
using System.Threading.Tasks;
using TinyShopping.Core.services;
using Xamarin.Forms;

namespace TinyShopping.ViewModels
{
    public class ShoppingListViewModel 
    {
        private ShoppingService _shoppingService;

        public ShoppingListViewModel(int shoppingListId)
        {
            _shoppingService = new ShoppingService();
            Task.Run(async () => await LoadData(shoppingListId));
        }

        public async Task LoadData(int id)
        {
            var items = await _shoppingService.GetItems(id);
        }
    }
}
