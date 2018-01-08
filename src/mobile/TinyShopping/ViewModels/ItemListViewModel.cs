using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;
using TinyPubSubLib;
using TinyShopping.ApplicationModels;
using TinyShopping.Core.Services;
using TinyShopping.Messaging;
using TinyShopping.Views;
using Xamarin.Forms;

namespace TinyShopping.ViewModels
{
    public class ItemListViewModel : ShoppingBaseModel, ISearchHandler
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

        public void AddItem()
        {
            if (!string.IsNullOrWhiteSpace(_searchString) && _searchString.Length > 2)
            {
                var newItem = new Item()
                {
                    ListId = _shoppingList.Id,
                    Name = _searchString
                };
                Device.BeginInvokeOnMainThread(() =>
                {
                    ItemsList.Insert(0, newItem);
                });
                Task.Run(async () =>
                {
                    await _shoppingService.AddItem(newItem);
                    await LoadData();
                });
            }
            //NewItemName = string.Empty;
        }

        public async override Task OnFirstAppear()
        {
            await LoadData();
        }

        public async Task LoadData()
        {
            _allItems = await _shoppingService.GetListItems(_shoppingList.Id);
            FilterResult();
        }

        private void FilterResult()
        {
            var ret = _allItems;
            if (!string.IsNullOrEmpty(_searchString))
                ret = _allItems.Where(d => d.Name.Contains(_searchString)).ToList();
            ItemsList = new ObservableCollection<Item>(ret);
        }


        private string _searchString;
        public void Search(string value)
        {
            _searchString = value;
            FilterResult();
        }

        public void Clear()
        {
            _searchString = string.Empty;
            FilterResult();
        }

        public string NewItemName
        {
            get;
            set;
        }

        private IList<Item> _allItems;
        public ObservableCollection<Item> ItemsList { get; set; }

        public ICommand Delete => new TinyCommand<Item>(async (item) =>
        {
            ItemsList.Remove(item);
            await _shoppingService.DeleteItem(item);
        });

        public ICommand Refresh => new TinyCommand(async () => await LoadData());

        public ICommand Edit => new TinyCommand<Item>(async (item) =>
        {
            await Navigation.NavigateToAsync("ListItemEditorView", item);
        });

        public ICommand Changed => new TinyCommand<Item>(async (item) =>
        {
            await _shoppingService.UpdateItem(item);

            // QUESTION: Should the task firing be part of the shopping service?
            TinyPubSub.Publish(Channels.ShoppingListUpdated, _shoppingList);
        });

        public ICommand CreateNewItem => new TinyCommand(() =>
        {
            var newItem = new Item()
            {
                ListId = _shoppingList.Id
            };
            Edit.Execute(newItem);
        });
    }
}
