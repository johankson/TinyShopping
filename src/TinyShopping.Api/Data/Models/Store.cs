using System.ComponentModel.DataAnnotations;
using TinyShopping.Api.Extensions;

namespace TinyShopping.Api.Data.Models
{
    public class Store : IGeoLocation
    {
        [Key, Copy(Exclude = true)]
        public int ID { get; set; }
        public string Chain { get; set; }
        public string Notes { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
