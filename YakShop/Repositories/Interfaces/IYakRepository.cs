using YakShop.Entities;

namespace YakShop.Repositories.Interfaces
{
    public interface IYakRepository
    {
        Task<List<Yak>> GetAllYaksAsync();
        Task<Yak?> GetYakByIdAsync(int id);
        Task AddYakAsync(Yak yak);
        Task AddMultipleYaksAsync(List<Yak> yaks);
        Task UpdateYakAsync(Yak yak);
        Task DeleteYakAsync(int id);
        Task RemoveAllYaks();
    }
}
