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
    /// <summary>
    /// Handles the list of items for a specific shopping list
    /// </summary>
    public class ItemListViewModel : ShoppingBaseModel, ISearchHandler
    {
        private ShoppingService _shoppingService;
        private ShoppingList _shoppingList;

        private bool _isLoading;

        public Action<Item> ScrollTo { get; set; } = null;

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
            if (!string.IsNullOrWhiteSpace(_searchString) && _searchString.Length > 1)
            {
                var newItem = new Item()
                {
                    ListId = _shoppingList.Id,
                    Name = _searchString
                };

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //ItemsList.Insert(0, newItem);
                    Clear();

                    await Task.Delay(100);

					ScrollTo?.Invoke(newItem);

                    _shoppingService.AddItem(newItem);
                    await LoadData();
                });

                //Task.Run(async () =>
                //{
                //    await _shoppingService.AddItem(newItem);
                //    await LoadData();
                //    ScrollTo?.Invoke(_allItems.First());
                //});
            }
        }

        public bool ListComplete { get; set; }

        public bool DisplayTick { get; set; }

        public ICommand TickPlaybackFinished => new Command(
            (o) => 
            {
                DisplayTick = false;
            }
        );

        public Action PlayTickAnimation { get; set; }

        public async override Task OnFirstAppear()
        {
            await LoadData();
        }

        public async Task LoadData()
        {
            IsBusy = true;
            _allItems = await _shoppingService.GetListItems(_shoppingList.Id);
            FilterResult();
            IsBusy = false;
        }

        private void FilterResult()
        {
            if (_allItems != null)
            {
                var ret = _allItems;
                if (!string.IsNullOrEmpty(_searchString))
                {
                    ret = _allItems.Where(d => d.Name.Contains(_searchString)).ToList();
                }

                ItemsList = new ObservableCollection<Item>(ret.Where(d=>!d.Deleted));
            }
            else
            {
                ItemsList = new ObservableCollection<Item>();
            }
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

        public ICommand Delete => new TinyCommand<Item>((item) =>
        {
            ItemsList.Remove(item);
            _shoppingService.Delete(item);
        });

        public ICommand Refresh => new TinyCommand(async () => await LoadData());

        public ICommand Edit => new TinyCommand<Item>(async (item) =>
        {
            await Navigation.NavigateToAsync("ListItemEditorView", item);
        });

        public ICommand Changed => new TinyCommand<Item>((item) =>
        {
            _shoppingService.UpdateItem(item);

            var isCompleted = ItemsList.All(x => x.Completed == true);
            if (isCompleted && IsNotBusy)
            {
                PlayTickAnimation?.Invoke();
            }

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
