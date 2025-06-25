using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Entities;
using YakShop.Services;

namespace YakShop.Services
{
    public class Restocker
    {
        // Simulates the stock for a given day using the YakSimulator service
        public Stock Restock(List<Yak> yaks, Stock stock, [FromServices] YakSimulator simulator)
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
