// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace TinyShopping.Core.Net
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for ShoppingAPI.
    /// </summary>
    public static partial class ShoppingAPIExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<ShoppingList> GetShoppingLists(this IShoppingAPI operations)
            {
                return operations.GetShoppingListsAsync().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<ShoppingList>> GetShoppingListsAsync(this IShoppingAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetShoppingListsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='listData'>
            /// </param>
            public static void AddShoppingList(this IShoppingAPI operations, ShoppingList listData = default(ShoppingList))
            {
                operations.AddShoppingListAsync(listData).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='listData'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task AddShoppingListAsync(this IShoppingAPI operations, ShoppingList listData = default(ShoppingList), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.AddShoppingListWithHttpMessagesAsync(listData, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static ShoppingList GetShoppingList(this IShoppingAPI operations, int id)
            {
                return operations.GetShoppingListAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ShoppingList> GetShoppingListAsync(this IShoppingAPI operations, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetShoppingListWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listData'>
            /// </param>
            public static void UpdateShoppingList(this IShoppingAPI operations, int id, ShoppingList listData = default(ShoppingList))
            {
                operations.UpdateShoppingListAsync(id, listData).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listData'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateShoppingListAsync(this IShoppingAPI operations, int id, ShoppingList listData = default(ShoppingList), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateShoppingListWithHttpMessagesAsync(id, listData, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static void DeleteShoppingList(this IShoppingAPI operations, int id)
            {
                operations.DeleteShoppingListAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteShoppingListAsync(this IShoppingAPI operations, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteShoppingListWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            public static IList<Item> GetListItems(this IShoppingAPI operations, int id, string listid)
            {
                return operations.GetListItemsAsync(id, listid).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Item>> GetListItemsAsync(this IShoppingAPI operations, int id, string listid, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetListItemsWithHttpMessagesAsync(id, listid, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='listid'>
            /// </param>
            /// <param name='itemData'>
            /// </param>
            public static void AddListItem(this IShoppingAPI operations, string listid, Item itemData = default(Item))
            {
                operations.AddListItemAsync(listid, itemData).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='listid'>
            /// </param>
            /// <param name='itemData'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task AddListItemAsync(this IShoppingAPI operations, string listid, Item itemData = default(Item), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.AddListItemWithHttpMessagesAsync(listid, itemData, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            public static Item GetListItem(this IShoppingAPI operations, int id, string listid)
            {
                return operations.GetListItemAsync(id, listid).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Item> GetListItemAsync(this IShoppingAPI operations, int id, string listid, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetListItemWithHttpMessagesAsync(id, listid, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            /// <param name='itemData'>
            /// </param>
            public static void UpdateListItem(this IShoppingAPI operations, int id, string listid, Item itemData = default(Item))
            {
                operations.UpdateListItemAsync(id, listid, itemData).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            /// <param name='itemData'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateListItemAsync(this IShoppingAPI operations, int id, string listid, Item itemData = default(Item), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateListItemWithHttpMessagesAsync(id, listid, itemData, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            public static void DeleteListItem(this IShoppingAPI operations, int id, string listid)
            {
                operations.DeleteListItemAsync(id, listid).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='listid'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteListItemAsync(this IShoppingAPI operations, int id, string listid, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteListItemWithHttpMessagesAsync(id, listid, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='user'>
            /// </param>
            /// <param name='password'>
            /// </param>
            public static IdentityResult ApiUserAdduserByUserGet(this IShoppingAPI operations, string user, string password = default(string))
            {
                return operations.ApiUserAdduserByUserGetAsync(user, password).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='user'>
            /// </param>
            /// <param name='password'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IdentityResult> ApiUserAdduserByUserGetAsync(this IShoppingAPI operations, string user, string password = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiUserAdduserByUserGetWithHttpMessagesAsync(user, password, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='user'>
            /// </param>
            /// <param name='password'>
            /// </param>
            public static string ApiUserLoginByUserGet(this IShoppingAPI operations, string user, string password = default(string))
            {
                return operations.ApiUserLoginByUserGetAsync(user, password).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='user'>
            /// </param>
            /// <param name='password'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> ApiUserLoginByUserGetAsync(this IShoppingAPI operations, string user, string password = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiUserLoginByUserGetWithHttpMessagesAsync(user, password, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
