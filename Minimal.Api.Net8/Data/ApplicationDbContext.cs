using Microsoft.EntityFrameworkCore;
using Minimal.Api.Net8.Helpers.Constant;
using Minimal.Api.Net8.Models;

namespace Minimal.Api.Net8.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("LocalUser");
            modelBuilder.Entity<Coupon>().ToTable("Coupon").HasData(
                new Coupon { Id = 1, Name = "10+2OFF", Percent = 10.2, IsActive = BooleanByte.Yes.First(), CreatedBy = "JURAE008", CreatedAt = new DateTime(2023, 11, 10), UpdatedBy = "JURAE008", UpdatedAt = DateTime.Now },
                new Coupon { Id = 2, Name = "20OFF", Percent = 20, IsActive = BooleanByte.No.First(), CreatedBy = "JURAE008", CreatedAt = new DateTime(2023, 11, 10), UpdatedBy = "JURAE008", UpdatedAt = DateTime.Now }
                );
        }

    }
}
