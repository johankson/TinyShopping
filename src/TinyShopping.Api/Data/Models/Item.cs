using System;
using System.ComponentModel.DataAnnotations;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Data.Models
{

    public class Item : IGeoLocation
    {
        [Key, Copy(Exclude = true)]
        public int ID { get; set; }
        public DateTime Added { get; set; }
        public DateTime Done { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public int StoreID { get; set; }
        public int ListId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public bool PriceExists { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }

    }
}
