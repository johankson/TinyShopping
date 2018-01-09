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

namespace TinyShopping.Core.Services
{
    public class ShoppingService
    {
        private IShoppingAPI _client;

        public ShoppingService()
        {
            _client = new ShoppingAPI(new Uri("http://localhost:5000"), new UnsafeCredentials());
            //_client = new ShoppingAPI(new Uri("http://192.168.1.131:5000"), new UnsafeCredentials());
            TinyCache.TinyCache.OnUpdate += TinyCache_OnUpdate;
        }

        private TinyCache.TinyCachePolicy _fetchPolicy = new TinyCache.TinyCachePolicy().SetMode(TinyCache.TinyCacheModeEnum.CacheFirst).SetFetchTimeout(300);
        private const string LISTKEY = "shoppingLists";
        private const string ITEMSKEY = "listsItems_";

        private string ItemListKey(int listid)
        {
            return ITEMSKEY + listid;
        }

        private ObservableCollection<ShoppingList> _currentLists { get; set; } = new ObservableCollection<ShoppingList>();
        private IList<IOfflineSupport> _syncItems { get; set; } = new List<IOfflineSupport>();

        public async Task<IList<ShoppingList>> GetShoppingLists()
        {
            var data = await TinyCache.TinyCache.RunAsync(LISTKEY, async () =>
            {
                return await _client.GetShoppingListsAsync();
            }, _fetchPolicy);
            Merge(data);
            return _currentLists;
        }

        void TinyCache_OnUpdate(object sender, TinyCache.CacheUpdatedEvt e)
        {
            if (e.Value is IList<Net.Models.ShoppingList> lists)
            {
                Merge(lists);
            }
            else if (e.Value is IList<Net.Models.Item> items)
            {
                var first = items.FirstOrDefault();
                if (first != null)
                {
                    var list = _currentLists.FirstOrDefault(d => d.Id == first.ListId);
                    if (list != null)
                        MergeListItems(list, items);
                }
            }
        }

        private void Merge(IEnumerable<IShoppingList> list)
        {
            if (list != null && list.Any())
            {
                foreach (var old in _currentLists)
                {
                    if (!old.NeedSync)
                        old.Deleted = true;
                }
                foreach (var item in list)
                {
                    var old = _currentLists.FirstOrDefault(d => d.Id == item.Id);
                    if (old != null)
                    {
                        if (old.NeedSync == false)
                        {
                            item.MemberviseCopyTo(old, true);
                            old.LastSync = DateTime.Now;
                        }
                        old.Deleted = false;
                    }
                    else
                    {
                        var newlist = item.ToModel();
                        _currentLists.Add(newlist);
                        newlist.LastSync = DateTime.Now;
                    }
                }
            }
        }


        public void UpdateItem(Item item)
        {
            AddSync(item);
            //await _client.UpdateListItemAsync(item.ToRest());
            TinyCache.TinyCache.Remove(ItemListKey(item.ListId));
        }

        public void AddItem(Item item)
        {
            AddItemToList(item);
            //await _client.AddListItemAsync(item.ToRest());
            TinyCache.TinyCache.Remove(ItemListKey(item.ListId));
            AddSync(item);
        }

        private Task _syncTask;

        private void AddSync(IOfflineSupport data)
        {
            data.NeedSync = true;
            if (!_syncItems.Contains(data))
                _syncItems.Add(data);
            TriggerSync();
        }

        private void TriggerSync()
        {
            if (_syncItems != null && _syncItems.Any())
            {
                Console.WriteLine("Sync triggerd");
                if (_syncTask == null || _syncTask.IsCompleted)
                    _syncTask = Task.Delay(1500).ContinueWith(async (a) =>
                    {
                        try
                        {
                            await SyncItems();
                            _retryCount = 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            _retryCount++;
                            Console.WriteLine($"Retry in {_retryCount}s");
                            Task.Delay(_retryCount * 1000).ContinueWith((b) => TriggerSync());
                        }
                    });
            }
        }

        private int _retryCount = 0;

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
            //await _client.AddShoppingListAsync(item.ToRest());
            TinyCache.TinyCache.Remove(LISTKEY);
        }

        public async Task<IList<Item>> GetListItems(int listId)
        {

            var data = await TinyCache.TinyCache.RunAsync(ItemListKey(listId), async () =>
            {
                return await _client.GetListItemsAsync(listId);
            }, _fetchPolicy);
            var list = _currentLists.FirstOrDefault(d => d.Id == listId);
            MergeListItems(list, data);
            return list.Items;

        }

        private static void MergeListItems(ShoppingList list, IEnumerable<IShoppingItem> items)
        {
            if (items != null && items.Any())
            {
                foreach (var i in items)
                {
                    var old = list.Items.FirstOrDefault(d => d.Id == i.Id);
                    if (old != null)
                    {
                        if (!old.NeedSync)
                        {
                            i.MemberviseCopyTo(old);
                            old.LastSync = DateTime.Now;
                        }
                    }
                    else
                    {
                        var newitem = i.ToModel();
                        list.Items.Add(newitem);
                        newitem.LastSync = DateTime.Now;
                    }
                }
            }
        }

        private void SetDeleted(IOfflineSupport data)
        {
            data.Deleted = true;
            data.NeedSync = true;
            if (!_syncItems.Contains(data))
            {
                _syncItems.Add(data);
            }
            TriggerSync();
        }

        public async Task SyncItems()
        {
            Console.WriteLine("Syncing items");
            foreach (var item in _syncItems.ToList())
            {
                switch (item)
                {
                    case Item i:
                        await SyncListItem(item, i);
                        break;
                    case ShoppingList list:
                        await SyncList(list);
                        break;
                }
            }
        }

        private async Task SyncList(ShoppingList i)
        {
            if (i.Deleted)
            {
                try
                {
                    var ret = await _client.DeleteShoppingListAsync(i.Id);
                    if (_currentLists.Contains(i))
                        _currentLists.Remove(i);
                    SetSynced(i);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                var ret = await _client.UpdateShoppingListAsync(i.ToRest());
                SetSynced(i);
            }
        }

        private void SetSynced(IOfflineSupport i)
        {
            i.LastSync = DateTime.Now;
            _syncItems.Remove(i);
        }

        private async Task SyncListItem(IOfflineSupport item, Item i)
        {
            if (i.Deleted)
            {
                try
                {
                    var ret = await _client.DeleteListItemAsync(i.Id);
                    var list = _currentLists.FirstOrDefault(d => d.Id == i.ListId);
                    if (list.Items.Contains(i))
                        list.Items.Remove(i);
                    SetSynced(i);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                var ret = await _client.UpdateListItemAsync(i.ToRest());
                var list = _currentLists.FirstOrDefault(d => d.Id == i.ListId);
                if (list.Items.Contains(i))
                    list.Items.Remove(i);
                SetSynced(i);
            }
        }

        //public void DeleteItem(Item item)
        //{
        //    SetDeleted(item);
        //    //await _client.DeleteListItemAsync(item.Id);
        //    TinyCache.TinyCache.Remove("listItems" + item.ListId);
        //}

        public void Delete(IOfflineSupport item)
        {
            SetDeleted(item);
            //await _client.DeleteShoppingListAsync(shoppingList.Id);
        }

        public void UpdateList(ShoppingList shoppingList)
        {
            AddSync(shoppingList);
            //await _client.UpdateShoppingListAsync(shoppingList.Id, shoppingList.ToRest());
        }
    }
}
