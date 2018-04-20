using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyShopping.ApplicationModels;
using TinyShopping.Core.ApplicationModels.RestHelper;
using TinyShopping.Core.Net;
using TinyShopping.Core.Net.Interface;
using TinyHelper;
using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using TinyPubSubLib;
using TinyShopping.Core.Extensions;


namespace TinyShopping.Core.Services
{
    public class ShoppingService
    {
        private FirebaseClient _client;

        private async void PopulateItems(ShoppingList list)
        {
            var key = "listitems/" + list.Id;
            var items = await _client
                .Child(key)
                          .OrderByKey()
                          .OnceAsync<Item>();
            foreach (var item in items)
            {
                list.Items.AddWithId(item);
            }

            var observable = _client
                .Child(key)
                .AsObservable<Item>()
                .Subscribe(d =>
                {

                    var oldItem = list.Items.FindExisting(d);
                    if (d.EventType == FirebaseEventType.InsertOrUpdate)
                    {
                        if (d.Object == null)
                            return;
                        if (oldItem == null)
                        {
                            list.Items.AddWithId(d);
                        }
                        else
                        {
                            list.Items.Replace(oldItem, d);
                        }
                    }
                    else if (d.EventType == FirebaseEventType.Delete)
                    {
                        if (oldItem != null)
                        {
                            list.Items.Remove(oldItem);
                        }
                    }

                });

        }

        public ShoppingService()
        {
            _client = new FirebaseClient("https://turnkey-lacing-622.firebaseio.com/");
            _currentLists = new ObservableCollection<ShoppingList>();
            _currentLists.CollectionChanged += async (sender, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    foreach (ShoppingList list in e.NewItems)
                    {
                        PopulateItems(list);
                    }
                }
            };
            Task.Run(async () =>
            {
                var lists = await _client
                  .Child("lists")
                  .OrderByKey()
                  .OnceAsync<ShoppingList>();

                foreach (var list in lists)
                {
                    _currentLists.AddWithId(list);
                }

                var observable = _client
                    .Child("lists")
                    .AsObservable<ShoppingList>()
                    .Subscribe(d =>
                    {

                        var oldList = _currentLists.FindExisting(d);
                        if (d.EventType == FirebaseEventType.InsertOrUpdate)
                        {
                            if (d.Object == null)
                                return;
                            if (oldList == null)
                            {
                                _currentLists.AddWithId(d);
                            }
                            else
                            {
                                oldList.CopyFrom(d.Object);
                                //_currentLists.Replace(oldList, d);
                                //PopulateItems(d.Object);
                            }
                            //TinyPubSub.Publish("shopping-list-added");
                        }
                        else if (d.EventType == FirebaseEventType.Delete)
                        {
                            if (oldList != null)
                            {
                                _currentLists.Remove(oldList);
                                //TinyPubSub.Publish("shopping-list-deleted");
                            }
                        }

                    });
            });
        }

        private ObservableCollection<ShoppingList> _currentLists { get; set; }

        public ObservableCollection<ShoppingList> ShoppingLists
        {
            get
            {
                return _currentLists;
            }
        }

        public async Task<Item> UpdateItem(Item item)
        {
            return await AddItem(item);
        }

        public async Task<Item> AddItem(Item item)
        {

            var key = "listitems/" + item.ListId;

            return await _client.AddOrUpdate(key, item);

        }


        //private void AddItemToList(Item item)
        //{
        //    var list = _currentLists.FirstOrDefault(d => d.Id == item.ListId);
        //    if (!list.Items.Contains(item))
        //        list.Items.Add(item);
        //}

        public async Task<ShoppingList> AddList(ShoppingList item)
        {
            //bool isNew = string.IsNullOrEmpty(item.Id);

            var ret = await _client.AddOrUpdate("lists", item);
            //var key = "listitems/" + ret.Id;
            //if (isNew)
            //{
            //    await _client.Child(key).PostAsync("placeholder");
            //}
            return ret;
        }

        public async Task Delete(IHasId item)
        {
            var key = (item is Item i) ? "listitems/" + i.ListId : "lists";

            await _client
                .Child(key)
                .Child(item.Id)
                .DeleteAsync();

        }

        public async Task<ShoppingList> UpdateList(ShoppingList shoppingList)
        {
            return await AddList(shoppingList);
        }

    }
}
