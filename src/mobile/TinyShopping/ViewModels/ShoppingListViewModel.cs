using TinyShopping.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TinyPubSubLib;
using TinyShopping.Messaging;
using TinyShopping.ApplicationModels;
using System.Windows.Input;
using TinyMvvm;
using System;
using Xamarin.Forms;

namespace TinyShopping.ViewModels
{

    public class ShoppingListViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;

        public ShoppingListViewModel(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        public void SearchTextChanged(string e)
        {
            var it = 2;
        }

        //public void OpenList(ShoppingList shoppingList)
        //{
        //    Navigation.NavigateToAsync("ItemListView", shoppingList);
        //}

        public async void AddListFromName()
        {
            var newList = new ShoppingList()
            {
                Name = NewListName
            };
            await _shoppingService.AddList(newList);
            ShoppingLists.Insert(0, newList);
            NewListName = string.Empty;
        }

        public async override Task OnFirstAppear()
        {
            await LoadData();
        }

        [TinySubscribe(Channels.ShoppingListAdded)]
        [TinySubscribe(Channels.ShoppingListDeleted)]
        public async Task LoadData()
        {
            IsBusy = true;
            ShoppingLists = new ObservableCollection<ShoppingList>(await _shoppingService.GetShoppingLists());
            IsBusy = false;
        }

        public ObservableCollection<ShoppingList> ShoppingLists { get; set; }

        ShoppingList selectedItem;

        public ShoppingList SelectedItem
        {
            get
            {
                return null;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.NavigateToAsync("ItemListView", value);
                    RaisePropertyChanged(nameof(SelectedItem));
                });
            }
        }

        public string NewListName
        {
            get;
            set;
        } = string.Empty;

        public ICommand Delete
        {
            get
            {
                return new TinyCommand<ShoppingList>(async (shoppingList) =>
                {
                    ShoppingLists.Remove(shoppingList);
                    await _shoppingService.Delete(shoppingList);
                });
            }
        }

        public ICommand Refresh => new TinyCommand(async () => await LoadData());

        public ICommand Edit
        {
            get
            {
                return new TinyCommand<ShoppingList>(async (shoppingList) =>
                {
                    await Navigation.NavigateToAsync("ListEditorView", shoppingList);
                });
            }
        }
    }
}
