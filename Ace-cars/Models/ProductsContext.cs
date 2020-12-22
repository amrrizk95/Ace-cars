using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Ace_cars.Models
{
    public partial class ProductsContext : DbContext
    {
        public ProductsContext() 
        {
            
        }
        //public AceAutoDealersContext(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        public IConfiguration Configuration { get; }
        public ProductsContext(DbContextOptions<ProductsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Products"));
            }
        }


    }
}
