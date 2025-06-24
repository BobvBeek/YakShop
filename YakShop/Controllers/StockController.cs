using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Api.DB;
using YakShop.Api.Entities;
using YakShop.Api.Models;
using YakShop.Api.Services;


namespace YakShop.Controllers
{
    [ApiController]
    [Route("stock")]
    public class StockController : ControllerBase
    {
        private readonly YakDbContext _context;

        public StockController(YakDbContext context)
        {
            _context = context;
        }

        //Generates a random day between 30 and 90
        private int GenerateRandomDay()
        {
            Random random = new Random();
            return random.Next(30, 91);
        }

        // Simulates the stock for a given day using the YakSimulator service
        private async Task<Stock> SimulateStock(int day, YakSimulator simulator)
        {
            var yaks = await _context.LabYaks.ToListAsync();
            var result = simulator.Simulate(yaks, day);
            return new Stock
            {
                Day = day,
                Milk = result.TotalMilk,
                Skins = result.TotalSkins,
                LastUpdate = DateTime.Now
            };
        }

        // Endpoint to get stock for a specific day
        [HttpGet("{day}")]
        public async Task<IActionResult> GetStock(int day, [FromServices] YakSimulator simulator)
        {
            var result = await SimulateStock(day, simulator);

            return Ok(result);
        }

        // Endpoint to get the current stock, simulating if necessary if no stock exists or if the last update was more than 5 minutes ago
        [HttpGet]
        public async Task<IActionResult> GetStock([FromServices] YakSimulator simulator)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync();

            if (stock == null)
            {
                var result = await SimulateStock(GenerateRandomDay(), simulator);

                _context.Stock.Add(result);
                await _context.SaveChangesAsync();

                return Ok(result);
            }

            var stockUpdateTimer = 5; // Number of minites after which a new day stock is generated

            if ((DateTime.Now - stock.LastUpdate).TotalMinutes > stockUpdateTimer)
            {
                var result = await SimulateStock(GenerateRandomDay(), simulator);

                stock.Skins = result.Skins;
                stock.Milk = result.Milk; 
                stock.LastUpdate = DateTime.Now;
                stock.Day = result.Day;
            }

            await _context.SaveChangesAsync();

            return Ok(stock);
        }
    }
}
