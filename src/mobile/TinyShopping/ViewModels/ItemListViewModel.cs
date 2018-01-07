using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;
using TinyPubSubLib;
using TinyShopping.ApplicationModels;
using TinyShopping.Core.Services;
using TinyShopping.Messaging;
using Xamarin.Forms;

namespace TinyShopping.ViewModels
{
    public class ItemListViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;
        private ShoppingList _shoppingList;

        public ItemListViewModel(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        public override Task Initialize()
        {
            _shoppingList = NavigationParameter as ShoppingList;
            RaisePropertyChanged(nameof(Name));
            return base.Initialize();
        }

        public string Name => _shoppingList?.Name ?? String.Empty;

        public void AddItemFromName()
        {
            var newItem = new Item()
            {
                ListId = _shoppingList.Id,
                Name = NewItemName
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                ItemsList.Insert(0,newItem);
            });
            Task.Run(async () =>
            {
                await _shoppingService.AddItem(newItem);
            });
            NewItemName = string.Empty;
        }

        public async override Task OnFirstAppear()
        {
            await LoadData();
        }

        public async Task LoadData()
        {
            ItemsList = new ObservableCollection<Item>(await _shoppingService.GetListItems(_shoppingList.Id));
        }

        public string NewItemName
        {
            get;
            set;
        }

        public ObservableCollection<Item> ItemsList { get; set; }

        public ICommand Delete
        {
            get
            {
                return new TinyCommand<Item>(async (item) =>
                {
                    ItemsList.Remove(item);
                    await _shoppingService.DeleteItem(item);
                });
            }
        }

        public ICommand Refresh => new TinyCommand(async () => await LoadData());

        public ICommand Edit
        {
            get
            {
                return new TinyCommand<ShoppingList>(async (item) =>
                {
                    await Navigation.NavigateToAsync("ListItemEditorView", item);
                });
            }
        }
    }
}
