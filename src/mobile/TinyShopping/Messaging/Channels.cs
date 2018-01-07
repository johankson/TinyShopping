using System;
namespace TinyShopping.Messaging
{
    public static class Channels
    {
        public const string ShoppingListAdded = "shopping-list-added";
        public const string ShoppingListDeleted = "shopping-list-deleted";

        public const string ShoppingListItemAdded = "shopping-list-item-added";
        public const string ShoppingListItemDeleted = "shopping-list-item-deleted";
    }
}
