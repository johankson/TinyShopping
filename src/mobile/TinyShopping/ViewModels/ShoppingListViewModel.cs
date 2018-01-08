﻿using TinyShopping.Core.Services;
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
            if (!string.IsNullOrWhiteSpace(_searchString) && _searchString.Length>2)
            {
                var newList = new ShoppingList()
                {
                    Name = _searchString
                };
                ShoppingLists.Insert(0, newList);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _shoppingService.AddList(newList);
                    await LoadData();
                });
            }
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

        [TinySubscribe(Channels.ShoppingListUpdated)]
        public async Task ShoppingListUpdated(ShoppingList shoppingList)
        {
            // TODO Only update the GUI for the shoppinglist that was updated
            await LoadData();
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
                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.NavigateToAsync("ItemListView", value);
                    });
                }
            }
        }

        public string NewListName
        {
            get;
            set;
        } = string.Empty;

        public ICommand Delete => new TinyCommand<ShoppingList>(async (shoppingList) =>
        {
            ShoppingLists.Remove(shoppingList);
            _allLists.Remove(shoppingList);
            await _shoppingService.Delete(shoppingList);
        });

        public ICommand Refresh => new TinyCommand(async () => await LoadData());

        public ICommand Edit => new TinyCommand<ShoppingList>(async (shoppingList) =>
         {
             await Navigation.NavigateToAsync("ListEditorView", shoppingList);
         });

    }
}
