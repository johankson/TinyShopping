using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TinyPubSubLib;
using TinyShopping.ApplicationModels;
using TinyShopping.Core.services;
using TinyShopping.Messaging;

namespace TinyShopping.ViewModels
{
    public class ItemListViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;

        public ItemListViewModel(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        public override Task Initialize()
        {
            var shoppingList = NavigationParameter as ShoppingList;
            ListId = shoppingList.Id;
            return base.Initialize();
        }

        public void AddItemFromName()
        {
            
        }

        public async override Task OnFirstAppear()
        {
            await LoadData();
        }

        public async Task LoadData()
        {
            ItemsList = new ObservableCollection<Item>(await _shoppingService.GetListItems(ListId));
        }

        public string NewItemName
        {
            get;
            set;
        }

        public ObservableCollection<Item> ItemsList { get; set; }

        public ShoppingList SelectedItem
        {
            set
            {
                if (value == null)
                {
                    return;
                }
            }
        }

        public int ListId
        {
            get;
            set;
        } = 1;
    }
}
