namespace TinyShopping.Core.Net.Interface
{
    public interface IShoppingList
    {
        int Id { get; set; }

        System.DateTime Created { get; set; }

        bool Completed { get; set; }

        System.DateTime Done { get; set; }

        string Name { get; set; }

        int StoreID { get; set; }

        int NumberOfItems { get; set; }

        int NumberOfCompletedItems { get; set; }
    }
}
