using WarehouseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
namespace WarehouseService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<AssembledVehicleStock> AssembledVehicleStocks { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if (!databaseCreator.CanConnect())
                    {
                        databaseCreator.Create();
                    }

                    if (!databaseCreator.HasTables())
                    {
                        databaseCreator.CreateTables();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var warehouse = new Warehouse
            {
                Id = new Guid("0145583a-8e54-4295-b8ba-01fc2d86edfa"),
                Name = "Warehouse 1",
                Location = "Kosovo"
            };

            modelBuilder.Entity<Warehouse>().HasData(warehouse);
        }
    }
}