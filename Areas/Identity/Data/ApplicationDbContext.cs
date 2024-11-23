using Eproject.Areas.Identity.Data;
using Eproject.Migrations;
using Eproject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eproject.Areas.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Caterers> caterers { get; set; } 
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Foodtype> foodtypes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            

            // Caterers ka PricePerPerson
            builder.Entity<Caterers>()
                .Property(c => c.PricePerPerson)
                .HasColumnType("decimal(18,2)");

            // Booking ka TotalPrice
            builder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}
