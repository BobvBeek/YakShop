using YakShop.Entities;
using YakShop.DTOs;

namespace YakShop.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToDto(Stock stock) =>
            new StockDto
            {
                Day = stock.Day,
                Milk = stock.Milk,
                Skins = stock.Skins
            };

        public static Stock FromDto(StockDto dto) =>
            new Stock
            {
                Day = dto.Day,
                Milk = dto.Milk,
                Skins = dto.Skins,
                LastUpdate = DateTime.UtcNow
            };
    }
}
