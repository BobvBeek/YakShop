namespace YakShop.Api.Entities;

public class Stock
{
    public int Id { get; set; }
    public int Day { get; set; }
    public double Milk { get; set; }
    public int Skins { get; set; }
    public DateTime LastUpdate { get; set; }
}