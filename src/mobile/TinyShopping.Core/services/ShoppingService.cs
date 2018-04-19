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


namespace TinyShopping.Core.Services
{
    public class ShoppingService
    {
        private FirebaseClient _client;

        public ShoppingService()
        {
            _client = new FirebaseClient("https://turnkey-lacing-622.firebaseio.com/");
            //_client = new ShoppingAPI(new Uri("http://localhost:5000"), new UnsafeCredentials());
            //_client = new ShoppingAPI(new Uri("http://192.168.1.131:5000"), new UnsafeCredentials());

            // Get changes from TinyCache to update collections
            //TinyCache.TinyCache.OnUpdate += TinyCache_OnUpdate;
        }

        //private TinyCache.TinyCachePolicy _fetchPolicy = new TinyCache.TinyCachePolicy().SetMode(TinyCache.TinyCacheModeEnum.CacheFirst).SetFetchTimeout(300);
        //private const string LISTKEY = "shoppingLists";
        //private const string ITEMSKEY = "listsItems_";
        //private int _retryCount = 0;
        //private IList<IOfflineSupport> _syncItems { get; set; } = new List<IOfflineSupport>();

        //private string ItemListKey(int listid)
        //{
        //    return ITEMSKEY + listid;
        //}

        private ObservableCollection<ShoppingList> _currentLists { get; set; }

        public async Task<IList<ShoppingList>> GetShoppingLists()
        {
            if (_currentLists == null)
            {
                _currentLists = new ObservableCollection<ShoppingList>();
                var lists = await _client
                  .Child("lists")
                  .OrderByKey()
                  .OnceAsync<ShoppingList>();

                foreach (var list in lists)
                {
                    list.Object.Id = list.Key;
                    _currentLists.Add(list.Object);
                }

                var observable = _client
  .Child("lists")
  .AsObservable<ShoppingList>()
                    .Subscribe(d =>
                    {

                        if (d.EventType == FirebaseEventType.InsertOrUpdate)
                        {
                            var oldList = _currentLists.FirstOrDefault(list => list.Id == d.Key);
                            if (oldList == null)
                            {
                                _currentLists.Add(d.Object);
                            }
                            else
                            {
                                var idx = _currentLists.IndexOf(oldList);
                                _currentLists.Remove(oldList);
                                _currentLists.Insert(idx, d.Object);
                            }
                            TinyPubSub.Publish("shopping-list-added");
                        }
                        else if (d.EventType == FirebaseEventType.Delete)
                        {
                            var oldList = _currentLists.FirstOrDefault(list => list.Id == d.Key);
                            if (oldList != null)
                            {
                                _currentLists.Remove(oldList);
                                TinyPubSub.Publish("shopping-list-deleted");
                            }
                        }

                    });

            }
            return _currentLists;
        }

        public void UpdateItem(Item item)
        {
            AddSync(item);
            //TinyCache.TinyCache.Remove(ItemListKey(item.ListId));
        }

        public void AddItem(Item item)
        {
            AddItemToList(item);
            //TinyCache.TinyCache.Remove(ItemListKey(item.ListId));
            AddSync(item);
        }

        private Task _syncTask;

        private ShoppingList GetList(IOfflineSupport data)
        {
            ShoppingList list = null;
            if (data is Item item)
            {
                list = _currentLists.FirstOrDefault(d => d.Id == item.ListId);
            }
            else if (data is ShoppingList datalist)
            {
                list = datalist;
            }
            return list;
        }

        private void AddSync(IOfflineSupport data)
        {
            var list = GetList(data);
            if (list != null)
            {
                Task.Run(async () =>
                {
                    if (string.IsNullOrEmpty(list.Id))
                    {
                        var ret = await _client
                          .Child("lists")
                                .PostAsync(list);
                        list.Id = ret.Key;
                    }
                    else
                    {
                        await _client.Child("lists")
                                     .Child(list.Id)
                                     .PutAsync(list);
                    }

                });
            }

            //data.NeedSync = true;
            //if (!_syncItems.Contains(data))
            //{
            //    _syncItems.Add(data);
            //    TriggerSync();
            //}
        }

