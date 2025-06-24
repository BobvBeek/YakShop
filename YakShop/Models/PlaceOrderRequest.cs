using YakShop.Models;

namespace YakShop.Api.Models
{
    public class PlaceOrderRequest
    {
        public string Customer { get; set; }
        public OrderContent Order { get; set; }
    }
}
