using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 1,
                CouponCode = "FirstOff",
                DiscountAmount = 10,
                MinAmount = 500,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 2,
                CouponCode = "100Off",
                DiscountAmount = 100,
                MinAmount = 2000,
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                CouponId = 3,
                CouponCode = "5PercentOff",
                DiscountAmount = 0.05,
                MinAmount = 1000,
            });
        }
    }
}
