using BigBox_v4.Domain;
using BigBox_v4.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBox_v4.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<Drivers> Drivers { get; set; }
        public DbSet<DriverSchedule> DriverSchedules { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<BoxSize> BoxSizes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WebSession> WebSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Drivers>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.LicenseNumber).IsUnique();
            });

            modelBuilder.Entity<DriverSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Driver)
                      .WithMany()
                      .HasForeignKey(e => e.DriverId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WebSession>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SessionHash).IsRequired().HasMaxLength(64);
                entity.Property(e => e.ExpiresAt).IsRequired();
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
