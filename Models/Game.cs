using System.ComponentModel.DataAnnotations;

namespace VideoSpelShop.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }  // Allows NULL

        [Required]
        public string? Genre { get; set; }  // Allows NULL

        public decimal? Price { get; set; }  // Allows NULL
    }
}