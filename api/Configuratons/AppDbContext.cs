
using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Configurations
{
    /// <summary>
    /// The AppDbContext class represents the database context for the application.
    /// </summary>
    /// 
    /// <remarks>
    /// The AppDbContext class is responsible for managing the connection to the database
    /// and providing access to the various entities in the database, such as orders, products,
    /// users, credentials, reviews, notifications, and tokens.
    /// It also configures the database and defines the primary keys for each entity.
    /// </remarks>
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