using Api_ECommerce.Data;
using Api_ECommerce.DTO;
using Api_ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api_ECommerce.Controllers
{
    [Authorize] // ✅ Ensure this is present
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EshopContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<ProductController> _logger; // Add logging

        public ProductController(EshopContext context, IWebHostEnvironment hostEnvironment, ILogger<ProductController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        // 🔹 Get All Products (GET: api/product)
        [HttpGet]
        [AllowAnonymous] // Make this endpoint public
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching products: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // 🔹 Get Product by ID (GET: api/product/{id})
        [HttpGet("{id}")]
        [AllowAnonymous] // Make this public
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching product by ID: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // 🔹 Create Product (POST: api/product)
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDTO.ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = "Category not found" });
            }

            string imagePath = productDto.ImageFile != null ? await SaveImage(productDto.ImageFile) : null;

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                ImageUrl = imagePath,
                CategoryId = productDto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        //// 🔹 Add Product to Cart (POST: api/product/add-to-cart)
        //[HttpPost("add-to-cart")]
        //[AllowAnonymous] // 🚀 Add this for debugging
        //                 // [Authorize] // Ensures the user is authenticated
        //public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartItemDto)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(new { message = "User not authenticated" });
        //    }

        //    var cartItem = new CartItem
        //    {
        //        ProductId = cartItemDto.ProductId,
        //        Quantity = cartItemDto.Quantity,
        //        UserId = userId // 🔹 Assign authenticated user's ID
        //    };

        //    _context.CartItems.Add(cartItem);
        //    await _context.SaveChangesAsync();

        //    return Ok(new { message = "Product added to cart successfully" });
        //}



        // 🔹 Update Product (PUT: api/product/{id})
        [HttpPut("{id}")]
        [AllowAnonymous] // 🚀 Add this for debugging
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDTO.ProductDto productDto)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            if (productDto.ImageFile != null)
            {
                existingProduct.ImageUrl = await SaveImage(productDto.ImageFile);
            }

            existingProduct.CategoryId = productDto.CategoryId;

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return Ok(existingProduct);
        }

        // 🔹 Delete Product (DELETE: api/product/{id})
        [HttpDelete("{id}")]
        [AllowAnonymous] // 🚀 Add this for debugging
        public async Task<IActionResult> DeleteProduct(int id)

        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product deleted successfully" });
        }

        // 🔹 Helper Method to Save Image
        private async Task<string> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null)
                return null;

            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/uploads/{fileName}";
        }
    }
}
