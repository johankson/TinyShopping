using TinyShopping.Core.Net.Interface;
using TinyEditor;
using System;
using TinyHelper;

namespace TinyShopping.ApplicationModels
{
    public class BarCodeHandler
    {

    }

    public class Item : IShoppingItem, IHasId
    {
        public string Id { get; set; }

        [Editor("Added", "Information", Readonly = true)]
        public System.DateTime Added { get; set; } = System.DateTime.Now;

        [Editor("Completed", "Information", Readonly = true)]
        public System.DateTime Done { get; set; }

        [Editor("Name", "Required", Order = 10)]
        public string Name { get; set; }

        [Editor("Marked as complete", "Required", Order = 11)]
        public bool Completed { get; set; }

        //[Editor("Store", "List data", RelationTo = typeof())]
        public int StoreID { get; set; }

        //[Editor("Move to list", "Information", RelationTo = typeof(ShoppingListRelation))]
        public string ListId { get; set; }

        public bool PriceExists { get; set; }

        [Editor("Price", "After purchase")]
        public double Price { get; set; }

        [Editor("Barcode", "After purchase", CustomHandler = typeof(BarCodeHandler))]
        public string Barcode { get; set; } = string.Empty;

        [Editor("Latitude", "Location", Readonly = true)]
        public double Lat { get; set; }

        [Editor("Longitude", "Location", Readonly = true)]
        public double Lng { get; set; }
    }
}
