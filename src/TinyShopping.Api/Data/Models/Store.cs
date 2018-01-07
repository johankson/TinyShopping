using System.ComponentModel.DataAnnotations;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Data.Models
{
    public class Store : IGeoLocation
    {
        [Key, Copy(Exclude = true)]
        [Required]
        public int ID { get; set; }
        [Required]
        public string Chain { get; set; }
        [Required]
        public string Notes { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Lat { get; set; }
        [Required]
        public double Lng { get; set; }
    }
}
