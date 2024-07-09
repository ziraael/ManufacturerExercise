using Microsoft.EntityFrameworkCore;
using OptionPackService.Domain.Entities;

namespace OptionPackService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<OptionPack> OptionPacks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}