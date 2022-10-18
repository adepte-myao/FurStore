using FurStore.Models.Auth;
using FurStore.Models.Store;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FurStore.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FurnitureOrderElement>()
                .HasKey(foe => new { foe.OrderId, foe.FurnitureId });

            base.OnModelCreating(builder);
        }

        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FurnitureOrderElement> FurnitureOrderElements { get; set; }
    }
}
