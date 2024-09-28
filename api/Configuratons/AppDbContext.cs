using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Configurations
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; init; }
        public DbSet<Product> Products { get; init; }
        public DbSet<User> Users { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}