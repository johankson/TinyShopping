using System;
using System.Collections.Generic;
using TinyHelper;
using TinyShopping.Core.Net.Interface;

namespace TinyShopping.Core.ApplicationModels.RestHelper
{
    public static class Converters
    {
        public static IList<TinyShopping.ApplicationModels.ShoppingList> ToModel(this IEnumerable<Net.Models.ShoppingList> list)
        {
            var ret = new List<TinyShopping.ApplicationModels.ShoppingList>();
            foreach (var item in list)
            {
                ret.Add(item.ToModel());
            }
            return ret;
        }

        public static TinyShopping.ApplicationModels.ShoppingList ToModel(this IShoppingList item)
        {
            var ret = new TinyShopping.ApplicationModels.ShoppingList();
            item.MemberviseCopyTo(ret);
            return ret;
        }

        public static IList<TinyShopping.ApplicationModels.Item> ToModel(this IEnumerable<Net.Models.Item> list)
        {
            var ret = new List<TinyShopping.ApplicationModels.Item>();
            foreach (var item in list)
            {
                ret.Add(item.ToModel());
            }
            return ret;
        }

        public static TinyShopping.ApplicationModels.Item ToModel(this IShoppingItem item)
        {
            var ret = new TinyShopping.ApplicationModels.Item();
            item.MemberviseCopyTo(ret);
            return ret;
        }


        public static IList<Net.Models.ShoppingList> ToRest(this IList<TinyShopping.ApplicationModels.ShoppingList> list)
        {
            var ret = new List<Net.Models.ShoppingList>();
            foreach (var item in list)
            {
                ret.Add(item.ToRest());
            }
            return ret;
        }

        public static Net.Models.ShoppingList ToRest(this TinyShopping.ApplicationModels.ShoppingList item)
        {
            var ret = new Net.Models.ShoppingList();
            item.MemberviseCopyTo(ret);
            return ret;
        }

        public static IList<Net.Models.Item> ToRest(this IList<TinyShopping.ApplicationModels.Item> list)
        {
            var ret = new List<Net.Models.Item>();
            foreach (var item in list)
            {
                ret.Add(item.ToRest());
            }
            return ret;
        }

        public static Net.Models.Item ToRest(this TinyShopping.ApplicationModels.Item item)
        {
            var ret = new Net.Models.Item();
            item.MemberviseCopyTo(ret);
            return ret;
        }
    }
}
