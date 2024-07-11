using Microsoft.EntityFrameworkCore;
using ChassisService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ChassisService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Chassis> Chasses { get; set; } = null!;
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
        }
    }
}