using System;
using TinyEditor;
using TinyShopping.Core.Net.Interface;

namespace TinyShopping.ApplicationModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ShoppingList : IShoppingList
    {
        public int Id { get; set; }

        [Editor("Created", "List data", Readonly = true)]
        public System.DateTime Created { get; set; } = System.DateTime.Now;

        bool completed;
        [Editor("Marked as complete", "List data")]
        public bool Completed
        {
            get
            {
                return completed;
            }

            set
            {
                completed = value;

                if (value == true)
                {
                    Done = System.DateTime.Now;
                }
            }
        }

        [Editor("Completed at", "List data", Readonly = true)]
        public System.DateTime Done { get; set; }

        [Editor("Name", "List data", Order = 1)]
        public string Name { get; set; }
       

        public int StoreID { get; set; }

        [Editor("Number of items", "Stats", Readonly = true)]
        public int NumberOfItems { get; set; }

        public int NumberOfCompletedItems { get; set; }

<<<<<<< HEAD
        public string NumberOfItemsChecked => $"{NumberOfCompletedItems}/{NumberOfItems} items checked";
=======
        public string ItemStats { get { return $"{NumberOfCompletedItems}/{NumberOfItems} items checked"; } }
>>>>>>> 7e514eecf41f26854540df6b1d6c60943ac12b64

        public override string ToString()
        {
            return Name;
        }
    }
}
