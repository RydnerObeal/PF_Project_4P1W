using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;

namespace resource_api.Seed
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new ResourceDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ResourceDbContext>>());

            // Check if database has any packs
            if (await context.Packs.AnyAsync())
            {
                return; // Database has been seeded
            }

            var packs = new List<Pack>
            {
                new Pack
                {
                    Id = Guid.NewGuid(),
                    Name = "Animals",
                    Description = "Guess the animal from four pictures",
                    IsPublished = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Pack
                {
                    Id = Guid.NewGuid(),
                    Name = "Food & Drinks",
                    Description = "Identify various foods and beverages",
                    IsPublished = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Pack
                {
                    Id = Guid.NewGuid(),
                    Name = "Sports",
                    Description = "Name the sport from the images shown",
                    IsPublished = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Pack
                {
                    Id = Guid.NewGuid(),
                    Name = "Technology",
                    Description = "Guess tech gadgets and innovations",
                    IsPublished = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Pack
                {
                    Id = Guid.NewGuid(),
                    Name = "Nature",
                    Description = "Explore natural wonders and landscapes",
                    IsPublished = false, // Draft pack
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Packs.AddRangeAsync(packs);
            await context.SaveChangesAsync();
        }
    }
}
