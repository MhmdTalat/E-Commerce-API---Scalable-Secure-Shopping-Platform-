using Api_ECommerce.Data;
using Api_ECommerce.DTO;
using Api_ECommerce.Model;
using Api_ECommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api_ECommerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EshopContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(EshopContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 🔹 Get All Categories (Public: No Authentication Required)
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // 🔹 Get Category by ID (Public)
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(category);
        }

        // 🔹 Create Category (Protected: Requires Authentication)
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = new Category
            {
                Name = categoryDto.Name,
                Department = categoryDto.Department
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // [Authorize]  

        //[Authorize]
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDTO categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _context.Categories.FirstOrDefault(e => e.Id == id);
                if (category == null)
                {
                    return NotFound(); // 🔹 Return 404 if category doesn't exist
                }

                category.Name = categoryDto.Name;
                category.Department = categoryDto.Department;
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content (Success)
            }
            return BadRequest(ModelState); // 🔹 If model is invalid, return 400
        }




        // 🔹 Delete Category (Protected: Requires Authentication)


        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(); // 🔹 Return 404 if category doesn't exist
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Category deleted successfully" });
        }

    }
}
