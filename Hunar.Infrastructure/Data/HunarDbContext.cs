using Hunar.Domain.Entities;
using Hunar.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Hunar.Infrastructure.Data
{
    public class HunarDbContext : DbContext
    {
        // DbContextOptions carries the connection string and provider info
        // We pass it from Program.cs — DbContext itself doesn't hardcode any connection
        public HunarDbContext(DbContextOptions<HunarDbContext> options) : base(options)
        {
        }

        // Each DbSet = one table in the database
        // EF Core will create a table named after the property (e.g. "Users", "ProviderProfiles")
        public DbSet<User> Users { get; set; }
        public DbSet<ProviderProfile> ProviderProfiles { get; set; }
        public DbSet<ServiceListing> ServiceListings { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // OnModelCreating is where we tell EF Core about relationships,
        // constraints, and rules that can't be expressed through properties alone
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─── USER ───────────────────────────────────────────
            modelBuilder.Entity<User>(entity =>
            {
                // Email must be unique — no two accounts with same email
                entity.HasIndex(u => u.Email).IsUnique();

                // Store the enum as a string in DB ("Customer", "Provider")
                // instead of a number (0, 1, 2) — more readable in DB
                entity.Property(u => u.Role)
                      .HasConversion<string>();
            });

            // ─── PROVIDER PROFILE ────────────────────────────────
            modelBuilder.Entity<ProviderProfile>(entity =>
            {
                // One User has at most one ProviderProfile
                // If User is deleted, delete their ProviderProfile too
                entity.HasOne(p => p.User)
                      .WithOne(u => u.ProviderProfile)
                      .HasForeignKey<ProviderProfile>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.Availability)
                      .HasConversion<string>();

                // Precision for rating: 3 digits total, 2 after decimal (e.g. 4.75)
                entity.Property(p => p.AverageRating)
                      .HasPrecision(3, 2);
            });

            // ─── SERVICE LISTING ─────────────────────────────────
            modelBuilder.Entity<ServiceListing>(entity =>
            {
                // One ProviderProfile has many ServiceListings
                // If ProviderProfile deleted, delete their listings too
                entity.HasOne(s => s.ProviderProfile)
                      .WithMany(p => p.ServiceListings)
                      .HasForeignKey(s => s.ProviderProfileId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Price precision: up to 8 digits, 2 after decimal (e.g. 999999.99)
                entity.Property(s => s.Price)
                      .HasPrecision(8, 2);
            });

            // ─── SERVICE REQUEST ──────────────────────────────────
            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.Property(s => s.Status)
                      .HasConversion<string>();

                // Customer (User) → ServiceRequests
                // Restrict delete: don't delete requests if customer deleted
                // (we want to keep history)
                entity.HasOne(s => s.Customer)
                      .WithMany(u => u.ServiceRequests)
                      .HasForeignKey(s => s.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Provider → ServiceRequests
                entity.HasOne(s => s.ProviderProfile)
                      .WithMany(p => p.ServiceRequests)
                      .HasForeignKey(s => s.ProviderProfileId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── REVIEW ───────────────────────────────────────────
            modelBuilder.Entity<Review>(entity =>
            {
                // One ServiceRequest has at most one Review
                entity.HasOne(r => r.ServiceRequest)
                      .WithOne(s => s.Review)
                      .HasForeignKey<Review>(r => r.ServiceRequestId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Rating must be between 1 and 5 — enforced at DB level
                entity.Property(r => r.Rating)
                      .HasDefaultValue(1);
            });
        }
    }
}

//if the database can enforce it automatically and it's about data shape or relationships, 
//    put it in DbContext.
//    If it requires thinking about business logic or system state, 
//    put it in Application.