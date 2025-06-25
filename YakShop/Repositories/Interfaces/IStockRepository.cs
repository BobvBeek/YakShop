using YakShop.Entities;

namespace YakShop.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock> GetStockAsync();
        Task<Stock?> GetStockByIdAsync(int id);
        Task AddStockAsync(Stock stock);
        Task UpdateStockAsync(Stock stock);
        Task DeleteStockAsync(int id);
        Task RemoveAllStock();
    }
}
