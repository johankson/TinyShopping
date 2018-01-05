using System;
using System.ComponentModel.DataAnnotations;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Data.Models
{
    public class ShoppingList
    {
        [Key, Copy(Exclude = true)]
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public bool Completed { get; set; }
        public DateTime Done { get; set; }
        public string Name { get; set; }
        public int StoreId { get; set; }
    }

    public class Store : IGeoLocation
    {
        [Key, Copy(Exclude = true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Item : IGeoLocation
    {
        [Key, Copy(Exclude = true)]
        public int ID { get; set; }
        public DateTime Added { get; set; }
        public DateTime Done { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public int ListId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
