using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api_ECommerce.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        // 🔹 Foreign key for Cart
        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        // 🔹 Foreign key for Product
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // 🔹 Quantity of the product in the cart
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // 🔹 User ID to track the owner of the cart item
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
