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

            // Ensure database is created with the current model for local development.
            await context.Database.EnsureCreatedAsync();

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
                        BaseScore = 10,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Pack
                    {
                        Id = Guid.NewGuid(),
                        Name = "Food & Drinks",
                        Description = "Identify various foods and beverages",
                        IsPublished = true,
                        BaseScore = 12,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Pack
                    {
                        Id = Guid.NewGuid(),
                        Name = "Sports",
                        Description = "Name the sport from the images shown",
                        IsPublished = true,
                        BaseScore = 15,
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
                Answer = "cat",
                CreatedAt = DateTime.UtcNow
            });

            var catImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/cat-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/cat-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/cat-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/cat-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            var dogPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = dogPuzzleId,
                Answer = "dog",
                CreatedAt = DateTime.UtcNow
            });

            var dogImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/dog-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/dog-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/dog-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/dog-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            // ================= FOOD =================
            var pizzaPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = pizzaPuzzleId,
                Answer = "pizza",
                CreatedAt = DateTime.UtcNow
            });

            var pizzaImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/pizza-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/pizza-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/pizza-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/pizza-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            var applePuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = applePuzzleId,
                Answer = "apple",
                CreatedAt = DateTime.UtcNow
            });

            var appleImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = Guid.Empty, Url = "/images/apple-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = Guid.Empty, Url = "/images/apple-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = Guid.Empty, Url = "/images/apple-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = Guid.Empty, Url = "/images/apple-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            // ================= SPORTS =================
            var basketballPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = basketballPuzzleId,
                Answer = "basketball",
                CreatedAt = DateTime.UtcNow
            });

            var basketballImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/basketball-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/basketball-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/basketball-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/basketball-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            var soccerPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle
            {
                Id = soccerPuzzleId,
                Answer = "soccer",
                CreatedAt = DateTime.UtcNow
            });

            var soccerImages = new List<Image>
            {
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/soccer-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/soccer-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/soccer-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = Guid.Empty, Url = "/images/soccer-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };

            // Save puzzles first
            await context.Puzzles.AddRangeAsync(puzzles);
            await context.SaveChangesAsync();

            // Create PackPuzzle associations
            var packPuzzles = new List<PackPuzzle>
            {
                new PackPuzzle { PackId = animalsPackId, PuzzleId = catPuzzleId },
                new PackPuzzle { PackId = animalsPackId, PuzzleId = dogPuzzleId },
                new PackPuzzle { PackId = foodPackId, PuzzleId = pizzaPuzzleId },
                new PackPuzzle { PackId = foodPackId, PuzzleId = applePuzzleId },
                new PackPuzzle { PackId = sportsPackId, PuzzleId = basketballPuzzleId },
                new PackPuzzle { PackId = sportsPackId, PuzzleId = soccerPuzzleId }
            };

            await context.PackPuzzles.AddRangeAsync(packPuzzles);
            await context.SaveChangesAsync();

            // Create or reuse library image records for the seeded puzzle images
            var allImages = catImages
                .Concat(dogImages)
                .Concat(pizzaImages)
                .Concat(appleImages)
                .Concat(basketballImages)
                .Concat(soccerImages)
                .ToList();

            var imageUrls = allImages.Select(i => i.Url).Distinct().ToList();
            var existingLibraryImages = await context.LibraryImages
                .Where(li => imageUrls.Contains(li.Url))
                .ToListAsync();

            var libraryImagesByUrl = existingLibraryImages.ToDictionary(li => li.Url);
            var newLibraryImages = imageUrls
                .Where(url => !libraryImagesByUrl.ContainsKey(url))
                .Select(url => new LibraryImage { Id = Guid.NewGuid(), Url = url })
                .ToList();

            await context.LibraryImages.AddRangeAsync(newLibraryImages);
            await context.SaveChangesAsync();

            foreach (var libImage in newLibraryImages)
            {
                libraryImagesByUrl[libImage.Url] = libImage;
            }

            foreach (var image in allImages)
            {
                image.LibraryImageId = libraryImagesByUrl[image.Url].Id;
            }

            await context.Images.AddRangeAsync(allImages);
            await context.SaveChangesAsync();
        }
    }
}