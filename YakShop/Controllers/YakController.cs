using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YakShop.Api.DB;
using YakShop.Api.Entities;
using YakShop.Api.Models;
using YakShop.Api.Services;

namespace YakShop.Controllers
{
    [ApiController]
    [Route("herd")]
    public class YakController : ControllerBase
    {
        private readonly YakDbContext _context;

        public YakController(YakDbContext context)
        {
            _context = context;
        }

        // Loads a new herd of yaks from the provided request. Resets the current state by removing all existing orders, stock, and yaks.
        [HttpPost]
        public async Task<IActionResult> LoadHerd([FromBody] LoadHerdRequest request)
        {
            _context.Orders.RemoveRange(_context.Orders);
            _context.Stock.RemoveRange(_context.Stock);
            _context.LabYaks.RemoveRange(_context.LabYaks);
            await _context.SaveChangesAsync();

            var newYaks = request.Herd.Select(dto => new LabYak
            {
                Name = dto.Name,
                AgeInYears = dto.Age,
                Sex = dto.Sex,
                AgeLastShavedInDays = dto.Age * 100
            }).ToList();

            _context.LabYaks.AddRange(newYaks);
            await _context.SaveChangesAsync();

            return StatusCode(205);
        }

        // Returns the herd of yaks for a specific day, simulating their age and production using the YakSimulator service.
        [HttpGet("{day}")]
        public async Task<IActionResult> GetHerd(int day, [FromServices] YakSimulator simulator)
        {
            var yaks = await _context.LabYaks.ToListAsync();
            var result = simulator.Simulate(yaks, day);

            return Ok(new
            {
                herd = result.Herd.Select(y => new
                {
                    name = y.Name,
                    age = y.AgeInYears,
                    ageLastShaved = y.AgeLastShaved
                })
            });
        }

        // Returns the original herd of yaks without any simulation.
        [HttpGet]
        public async Task<IActionResult> GetOriginalHerd()
        {
            var herd = await _context.LabYaks.ToListAsync();
            return Ok(new { herd });
        }
    }
}
