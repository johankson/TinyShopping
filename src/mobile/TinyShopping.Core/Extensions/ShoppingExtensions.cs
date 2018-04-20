using System;
using System.Collections.ObjectModel;
using Firebase.Database.Streaming;
using System.Linq;
using System.Collections.Generic;
using TinyShopping.Core.Net.Interface;
using TinyShopping.ApplicationModels;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace TinyShopping.Core.Extensions
{
    public static class ShoppingExtensions
    {
        public static T FindExisting<T>(this IEnumerable<T> lists, FirebaseEvent<T> evt) where T : IHasId
        {
            return lists.FirstOrDefault(d => d.Id == evt.Key);
        }

        public static void Replace<T>(this ObservableCollection<T> lists, T oldList, FirebaseObject<T> newList)
        {
            var idx = lists.IndexOf(oldList);
            lists.Remove(oldList);
            lists.InsertWithId(idx, newList);
        }

        //public static ShoppingList GetShoppingList(this IHasId data, IEnumerable<ShoppingList> lists)
        //{
        //    ShoppingList list = null;
        //    if (data is Item item)
        //    {
        //        list = lists.FirstOrDefault(d => d.Id == item.ListId);
        //    }
        //    else if (data is ShoppingList datalist)
        //    {
        //        list = datalist;
        //    }
        //    return list;
        //}

        public static void AddWithId<T>(this ObservableCollection<T> list, FirebaseObject<T> evt)
        {
            if (evt.Object is IHasId hasId)
            {
                hasId.Id = evt.Key;
            }
            list.Add(evt.Object);
        }

        public static void InsertWithId<T>(this ObservableCollection<T> list, int index, FirebaseObject<T> evt)
        {
            if (evt.Object is IHasId hasId)
            {
                hasId.Id = evt.Key;
            }
            list.Insert(index, evt.Object);
        }

        public static async Task<T> AddOrUpdate<T>(this FirebaseClient client, string key, T data) where T : IHasId
        {
            T ret = data;
            if (string.IsNullOrEmpty(data.Id))
            {
                try
                {
                    var fret = await client
                      .Child(key)
                            .PostAsync(data);
                    fret.Object.Id = fret.Key;
                    ret = fret.Object;
                }
                catch (Exception ex)
                {
                    var i = 2;
                }
            }
            else
            {
                await client.Child(key)
                             .Child(data.Id)
                             .PutAsync(data);
            }
            return ret;
        }
    }
}
