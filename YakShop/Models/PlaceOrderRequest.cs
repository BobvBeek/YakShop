namespace YakShop.Api.Models
{
    public class PlaceOrderRequest
    {
        public string Customer { get; set; }
        public OrderContent Order { get; set; }
    }

    public class OrderContent
    {
        public double? Milk { get; set; }
        public int? Skins { get; set; }
    }
}
