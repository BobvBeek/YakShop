using YakShop.DTOs;
using YakShop.Entities;

namespace YakShop.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(Order order) => new OrderDto
        {
            Customer = order.Customer,
            MilkOrdered = order.MilkOrdered,
            SkinsOrdered = order.SkinsOrdered
        };

        public static Order FromDto(OrderDto dto) => new Order
        {
            Customer = dto.Customer,
            MilkOrdered = dto.MilkOrdered,
            SkinsOrdered = dto.SkinsOrdered
        };
    }
}
