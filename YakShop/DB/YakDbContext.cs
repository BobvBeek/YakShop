using Microsoft.EntityFrameworkCore;
using YakShop.Entities;

namespace YakShop.DB;

public class YakDbContext : DbContext
{
    public YakDbContext(DbContextOptions<YakDbContext> options) : base(options) { }
    public DbSet<Yak> Yaks { get; set; }
    public DbSet<Stock> Stock { get; set; }
    public DbSet<Order> Orders { get; set; }
}
