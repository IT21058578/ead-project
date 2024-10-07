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
        public DbSet<Credential> Credentials { get; init; }
        public DbSet<Review> Reviews { get; init; }
        public DbSet<Notification> Notifications { get; init; }
        public DbSet<Token> Tokens { get; init; }

        //  This method is called by the runtime. Use this method to configure the database (and other options) to be used for this context.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Credential>().HasKey(c => c.Id);
            modelBuilder.Entity<Review>().HasKey(r => r.Id);
            modelBuilder.Entity<Notification>().HasKey(n => n.Id);
            modelBuilder.Entity<Token>().HasKey(t => t.Id);
        }
    }
}