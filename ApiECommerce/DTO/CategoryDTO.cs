using Api_ECommerce.Models;
using Api_ECommerce.DTO;
using System.ComponentModel.DataAnnotations;



    namespace Api_ECommerce.DTO
    {
    public class CategoryDTO
    {
        
           

        [Required]
        public string Name { get; set; }

        [Required]
        public Department Department { get; set; }
    }
}
 


