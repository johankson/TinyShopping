using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Data.Models
{

    public class Item : IGeoLocation
    {
        [Key, Copy(Exclude = true)]
        [Required]
        public int ID { get; set; }
        [Required]
        public DateTime Added { get; set; }
        [Required]
        public DateTime Done { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Completed { get; set; }
        [Required]
        public int StoreID { get; set; }
        [Required]
        public int ListId { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Lng { get; set; }

        [Required]
        public bool PriceExists { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Barcode { get; set; }

    }
}
