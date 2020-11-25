using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Ace_cars.Models
{
    public partial class AceAutoDealersContext : DbContext
    {
        public AceAutoDealersContext() 
        {
            
        }
        //public AceAutoDealersContext(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        public IConfiguration Configuration { get; }
        public AceAutoDealersContext(DbContextOptions<AceAutoDealersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<HeardAboutUs> HeardAboutUs { get; set; } 
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Ace-Auto"));
            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

        //    modelBuilder.Entity<Car>(entity =>
        //    {
        //        entity.Property(e => e.CarId).HasColumnName("carId");

        //        entity.Property(e => e.Name)
        //            .HasColumnName("name")
        //            .HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<Customer>(entity =>
        //    {
        //        entity.Property(e => e.Id)
        //            .HasColumnName("id")
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.Address)
        //            .HasColumnName("address")
        //            .HasMaxLength(50);

        //        entity.Property(e => e.CarId).HasColumnName("carId");

        //        entity.Property(e => e.HeardAboutUs).HasColumnName("heardAboutUs");

        //        entity.Property(e => e.Name)
        //            .HasColumnName("name")
        //            .HasMaxLength(50);

        //        entity.Property(e => e.VisitDate)
        //            .HasColumnName("visitDate")
        //            .HasColumnType("datetime");

        //        entity.HasOne(d => d.Car)
        //            .WithMany(p => p.Customer)
        //            .HasForeignKey(d => d.CarId)
        //            .HasConstraintName("FK_Customer_Car");

        //        entity.HasOne(d => d.HeardAboutUsNavigation)
        //            .WithMany(p => p.Customer)
        //            .HasForeignKey(d => d.HeardAboutUs)
        //            .HasConstraintName("FK_Customer_HeardAboutUs");
        //    });

        //    modelBuilder.Entity<HeardAboutUs>(entity =>
        //    {
        //        entity.Property(e => e.Id)
        //            .HasColumnName("id")
        //            .ValueGeneratedNever();

        //        entity.Property(e => e.Name)
        //            .HasColumnName("name")
        //            .HasMaxLength(50);
        //    });
        //}
    }
}
