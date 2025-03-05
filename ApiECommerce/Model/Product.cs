using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Api_ECommerce.Models
{
  
        public class Product
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Precision(18, 2)]
            public decimal Price { get; set; }

            public string ImageUrl { get; set; } // Stores the image file path

            [NotMapped] // Prevents EF from persisting this in the database
            public IFormFile? ImageFile { get; set; }

            [ForeignKey("CategoryId")]
            public int CategoryId { get; set; }
        [JsonIgnore]  // Prevents serialization loop
        public Category Category { get; set; }
        // Navigation Property for Many-to-Many Relationship
        [JsonIgnore]
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}

 
