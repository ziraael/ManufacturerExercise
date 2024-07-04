using AssemblyService.Infrastructure.Context.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssemblyService.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<AssemblyProduct> AssemblyProducts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}