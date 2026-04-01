using Microsoft.EntityFrameworkCore;
using resource_api.Models;

namespace resource_api.Data
{
    public class ResourceDbContext : DbContext
    {
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options) { }

        public DbSet<Pack> Packs { get; set; }
        public DbSet<Puzzle> Puzzles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<GameScore> GameScores { get; set; }

        // Iteration 4 additions
        public DbSet<LibraryImage> LibraryImages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ImageTag> ImageTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pack>(entity =>
            {
                entity.ToTable("Packs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasMany(e => e.Puzzles)
                    .WithOne(p => p.Pack)
                    .HasForeignKey(p => p.PackId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Puzzle>(entity =>
            {
                entity.ToTable("Puzzles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Answer).IsRequired().HasMaxLength(200);
                entity.HasMany(e => e.Images)
                    .WithOne(i => i.Puzzle)
                    .HasForeignKey(i => i.PuzzleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Images");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired();
            });

            modelBuilder.Entity<GameScore>(entity =>
            {
                entity.ToTable("GameScores");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.PuzzleId }).IsUnique();
            });

            // Iteration 4
            modelBuilder.Entity<LibraryImage>(entity =>
            {
                entity.ToTable("LibraryImages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tags");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<ImageTag>(entity =>
            {
                entity.ToTable("ImageTags");
                entity.HasKey(e => new { e.LibraryImageId, e.TagId });
                entity.HasOne<LibraryImage>()
                    .WithMany(i => i.ImageTags)
                    .HasForeignKey(it => it.LibraryImageId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<Tag>()
                    .WithMany(t => t.ImageTags)
                    .HasForeignKey(it => it.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}