        //private void TriggerSync()
        //{
        //    if (_syncItems != null && _syncItems.Any())
        //    {
        //        Console.WriteLine("Sync triggerd");
        //        if (_syncTask == null || _syncTask.IsCompleted)
        //            _syncTask = Task.Delay(1500).ContinueWith(async (a) =>
        //            {
        //                try
        //                {
        //                    await SyncItems();
        //                    _retryCount = 0;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex);
        //                    _retryCount++;
        //                    Console.WriteLine($"Retry in {_retryCount}s");
        //                    Task.Delay(_retryCount * 1000).ContinueWith((b) => TriggerSync());
        //                }
        //            });
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
            AddSync(item);
            //TinyCache.TinyCache.Remove(LISTKEY);
        }

        public void Delete(IOfflineSupport item)
        {

            Task.Run(async () =>
            {
                var list = GetList(item);
                await _client
                    .Child("lists")
                    .Child(list.Id)
                    .DeleteAsync();
            });
        }

        public void UpdateList(ShoppingList shoppingList)
        {
            AddSync(shoppingList);
        }

        //public async Task<IList<Item>> GetListItems(int listId)
        //{

        //    var data = await TinyCache.TinyCache.RunAsync(ItemListKey(listId), async () =>
        //    {
        //        return await _client.GetListItemsAsync(listId);
        //    }, _fetchPolicy);
        //    var list = _currentLists.FirstOrDefault(d => d.Id == listId);
        //    MergeListItems(list, data);
        //    return list.Items;

        //}

        //private static void MergeListItems(ShoppingList list, IEnumerable<IShoppingItem> items)
        //{
        //    if (items != null && items.Any())
        //    {
        //        foreach (var old in list.Items)
        //        {
        //            old.Deleted = true;
        //        }
        //        foreach (var i in items)
        //        {
        //            var old = list.Items.FirstOrDefault(d => d.Id == i.Id);
        //            if (old != null)
        //            {
        //                if (!old.NeedSync)
        //                {
        //                    i.MemberviseCopyTo(old);
        //                    old.LastSync = DateTime.Now;
        //                }
        //                old.Deleted = false;
        //            }
        //            else
        //            {
        //                var newitem = i.ToModel();
        //                list.Items.Add(newitem);
        //                newitem.LastSync = DateTime.Now;
        //                newitem.Deleted = false;
        //            }
        //        }
        //    }
        //}

        //private void SetDeleted(IOfflineSupport data)
        //{
        //    data.Deleted = true;
        //    data.NeedSync = true;
        //    if (!_syncItems.Contains(data))
        //    {
        //        _syncItems.Add(data);
        //    }
        //    TriggerSync();
        //}

        //public async Task SyncItems()
        //{
        //    Console.WriteLine("Syncing items");
        //    foreach (var item in _syncItems.ToList())
        //    {
        //        switch (item)
        //        {
        //            case Item i:
        //                await SyncListItem(item, i);
        //                break;
        //            case ShoppingList list:
        //                await SyncList(list);
        //                break;
        //        }
        //    }
        //}

        //private async Task SyncList(ShoppingList i)
        //{
        //    if (i.Deleted)
        //    {
        //        try
        //        {
        //            var ret = await _client.DeleteShoppingListAsync(i.Id);
        //            if (_currentLists.Contains(i))
        //                _currentLists.Remove(i);
        //            SetSynced(i);

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //        }
        //    }
        //    else
        //    {
        //        var ret = await _client.UpdateShoppingListAsync(i.ToRest());
        //        i.Id = ret.Id;
        //        SetSynced(i);
        //    }
        //}

        //private void SetSynced(IOfflineSupport i)
        //{
        //    i.LastSync = DateTime.Now;
        //    _syncItems.Remove(i);
        //}

        //private async Task SyncListItem(IOfflineSupport item, Item i)
        //{
        //    if (i.Deleted)
        //    {
        //        try
        //        {
        //            var ret = await _client.DeleteListItemAsync(i.Id);
        //            var list = _currentLists.FirstOrDefault(d => d.Id == i.ListId);
        //            if (list.Items.Contains(i))
        //                list.Items.Remove(i);
        //            SetSynced(i);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //        }
        //    }
        //    else
        //    {
        //        var ret = await _client.UpdateListItemAsync(i.ToRest());
        //        i.Id = ret.Id;
        //        var list = _currentLists.FirstOrDefault(d => d.Id == i.ListId);
        //        if (!list.Items.Contains(i))
        //        {
        //            list.Items.Add(i);
        //        }
        //        SetSynced(i);
        //    }
        //}


    }
}
