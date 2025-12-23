using EmlakPortal2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmlakPortal2.Data
{
    // IdentityDbContext'ten miras alıyoruz ki üyelik tabloları otomatik gelsin
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Veritabanında oluşacak tabloları tanımlıyoruz
        public DbSet<Category> Categories { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Price column configuration to avoid warning
            builder.Entity<Property>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }

     }
 }