using Microsoft.EntityFrameworkCore;
using VideoSpelShop.Models;

namespace VideoSpelShop.Data
{
    public class VideoSpelShopDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

       
        public VideoSpelShopDbContext() { }

        public VideoSpelShopDbContext(DbContextOptions<VideoSpelShopDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=SALAH54ALBIZRIH\\SQLEXPRESS01;Database=VideoSpelShop;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 Add Categories First (Because Games Depend on Them)
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Action" },
                new Category { CategoryId = 2, Name = "Adventure" },
                new Category { CategoryId = 3, Name = "RPG" }
            );

            // 🔹 Add Games (Ensure CategoryId Matches Existing Categories)
            modelBuilder.Entity<Game>().HasData(
                new Game { GameId = 1, Name = "The Witcher 3", Genre = "RPG", Price = 39.99m, CategoryId = 3 },
                new Game { GameId = 2, Name = "Cyberpunk 2077", Genre = "RPG", Price = 59.99m, CategoryId = 3 },
                new Game { GameId = 3, Name = "God of War", Genre = "Action", Price = 49.99m, CategoryId = 1 }
            );
        }
    }
}
 