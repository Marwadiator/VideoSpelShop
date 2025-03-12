using Microsoft.EntityFrameworkCore;
using VideoSpelShop.Models;

namespace VideoSpelShop.Data
{
    public class VideoSpelShopDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }  // Represents the Games table

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 🔹 Replace "SALAH54ALBIZRIH\\SQLEXPRESS01" with your actual SQL Server name
            optionsBuilder.UseSqlServer("Server=SALAH54ALBIZRIH\\SQLEXPRESS01;Database=VideoSpelShop;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}