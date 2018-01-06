using System;
using System.Collections.Generic;
using TinyShopping.Core.services;
using TinyShopping.Core.Net.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;
using TinyMvvm;
using TinyPubSubLib;
using TinyShopping.Messaging;

namespace TinyShopping.ViewModels
{
    
    public class ShoppingListViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;
		
        public ShoppingListViewModel(ShoppingService shoppingService) 
        {
            _shoppingService = shoppingService;
        }

        public async override Task OnFirstAppear()
        {
            await LoadData();
        }

        [TinySubscribe(Channels.ShoppingListAdded)]
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
