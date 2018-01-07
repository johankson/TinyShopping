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
using System.Collections.Generic;
using System.Linq;
using TinyShopping.Views;

namespace TinyShopping.ViewModels
{

    public class ShoppingListViewModel : ShoppingBaseModel, ISearchHandler
    {
        private ShoppingService _shoppingService;

        public ShoppingListViewModel(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        private string _searchString;



        private void FilterResults()
        {
            var res = _allLists;
            if (!string.IsNullOrEmpty(_searchString))
            {
                res = _allLists.Where(d => d.Name.Contains(_searchString)).ToList();
            }
            ShoppingLists = new ObservableCollection<ShoppingList>(res);
        }

        public void AddItem()
        {
            var newList = new ShoppingList()
            {
                Name = _searchString
            };
            Device.BeginInvokeOnMainThread(async () =>
            {
                await _shoppingService.AddList(newList);
                ShoppingLists.Insert(0, newList);
            });
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
            _allLists = await _shoppingService.GetShoppingLists();
            FilterResults();
            IsBusy = false;
        }

        public void Search(string value)
        {
            _searchString = value;
            FilterResults();
        }

        public void Clear()
        {
            _searchString = string.Empty;
            FilterResults();
        }

        private IList<ShoppingList> _allLists;
        public ObservableCollection<ShoppingList> ShoppingLists { get; set; }

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
