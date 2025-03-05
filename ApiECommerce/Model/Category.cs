using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Api_ECommerce.Models
{
    // Define an enum for departments
    public enum Department
    {
        Fruits = 0,
        Vegetables = 1,
        Drinks = 2,
        Flavor = 3,
        Meat = 4,
        Chickens = 5
    }

    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Department Department { get; set; }
        [JsonIgnore]  // Prevents serialization loop

       // Prevents serialization loop
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
