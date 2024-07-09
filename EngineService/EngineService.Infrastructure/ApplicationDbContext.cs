﻿using Microsoft.EntityFrameworkCore;
using EngineService.Domain.Entities;

namespace EngineService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Engine> Engines { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}