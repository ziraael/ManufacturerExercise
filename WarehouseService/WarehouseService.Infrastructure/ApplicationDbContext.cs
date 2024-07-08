using Microsoft.EntityFrameworkCore;
using WarehouseService.Domain.Entities;

namespace WarehouseService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<AssembledVehicleStock> AssembledVehicleStocks { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}