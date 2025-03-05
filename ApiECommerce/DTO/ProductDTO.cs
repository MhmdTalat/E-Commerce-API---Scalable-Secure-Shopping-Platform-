using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api_ECommerce.Models;

namespace Api_ECommerce.DTO
{
    public class ProductDTO
    {
        public class ProductDto
        {
            
            [Required(ErrorMessage = "Product name is required.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Description is required.")]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

             

            [Required(ErrorMessage = "Image file is required.")]
            public IFormFile ImageFile { get; set; } // Image upload

            [Required(ErrorMessage = "Category ID is required.")]
            public int CategoryId { get; set; }

            public string CategoryName { get; set; } // Extra field for display
        }
    }

}
