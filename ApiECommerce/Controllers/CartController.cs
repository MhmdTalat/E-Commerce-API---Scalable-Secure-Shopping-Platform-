using Api_ECommerce.Data;
using Api_ECommerce.DTO;
using Api_ECommerce.Model;
using Api_ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api_ECommerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddToCartController : ControllerBase
    {
        private readonly EshopContext _context;
        private readonly UserManager<Appuser> _userManager;
        private readonly ILogger<AddToCartController> _logger;

        public AddToCartController(EshopContext context, UserManager<Appuser> userManager, ILogger<AddToCartController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        [AllowAnonymous]
        // 🔹 Add product to cart (POST: api/addtocart)
        [HttpPost]
         // Requires authentication
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartItemDto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == cartItemDto.ProductId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {cartItemDto.ProductId} not found.");
                    return NotFound(new { message = "Product not found" });
                }

                var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null)
                {
                    cart = new Cart { UserId = userId };
                    _context.Carts.Add(cart);
                    await _context.SaveChangesAsync();
                }

                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemDto.ProductId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cartItemDto.Quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem { CartId = cart.Id, ProductId = cartItemDto.ProductId, Quantity = cartItemDto.Quantity });
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Product added to cart successfully" });
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database error while adding to cart: {dbEx.InnerException?.Message ?? dbEx.Message}");
                return StatusCode(500, "Database error. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding to cart: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [AllowAnonymous]
        // 🔹 Delete a cart item (DELETE: api/addtocart/{productId})
        [HttpDelete("{productId}")]
         // Requires authentication
        public async Task<IActionResult> DeleteCartItem(int productId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null || !cart.CartItems.Any())
                {
                    return NotFound(new { message = "Cart is empty or does not exist" });
                }

                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem == null)
                {
                    return NotFound(new { message = "Product not found in cart" });
                }

                cart.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Product removed from cart successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting cart item: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
