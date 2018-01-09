using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TinyEditor;
using TinyHelper;
using TinyShopping.Core.Net.Interface;

namespace TinyShopping.ApplicationModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ShoppingList : IShoppingList, IOfflineSupport
    {
        public int Id { get; set; }

        [Editor("Created", "Information", Readonly = true)]
        public System.DateTime Created { get; set; } = System.DateTime.Now;

        bool completed;
        [Editor("Marked as complete", "Required")]
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

        [Editor("Completed at", "Information", Readonly = true)]
        public System.DateTime Done { get; set; }

        [Editor("Name", "Required", Order = 1)]
        public string Name { get; set; }

        public int StoreID { get; set; }

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        [Editor("Number of items", "Stats", Readonly = true)]
        public int NumberOfItems { get; set; }

        [Editor("Number of compleded items", "Stats", Readonly = true)]
        public int NumberOfCompletedItems { get; set; }

        public string NumberOfItemsChecked => $"{NumberOfCompletedItems}/{NumberOfItems} items checked";

        [Copy(Exclude = true), Editor("Last sync", "Sync", Readonly = true)]
        public DateTime LastSync { get; set; }

        [Copy(Exclude = true), Editor("Needs sync", "Sync", Readonly = true)]
        public bool NeedSync { get; set; }

        [Copy(Exclude = true)]
        public bool Deleted { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
