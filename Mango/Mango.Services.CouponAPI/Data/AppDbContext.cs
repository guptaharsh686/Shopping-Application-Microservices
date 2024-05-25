using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Coupon> Coupons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //use for identity services identity
            base.OnModelCreating(modelBuilder);

            //seed to database
            modelBuilder.Entity<Coupon>()
                .HasData(new Coupon
                {
                    CouponId = 1,
                    CouponCode = "10OFF",
                    DiscountAmount = 10,
                    MinAmount = 20,
                });

            modelBuilder.Entity<Coupon>()
                .HasData(new Coupon
                {
                    CouponId = 2,
                    CouponCode = "20OFF",
                    DiscountAmount = 20,
                    MinAmount = 40,
                });
        }
    }
}
