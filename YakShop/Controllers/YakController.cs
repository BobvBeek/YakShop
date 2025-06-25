using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.DB;
using YakShop.DTOs;
using YakShop.Entities;
using YakShop.Mappers;
using YakShop.Models;
using YakShop.Repositories.Interfaces;
using YakShop.Services;

namespace YakShop.Controllers
{
    [ApiController]
    [Route("herd")]
    public class YakController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IYakRepository _yakRepo;

        public YakController(IOrderRepository orderRepo, IStockRepository stockRepo, IYakRepository yakRepo)
        {
            _orderRepo = orderRepo;
            _stockRepo = stockRepo;
            _yakRepo = yakRepo;
        }

        // Loads a new herd of yaks from the provided request. Resets the current state by removing all existing orders, stock, and yaks.
        [HttpPost]
        public async Task<IActionResult> LoadHerd([FromBody] List<YakDto> request, [FromServices] Restocker restocker, [FromServices] YakSimulator yakSim)
        {
            await _orderRepo.RemoveAllOrders();
            await _stockRepo.RemoveAllStock();
            await _yakRepo.RemoveAllYaks();

            var newYaks = request.Select(dto => new Yak
            {
                Name = dto.Name,
                AgeInYears = dto.Age,
                Sex = dto.Sex,
                AgeLastShavedInDays = dto.Age * 100
            }).ToList();

            // Create a new stock based on the new herd of yaks
            var result = restocker.Restock(newYaks, new Stock(), yakSim);
            await _stockRepo.AddStockAsync(result);

            //Add the new herd of yaks to the repository
            await _yakRepo.AddMultipleYaksAsync(newYaks);

            return Ok(new { herd = newYaks });
        }

        // Returns the original herd of yaks without any simulation.
        [HttpGet]
        public async Task<IActionResult> GetOriginalHerd()
        {
            var herd = await _yakRepo.GetAllYaksAsync();
            var herdDtos = herd.Select(YakMapper.ToDto).ToList();

            return Ok(new { herd = herdDtos });
        }
    }
}
