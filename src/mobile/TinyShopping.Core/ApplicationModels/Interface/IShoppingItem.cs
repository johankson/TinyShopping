using System;

namespace TinyShopping.Core.Net.Interface
{
    public interface IShoppingItem
    {
        int Id { get; set; }

        DateTime Added { get; set; }

        DateTime Done { get; set; }

        string Name { get; set; }

        bool Completed { get; set; }

        int StoreID { get; set; }

        int ListId { get; set; }

        double Lat { get; set; }

        double Lng { get; set; }

        bool PriceExists { get; set; }

        double Price { get; set; }

        string Barcode { get; set; }
    }
}