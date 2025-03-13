using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VideoSpelShop.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>(); // Relationship with Orders
    }
}