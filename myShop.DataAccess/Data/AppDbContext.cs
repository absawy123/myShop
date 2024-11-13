using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myShop.Entities.Models;

namespace myShop.DataAccess.Data
{
    public class AppDbContext :IdentityDbContext<IdentityUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }


		public AppDbContext(DbContextOptions options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Category>().HasData(
            //    new List<Category>
            //    {
            //    new Category{Name="Laptops",Description="This is laptop description"},                new Category{Name="Laptops",Description="This is laptop description"},
            //    new Category{Name="Phones",Description="This is Phones description"},
            //    new Category{Name="Fashion",Description="This is Fashion description"}
            //    });

            builder.Entity<ApplicationUser>().Property(a => a.Name).IsRequired(true);
            builder.Entity<ApplicationUser>().Property(a => a.Address).IsRequired(true);
            builder.Entity<ApplicationUser>().Property(a => a.City).IsRequired(false);
            base.OnModelCreating(builder);
        }



    }
}
