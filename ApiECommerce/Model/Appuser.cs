using Api_ECommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace Api_ECommerce.Model
{
    public class Appuser :IdentityUser
    {
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
