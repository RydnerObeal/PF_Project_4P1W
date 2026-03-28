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

            // Ensure database is created with all migrations
            await context.Database.MigrateAsync();

            // Reset seed data by clearing old pack/puzzle/image/score data in dev
            if (await context.Puzzles.AnyAsync())
            {
                context.GameScores.RemoveRange(context.GameScores);
                context.Images.RemoveRange(context.Images);
                context.Puzzles.RemoveRange(context.Puzzles);
                context.Packs.RemoveRange(context.Packs);
                await context.SaveChangesAsync();
            }

            // Get or create packs
            var existingPacks = await context.Packs.ToListAsync();
            List<Pack> packs;

            if (existingPacks.Count == 0)
            {
                packs = new List<Pack>
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
                    }
                };

                await context.Packs.AddRangeAsync(packs);
                await context.SaveChangesAsync();
            }
            else
            {
                packs = existingPacks;
            }

            var animalsPackId = packs[0].Id;
            var foodPackId = packs[1].Id;
            var sportsPackId = packs[2].Id;

            var puzzles = new List<Puzzle>();

            // ================= ANIMALS =================
            var catPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = catPuzzleId,
                PackId = animalsPackId,
                Answer = "cat",
                CreatedAt = DateTime.UtcNow
            });

            var catImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, Url = "/images/cat-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, Url = "/images/cat-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, Url = "/images/cat-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, Url = "/images/cat-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            var dogPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = dogPuzzleId,
                PackId = animalsPackId,
                Answer = "dog",
                CreatedAt = DateTime.UtcNow
            });

            var dogImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, Url = "/images/dog-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, Url = "/images/dog-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, Url = "/images/dog-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, Url = "/images/dog-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            // ================= FOOD =================
            var pizzaPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = pizzaPuzzleId,
                PackId = foodPackId,
                Answer = "pizza",
                CreatedAt = DateTime.UtcNow
            });

            var pizzaImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, Url = "/images/pizza-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, Url = "/images/pizza-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, Url = "/images/pizza-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, Url = "/images/pizza-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            var applePuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = applePuzzleId,
                PackId = foodPackId,
                Answer = "apple",
                CreatedAt = DateTime.UtcNow
            });

            var appleImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, Url = "/images/apple-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, Url = "/images/apple-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, Url = "/images/apple-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, Url = "/images/apple-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            // ================= SPORTS =================
            var basketballPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = basketballPuzzleId,
                PackId = sportsPackId,
                Answer = "basketball",
                CreatedAt = DateTime.UtcNow
            });

            var basketballImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, Url = "/images/basketball-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, Url = "/images/basketball-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, Url = "/images/basketball-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, Url = "/images/basketball-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            var soccerPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = soccerPuzzleId,
                PackId = sportsPackId,
                Answer = "soccer",
                CreatedAt = DateTime.UtcNow
            });

            var soccerImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, Url = "/images/soccer-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, Url = "/images/soccer-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, Url = "/images/soccer-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, Url = "/images/soccer-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            // Save puzzles first
            await context.Puzzles.AddRangeAsync(puzzles);
            await context.SaveChangesAsync();

            // Save images
            await context.Images.AddRangeAsync(catImages);
            await context.Images.AddRangeAsync(dogImages);
            await context.Images.AddRangeAsync(pizzaImages);
            await context.Images.AddRangeAsync(appleImages);
            await context.Images.AddRangeAsync(basketballImages);
            await context.Images.AddRangeAsync(soccerImages);

            await context.SaveChangesAsync();
        }
    }
}