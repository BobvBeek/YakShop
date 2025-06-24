using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Api.DB;
using YakShop.Api.Entities;
using YakShop.Api.Models;
using YakShop.Api.Services;
using YakShop.Services;


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

        // Endpoint to get stock for a specific day
        [HttpGet("{day}")]
        public async Task<IActionResult> GetStock(int day, [FromServices] YakSimulator yakSim, [FromServices] StockSimulator stockSim)
        {
            var yaks = await _context.LabYaks.ToListAsync();
            var result = yakSim.Simulate(yaks, day);
            var stock = new Stock
            {
                Day = day,
                Milk = result.TotalMilk,
                Skins = result.TotalSkins,
                LastUpdate = DateTime.Now
            };

            return Ok(stock);
        }

        // Endpoint to get the current stock, simulating if necessary if no stock exists or if the last update was more than 5 minutes ago
        [HttpGet]
        public async Task<IActionResult> GetStock([FromServices] YakSimulator yakSim, [FromServices] StockSimulator stockSim)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync();
            var yaks = await _context.LabYaks.ToListAsync();
            var stockUpdateTimer = 5;

            if ((stock == null) || ((DateTime.Now - stock.LastUpdate).TotalMinutes > stockUpdateTimer))
            {         
                var result = stockSim.SimulateStock(yaks, stock, yakSim);
                _context.Stock.RemoveRange(_context.Stock);
                _context.Stock.Add(result);
                await _context.SaveChangesAsync();

                return Ok(result);
            }

            await _context.SaveChangesAsync();

            return Ok(stock);
        }
    }
}
