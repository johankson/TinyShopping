using TinyShopping.Core.Net.Interface;
using TinyEditor;

namespace TinyShopping.ApplicationModels
{
    public class BarCodeHandler
    {

    }

    public class Item : IShoppingItem
    {
        public int Id { get; set; }

        [Editor("Added", "List data", Readonly = true)]
        public System.DateTime Added { get; set; }

        [Editor("Completed", "List data", Readonly = true)]
        public System.DateTime Done { get; set; }

        [Editor("Name", "List data")]
        public string Name { get; set; }

        [Editor("Marked as complete", "List data")]
        public bool Completed { get; set; }

        //[Editor("Store", "List data", RelationTo = typeof())]
        public int StoreID { get; set; }

        [Editor("Store", "List data", RelationTo = typeof(ShoppingListRelation))]
        public int ListId { get; set; }


        public double Lat { get; set; }


        public double Lng { get; set; }

        public bool PriceExists { get; set; }

        [Editor("Price", "List data")]
        public double Price { get; set; }

        [Editor("Barcode", "List data", CustomHandler = typeof(BarCodeHandler))]
        public string Barcode { get; set; } = string.Empty;


    }
}
