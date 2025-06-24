using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Api.Entities;
using YakShop.Api.Services;

namespace YakShop.Services
{
    public class StockSimulator
    {
        // Simulates the stock for a given day using the YakSimulator service
        public Stock SimulateStock(List<LabYak> yaks, Stock stock, [FromServices] YakSimulator simulator)
        {
            Random random = new Random();
            var day = random.Next(30, 91);
            var result = simulator.Simulate(yaks, day);

            return new Stock
            {
                Day = day,
                Milk = result.TotalMilk,
                Skins = result.TotalSkins,
                LastUpdate = DateTime.Now
            };
        }
    }
}
