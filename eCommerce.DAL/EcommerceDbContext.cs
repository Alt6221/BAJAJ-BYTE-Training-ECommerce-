using eCommerce.Models;
using Microsoft.EntityFrameworkCore;
namespace eCommerce.DAL
{
    public class EcommerceDbContext: DbContext
    {
        public EcommerceDbContext()
        {
            
        }
        public EcommerceDbContext(DbContextOptions<DbContext> options): base(options)
        {
            
        }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Product>Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CartDetail> CartDetails{ get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Invoice> Invoices{ get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured) 
            {
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ByteJanECommerceDb24;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }
    }
}
