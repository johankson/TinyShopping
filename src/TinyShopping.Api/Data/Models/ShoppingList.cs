using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Data.Models
{
    public class ShoppingList
    {
        [Key, Copy(Exclude = true)]
        [Required]
        public int ID { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public bool Completed { get; set; }
        [Required]
        public DateTime Done { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int StoreID { get; set; }
        
        [NotMapped, Required]
        public int NumberOfItems { get; set; }
        [NotMapped, Required]
        public int NumberOfCompletedItems { get; set; }
    }
}
