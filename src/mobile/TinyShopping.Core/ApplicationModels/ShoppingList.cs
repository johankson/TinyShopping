using TinyEditor;
using TinyShopping.Core.Net.Interface;

namespace TinyShopping.ApplicationModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ShoppingList : IShoppingList
    {
        public int Id { get; set; }

        [Editor("Completed", "List data", Readonly = true)]
        public System.DateTime Created { get; set; }

        [Editor("Marked as complete", "List data")]
        public bool Completed { get; set; }

        [Editor("Completed at", "List data", Readonly = true)]
        public System.DateTime Done { get; set; }

        [Editor("Name", "List data")]
        public string Name { get; set; }

        public int StoreID { get; set; }
    }
}
