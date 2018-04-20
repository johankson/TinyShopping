using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TinyEditor;
using TinyHelper;
using TinyShopping.Core.Net.Interface;
using Newtonsoft.Json;

namespace TinyShopping.ApplicationModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ShoppingList : IShoppingList, IHasId
    {
        public ShoppingList()
        {
            Items.CollectionChanged += (sender, e) => {
                if (Items.Any())
                {
                    NumberOfItems = Items.Count;
                    NumberOfCompletedItems = Items.Count(d => d.Completed);
                }
            };
        }
        public string Id { get; set; }

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

        [JsonIgnore]
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        int numberOfItems;

        [Editor("Number of items", "Stats", Readonly = true)]
        [JsonIgnore]
        public int NumberOfItems
        {
            get
            {
                return numberOfItems;
            }

            set
            {
                numberOfItems = value;
            }
        }

        int numberOfCompletedItems;

        [Editor("Number of compleded items", "Stats", Readonly = true)]
        [JsonIgnore]
        public int NumberOfCompletedItems
        {
            get
            {
                return numberOfCompletedItems;
            }

            set
            {
                numberOfCompletedItems = value;
            }
        }

        public string NumberOfItemsChecked => $"{NumberOfCompletedItems}/{NumberOfItems} items checked";

        public override string ToString()
        {
            return Name;
        }
    }
}
