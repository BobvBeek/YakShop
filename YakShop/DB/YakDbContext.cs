using Microsoft.EntityFrameworkCore;
using YakShop.Api.Entities;

namespace YakShop.Api.DB;

public class YakDbContext : DbContext
{
    public YakDbContext(DbContextOptions<YakDbContext> options) : base(options) { }
    public DbSet<Yak> LabYaks { get; set; }
    public DbSet<Stock> Stock { get; set; }
    public DbSet<Order> Orders { get; set; }
}
