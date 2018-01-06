using System;
using System.Collections.Generic;
using TinyShopping.Core.services;
using TinyShopping.Core.Net.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;

namespace TinyShopping.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class TinyShoppingListViewModel
    {
        private ShoppingService _shoppingService;
		
        public INavigation Navigation { get; internal set; }

        public TinyShoppingListViewModel()
        {
            _shoppingService = new ShoppingService();

            Task.Run(async () => await LoadData());
        }

        public async Task LoadData()
        {
            ShoppingLists = new ObservableCollection<ShoppingList>(await _shoppingService.GetShoppingLists());
        }

        public ObservableCollection<ShoppingList> ShoppingLists { get; set; }

        public ShoppingList SelectedItem
        {
            set
            {
                if(value == null)
                {
                    return;
                }
            }
        }
    }
}
