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
                            if (oldList == null)
                            {
                                _currentLists.AddWithId(d);
                            }
                            else
                            {
                                _currentLists.Replace(oldList, d);
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

        public void UpdateItem(Item item)
        {
            AddItem(item);
        }

        public void AddItem(Item item)
        {
            Task.Run(async () =>
            {
                var key = "listitems/" + item.ListId;

                await _client.AddOrUpdate(key, item);
            });
        }

        //private void AddSync(IHasId data)
        //{
        //    var list = data.GetShoppingList(_currentLists);
        //    if (list != null)
        //    {
        //        Task.Run(async () =>
        //        {
        //            if (string.IsNullOrEmpty(list.Id))
        //            {
        //                var ret = await _client
        //                  .Child("lists")
        //                        .PostAsync(list);
        //                list.Id = ret.Key;
        //            }
        //            else
        //            {
        //                await _client.Child("lists")
        //                             .Child(list.Id)
        //                             .PutAsync(list);
        //            }

        //        });
        //    }
        //}

        private void AddItemToList(Item item)
        {
            var list = _currentLists.FirstOrDefault(d => d.Id == item.ListId);
            if (!list.Items.Contains(item))
                list.Items.Add(item);
        }

        public void AddList(ShoppingList item)
        {
            if (!_currentLists.Contains(item))
                _currentLists.Add(item);
            Task.Run(async () =>
            {
                await _client.AddOrUpdate("lists", item);
            });

        }

        public void Delete(IHasId item)
        {
            var key = (item is Item i) ? "listitems/" + i.ListId : "lists";
            Task.Run(async () =>
            {
                await _client
                    .Child(key)
                    .Child(item.Id)
                    .DeleteAsync();
            });
        }

        public void UpdateList(ShoppingList shoppingList)
        {
            AddList(shoppingList);
        }

    }
}
