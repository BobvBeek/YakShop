using Microsoft.EntityFrameworkCore;
using YakShop.DB;
using YakShop.Entities;
using YakShop.Repositories.Interfaces;

namespace YakShop.Repositories
{
    public class YakRepository : IYakRepository
    {
        private readonly YakDbContext _context;

        public YakRepository(YakDbContext context)
        {
            _context = context;
        }

        // Method to add a new order
        public async Task AddYakAsync(Yak yak)
        {
            _context.Yaks.Add(yak);
            await _context.SaveChangesAsync();
        }
        // Method for adding multiple yaks at once
        public async Task AddMultipleYaksAsync(List<Yak> yaks)
        {
            _context.Yaks.AddRange(yaks);
            await _context.SaveChangesAsync();
        }

        // Method to update an order
        public async Task UpdateYakAsync(Yak yak)
        {
            _context.Yaks.Update(yak);
            await _context.SaveChangesAsync();
        }

        // Method to get all orders
        public async Task<List<Yak>> GetAllYaksAsync()
        {
            return await _context.Yaks.ToListAsync();
        }

        // Method to get an order by ID
        public async Task<Yak?> GetYakByIdAsync(int id)
        {
            return await _context.Yaks.FindAsync(id);
        }

        // Method to delete an order by ID
        public async Task DeleteYakAsync(int id)
        {
            var yak = await GetYakByIdAsync(id);
            if (yak != null)
            {
                _context.Yaks.Remove(yak);
                await _context.SaveChangesAsync();
            }
        }

        // Method to remove all yaks
        public async Task RemoveAllYaks()
        {
            _context.Yaks.RemoveRange(_context.Yaks);
            await _context.SaveChangesAsync();
        }
    }
}
