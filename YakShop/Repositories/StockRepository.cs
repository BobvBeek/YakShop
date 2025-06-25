using Microsoft.EntityFrameworkCore;
using YakShop.DB;
using YakShop.Entities;
using YakShop.Repositories.Interfaces;

namespace YakShop.Repositories
{
    public class StockRepository : IStockRepository
    {

        private readonly YakDbContext _context;

        public StockRepository(YakDbContext context)
        {
            _context = context;
        }

        // Method to add a new stock
        public async Task AddStockAsync(Stock stock)
        {
            _context.Stock.Add(stock);
            await _context.SaveChangesAsync();
        }

        // Method to update a stock
        public async Task UpdateStockAsync(Stock stock)
        {
            _context.Stock.Update(stock);
            await _context.SaveChangesAsync();
        }

        // Method to get all stocks
        public async Task<Stock> GetStockAsync()
        {
            return await _context.Stock.FirstOrDefaultAsync();
        }

        // Method to get a stock by ID
        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stock.FindAsync(id);
        }

        // Method to delete a stock by ID
        public async Task DeleteStockAsync(int id)
        {
            var stock = await GetStockByIdAsync(id);
            if (stock != null)
            {
                _context.Stock.Remove(stock);
                await _context.SaveChangesAsync();
            }
        }

        //Method to remove all stocks
        public async Task RemoveAllStock()
        {
            _context.Stock.RemoveRange(_context.Stock);
            await _context.SaveChangesAsync();
        }
    }
}
