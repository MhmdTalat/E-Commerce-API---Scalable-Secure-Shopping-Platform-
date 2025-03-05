using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Api_ECommerce.Models;
using Api_ECommerce.Model;

namespace Api_ECommerce.Data
{
    public class EshopContext : IdentityDbContext<Appuser>
    {
        public EshopContext(DbContextOptions<EshopContext> options) : base(options)
        {
        }

        public DbSet<Appuser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity models are configured correctly

            // Define primary keys for Identity tables
            //modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(i => new { i.LoginProvider, i.ProviderKey });
            //modelBuilder.Entity<IdentityUserRole<string>>().HasKey(i => new { i.UserId, i.RoleId });
            //modelBuilder.Entity<IdentityUserToken<string>>().HasKey(i => new { i.UserId, i.LoginProvider, i.Name });

            // Define relationships for CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define relationship between Cart and User
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
