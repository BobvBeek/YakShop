using YakShop.Models;

namespace YakShop.Models
{
    public class PlaceOrderRequest
    {
        public string Customer { get; set; }
        public OrderContent Order { get; set; }
    }
}
