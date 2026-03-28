using Microsoft.EntityFrameworkCore;
using resource_api.Models;

namespace resource_api.Data
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options)
        {
        }

        public DbSet<Pack> Packs { get; set; }
        public DbSet<Puzzle> Puzzles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<GameScore> GameScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pack configuration
            modelBuilder.Entity<Pack>(entity =>
            {
                entity.ToTable("Packs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasMany(e => e.Puzzles)
                    .WithOne(p => p.Pack)
                    .HasForeignKey(p => p.PackId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Puzzle configuration
            modelBuilder.Entity<Puzzle>(entity =>
            {
                entity.ToTable("Puzzles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Answer).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasMany(e => e.Images)
                    .WithOne(i => i.Puzzle)
                    .HasForeignKey(i => i.PuzzleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Image configuration
            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Images");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // GameScore configuration
            modelBuilder.Entity<GameScore>(entity =>
            {
                entity.ToTable("GameScores");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SolvedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => new { e.UserId, e.PuzzleId }).IsUnique();
            });
        }
    }
}

