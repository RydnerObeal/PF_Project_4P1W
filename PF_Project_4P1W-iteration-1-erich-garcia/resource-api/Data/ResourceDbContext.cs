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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pack>(entity =>
            {
                entity.ToTable("Packs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }
}
