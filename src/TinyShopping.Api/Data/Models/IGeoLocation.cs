namespace TinyShopping.Api.Data.Models
{
    public interface IGeoLocation
    {
        double Lat { get; set; }
        double Lng { get; set; }
    }
}