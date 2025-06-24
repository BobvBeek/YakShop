using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Api.DB;
using YakShop.Api.Entities;
using YakShop.Api.Models;
using YakShop.Api.Services;
using YakShop.Repositories.Interfaces;
using YakShop.Repositories.Repositories;

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
        public async Task<IActionResult> LoadHerd([FromBody] LoadHerdRequest request)
        {
            await _orderRepo.RemoveAllOrders();
            await _stockRepo.RemoveAllStock();
            await _yakRepo.RemoveAllYaks();

            var newYaks = request.Herd.Select(dto => new Yak
            {
                Name = dto.Name,
                AgeInYears = dto.Age,
                Sex = dto.Sex,
                AgeLastShavedInDays = dto.Age * 100
            }).ToList();

            await _yakRepo.AddMultipleYaksAsync(newYaks);

            return StatusCode(205);
        }

        // Returns the original herd of yaks without any simulation.
        [HttpGet]
        public async Task<IActionResult> GetOriginalHerd()
        {
            var herd = await _yakRepo.GetAllYaksAsync();
            return Ok(new { herd });
        }
    }
}
