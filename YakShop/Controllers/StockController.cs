using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.DB;
using YakShop.Entities;
using YakShop.Models;
using YakShop.Services;
using YakShop.Mappers;
using YakShop.Repositories.Interfaces;

namespace YakShop.Controllers
{
    [ApiController]
    [Route("stock")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly IYakRepository _yakRepo;

        public StockController(IStockRepository stockRepo, IYakRepository yakRepo)
        {
            _stockRepo = stockRepo;
            _yakRepo = yakRepo;
        }

        // Endpoint to get stock for a specific day
        [HttpGet("{day}")]
        public async Task<IActionResult> GetStock(int day, [FromServices] YakSimulator yakSim)
        {
            var yaks = await _yakRepo.GetAllYaksAsync();
            var result = yakSim.Simulate(yaks, day);
            var stock = new Stock
            {
                Day = day,
                Milk = result.TotalMilk,
                Skins = result.TotalSkins,
                LastUpdate = DateTime.Now
            };

            return Ok(StockMapper.ToDto(stock));
        }

        // Endpoint to get the current stock, simulating if necessary if no stock exists or if the last update was more than 5 minutes ago
        [HttpGet]
        public async Task<IActionResult> GetStock([FromServices] YakSimulator yakSim, [FromServices] Restocker restocker)
        {
            var stock = await _stockRepo.GetStockAsync();
            var yaks = await _yakRepo.GetAllYaksAsync();
            var stockUpdateTimer = 5;

            // If no stock exists or the last update was more than 5 minutes ago, generate new stock
            if ((stock == null) || ((DateTime.Now - stock.LastUpdate).TotalMinutes > stockUpdateTimer))
            {         
                var result = restocker.Restock(yaks, stock, yakSim);

                await _stockRepo.RemoveAllStock(); // Clear existing stock before adding new one
                await _stockRepo.AddStockAsync(result);

                return Ok(StockMapper.ToDto(result));
            }

            await _stockRepo.UpdateStockAsync(stock);

            return Ok(StockMapper.ToDto(stock));
        }
    }
}
