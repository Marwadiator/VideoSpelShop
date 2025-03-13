using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoSpelShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Game> Games { get; set; } = new List<Game>();  // Relationship with Games
    }
}