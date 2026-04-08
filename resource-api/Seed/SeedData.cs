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

<<<<<<< HEAD
            // Reset seed data by clearing old data in dev
=======
            // Ensure database is created with the current model for local development.
            await context.Database.EnsureCreatedAsync();

            // Reset seed data by clearing old pack/puzzle/image/score data in dev
>>>>>>> origin/iteration-5-rydner-obeal
            if (await context.Puzzles.AnyAsync())
            {
                context.GameScores.RemoveRange(context.GameScores);
                context.Images.RemoveRange(context.Images);
                context.Puzzles.RemoveRange(context.Puzzles);
                context.Packs.RemoveRange(context.Packs);
                await context.SaveChangesAsync();
            }

<<<<<<< HEAD
            // Create packs (6 categories)
            var packs = new List<Pack>
            {
                new Pack { Id = Guid.NewGuid(), Name = "Animals", Description = "Guess the animal from four pictures", IsPublished = true, BaseScore = 10, CreatedAt = DateTime.UtcNow },
                new Pack { Id = Guid.NewGuid(), Name = "Food & Drinks", Description = "Identify various foods and beverages", IsPublished = true, BaseScore = 12, CreatedAt = DateTime.UtcNow },
                new Pack { Id = Guid.NewGuid(), Name = "Sports", Description = "Name the sport from the images shown", IsPublished = true, BaseScore = 15, CreatedAt = DateTime.UtcNow },
                new Pack { Id = Guid.NewGuid(), Name = "Emotions", Description = "Guess the emotion or feeling from the pictures", IsPublished = true, BaseScore = 12, CreatedAt = DateTime.UtcNow },
                new Pack { Id = Guid.NewGuid(), Name = "Places", Description = "Identify famous places and locations", IsPublished = true, BaseScore = 14, CreatedAt = DateTime.UtcNow },
                new Pack { Id = Guid.NewGuid(), Name = "Objects", Description = "Name common objects from the images", IsPublished = true, BaseScore = 10, CreatedAt = DateTime.UtcNow }
            };

            await context.Packs.AddRangeAsync(packs);
            await context.SaveChangesAsync();
=======
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
>>>>>>> origin/iteration-5-rydner-obeal

            var animalsPackId = packs[0].Id;
            var foodPackId = packs[1].Id;
            var sportsPackId = packs[2].Id;
<<<<<<< HEAD
            var emotionsPackId = packs[3].Id;
            var placesPackId = packs[4].Id;
            var objectsPackId = packs[5].Id;

            var puzzles = new List<Puzzle>();

            // ================= ANIMALS (5 puzzles) =================
            var catPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = catPuzzleId, Answer = "cat", CreatedAt = DateTime.UtcNow });

            var dogPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = dogPuzzleId, Answer = "dog", CreatedAt = DateTime.UtcNow });

            var lion1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = lion1PuzzleId, Answer = "lion", CreatedAt = DateTime.UtcNow });

            var tiger1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = tiger1PuzzleId, Answer = "tiger", CreatedAt = DateTime.UtcNow });

            var elephant1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = elephant1PuzzleId, Answer = "elephant", CreatedAt = DateTime.UtcNow });

            // ================= FOOD (5 puzzles) =================
            var pizzaPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = pizzaPuzzleId, Answer = "pizza", CreatedAt = DateTime.UtcNow });

            var applePuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = applePuzzleId, Answer = "apple", CreatedAt = DateTime.UtcNow });

            var coffee1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = coffee1PuzzleId, Answer = "coffee", CreatedAt = DateTime.UtcNow });

            var cake1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = cake1PuzzleId, Answer = "cake", CreatedAt = DateTime.UtcNow });

            var noodles1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = noodles1PuzzleId, Answer = "noodles", CreatedAt = DateTime.UtcNow });

            // ================= SPORTS (5 puzzles) =================
            var basketballPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = basketballPuzzleId, Answer = "basketball", CreatedAt = DateTime.UtcNow });

            var soccerPuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = soccerPuzzleId, Answer = "soccer", CreatedAt = DateTime.UtcNow });

            var tennis1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = tennis1PuzzleId, Answer = "tennis", CreatedAt = DateTime.UtcNow });

            var swimming1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = swimming1PuzzleId, Answer = "swimming", CreatedAt = DateTime.UtcNow });

            var boxing1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = boxing1PuzzleId, Answer = "boxing", CreatedAt = DateTime.UtcNow });

            // ================= EMOTIONS (5 puzzles) =================
            var happy1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = happy1PuzzleId, Answer = "happy", CreatedAt = DateTime.UtcNow });

            var sad1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = sad1PuzzleId, Answer = "sad", CreatedAt = DateTime.UtcNow });

            var angry1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = angry1PuzzleId, Answer = "angry", CreatedAt = DateTime.UtcNow });

            var surprise1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = surprise1PuzzleId, Answer = "surprise", CreatedAt = DateTime.UtcNow });

            var scared1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = scared1PuzzleId, Answer = "scared", CreatedAt = DateTime.UtcNow });

            // ================= PLACES (5 puzzles) =================
            var beach1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = beach1PuzzleId, Answer = "beach", CreatedAt = DateTime.UtcNow });

            var mountain1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = mountain1PuzzleId, Answer = "mountain", CreatedAt = DateTime.UtcNow });

            var city1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = city1PuzzleId, Answer = "city", CreatedAt = DateTime.UtcNow });

            var forest1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = forest1PuzzleId, Answer = "forest", CreatedAt = DateTime.UtcNow });

            var desert1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = desert1PuzzleId, Answer = "desert", CreatedAt = DateTime.UtcNow });

            // ================= OBJECTS (5 puzzles) =================
            var car1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = car1PuzzleId, Answer = "car", CreatedAt = DateTime.UtcNow });

            var book1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = book1PuzzleId, Answer = "book", CreatedAt = DateTime.UtcNow });

            var cup1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = cup1PuzzleId, Answer = "cup", CreatedAt = DateTime.UtcNow });

            var pen1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = pen1PuzzleId, Answer = "pen", CreatedAt = DateTime.UtcNow });

            var bottle1PuzzleId = Guid.NewGuid();
            puzzles.Add(new Puzzle { Id = bottle1PuzzleId, Answer = "bottle", CreatedAt = DateTime.UtcNow });

            await context.Puzzles.AddRangeAsync(puzzles);
            await context.SaveChangesAsync();

            // Create PackPuzzles
            var packPuzzles = new List<PackPuzzle>
            {
                // Animals pack (5 puzzles)
                new PackPuzzle { PackId = animalsPackId, PuzzleId = catPuzzleId },
                new PackPuzzle { PackId = animalsPackId, PuzzleId = dogPuzzleId },
                new PackPuzzle { PackId = animalsPackId, PuzzleId = lion1PuzzleId },
                new PackPuzzle { PackId = animalsPackId, PuzzleId = tiger1PuzzleId },
                new PackPuzzle { PackId = animalsPackId, PuzzleId = elephant1PuzzleId },

                // Food pack (5 puzzles)
                new PackPuzzle { PackId = foodPackId, PuzzleId = pizzaPuzzleId },
                new PackPuzzle { PackId = foodPackId, PuzzleId = applePuzzleId },
                new PackPuzzle { PackId = foodPackId, PuzzleId = coffee1PuzzleId },
                new PackPuzzle { PackId = foodPackId, PuzzleId = cake1PuzzleId },
                new PackPuzzle { PackId = foodPackId, PuzzleId = noodles1PuzzleId },

                // Sports pack (5 puzzles)
                new PackPuzzle { PackId = sportsPackId, PuzzleId = basketballPuzzleId },
                new PackPuzzle { PackId = sportsPackId, PuzzleId = soccerPuzzleId },
                new PackPuzzle { PackId = sportsPackId, PuzzleId = tennis1PuzzleId },
                new PackPuzzle { PackId = sportsPackId, PuzzleId = swimming1PuzzleId },
                new PackPuzzle { PackId = sportsPackId, PuzzleId = boxing1PuzzleId },

                // Emotions pack (5 puzzles)
                new PackPuzzle { PackId = emotionsPackId, PuzzleId = happy1PuzzleId },
                new PackPuzzle { PackId = emotionsPackId, PuzzleId = sad1PuzzleId },
                new PackPuzzle { PackId = emotionsPackId, PuzzleId = angry1PuzzleId },
                new PackPuzzle { PackId = emotionsPackId, PuzzleId = surprise1PuzzleId },
                new PackPuzzle { PackId = emotionsPackId, PuzzleId = scared1PuzzleId },

                // Places pack (5 puzzles)
                new PackPuzzle { PackId = placesPackId, PuzzleId = beach1PuzzleId },
                new PackPuzzle { PackId = placesPackId, PuzzleId = mountain1PuzzleId },
                new PackPuzzle { PackId = placesPackId, PuzzleId = city1PuzzleId },
                new PackPuzzle { PackId = placesPackId, PuzzleId = forest1PuzzleId },
                new PackPuzzle { PackId = placesPackId, PuzzleId = desert1PuzzleId },

                // Objects pack (5 puzzles)
                new PackPuzzle { PackId = objectsPackId, PuzzleId = car1PuzzleId },
                new PackPuzzle { PackId = objectsPackId, PuzzleId = book1PuzzleId },
                new PackPuzzle { PackId = objectsPackId, PuzzleId = cup1PuzzleId },
                new PackPuzzle { PackId = objectsPackId, PuzzleId = pen1PuzzleId },
                new PackPuzzle { PackId = objectsPackId, PuzzleId = bottle1PuzzleId }
=======

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
>>>>>>> origin/iteration-5-rydner-obeal
            };

            await context.PackPuzzles.AddRangeAsync(packPuzzles);
            await context.SaveChangesAsync();

<<<<<<< HEAD
            // Create images - using only available image files
            var allImages = new List<Image>
            {
                // Cat images
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = null, Url = "/images/cat-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = null, Url = "/images/cat-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = null, Url = "/images/cat-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = catPuzzleId, LibraryImageId = null, Url = "/images/cat-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Dog images
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = null, Url = "/images/dog-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = null, Url = "/images/dog-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = null, Url = "/images/dog-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = dogPuzzleId, LibraryImageId = null, Url = "/images/dog-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Lion images
                new Image { Id = Guid.NewGuid(), PuzzleId = lion1PuzzleId, LibraryImageId = null, Url = "/images/lion-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = lion1PuzzleId, LibraryImageId = null, Url = "/images/lion-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = lion1PuzzleId, LibraryImageId = null, Url = "/images/lion-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = lion1PuzzleId, LibraryImageId = null, Url = "/images/lion-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Tiger images
                new Image { Id = Guid.NewGuid(), PuzzleId = tiger1PuzzleId, LibraryImageId = null, Url = "/images/tiger-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = tiger1PuzzleId, LibraryImageId = null, Url = "/images/tiger-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = tiger1PuzzleId, LibraryImageId = null, Url = "/images/tiger-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = tiger1PuzzleId, LibraryImageId = null, Url = "/images/tiger-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Elephant images
                new Image { Id = Guid.NewGuid(), PuzzleId = elephant1PuzzleId, LibraryImageId = null, Url = "/images/elephant-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = elephant1PuzzleId, LibraryImageId = null, Url = "/images/elephant-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = elephant1PuzzleId, LibraryImageId = null, Url = "/images/elephant-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = elephant1PuzzleId, LibraryImageId = null, Url = "/images/elephant-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Pizza images
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = null, Url = "/images/pizza-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = null, Url = "/images/pizza-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = null, Url = "/images/pizza-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pizzaPuzzleId, LibraryImageId = null, Url = "/images/pizza-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Apple images
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = null, Url = "/images/apple-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = null, Url = "/images/apple-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = null, Url = "/images/apple-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = applePuzzleId, LibraryImageId = null, Url = "/images/apple-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Coffee images
                new Image { Id = Guid.NewGuid(), PuzzleId = coffee1PuzzleId, LibraryImageId = null, Url = "/images/coffee-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = coffee1PuzzleId, LibraryImageId = null, Url = "/images/coffee-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = coffee1PuzzleId, LibraryImageId = null, Url = "/images/coffee-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = coffee1PuzzleId, LibraryImageId = null, Url = "/images/coffee-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Cake images
                new Image { Id = Guid.NewGuid(), PuzzleId = cake1PuzzleId, LibraryImageId = null, Url = "/images/cake-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = cake1PuzzleId, LibraryImageId = null, Url = "/images/cake-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = cake1PuzzleId, LibraryImageId = null, Url = "/images/cake-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = cake1PuzzleId, LibraryImageId = null, Url = "/images/cake-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Noodles images
                new Image { Id = Guid.NewGuid(), PuzzleId = noodles1PuzzleId, LibraryImageId = null, Url = "/images/noodles-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = noodles1PuzzleId, LibraryImageId = null, Url = "/images/noodles-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = noodles1PuzzleId, LibraryImageId = null, Url = "/images/noodles-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = noodles1PuzzleId, LibraryImageId = null, Url = "/images/noodles-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Basketball images
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = null, Url = "/images/basketball-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = null, Url = "/images/basketball-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = null, Url = "/images/basketball-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = basketballPuzzleId, LibraryImageId = null, Url = "/images/basketball-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Soccer images
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = null, Url = "/images/soccer-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = null, Url = "/images/soccer-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = null, Url = "/images/soccer-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = soccerPuzzleId, LibraryImageId = null, Url = "/images/soccer-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Tennis images
                new Image { Id = Guid.NewGuid(), PuzzleId = tennis1PuzzleId, LibraryImageId = null, Url = "/images/tennis-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = tennis1PuzzleId, LibraryImageId = null, Url = "/images/tennis-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = tennis1PuzzleId, LibraryImageId = null, Url = "/images/tennis-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = tennis1PuzzleId, LibraryImageId = null, Url = "/images/tennis-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Swimming images
                new Image { Id = Guid.NewGuid(), PuzzleId = swimming1PuzzleId, LibraryImageId = null, Url = "/images/swimming-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = swimming1PuzzleId, LibraryImageId = null, Url = "/images/swimming-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = swimming1PuzzleId, LibraryImageId = null, Url = "/images/swimming-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = swimming1PuzzleId, LibraryImageId = null, Url = "/images/swimming-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Boxing images
                new Image { Id = Guid.NewGuid(), PuzzleId = boxing1PuzzleId, LibraryImageId = null, Url = "/images/boxing-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = boxing1PuzzleId, LibraryImageId = null, Url = "/images/boxing-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = boxing1PuzzleId, LibraryImageId = null, Url = "/images/boxing-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = boxing1PuzzleId, LibraryImageId = null, Url = "/images/boxing-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Happy images 
                new Image { Id = Guid.NewGuid(), PuzzleId = happy1PuzzleId, LibraryImageId = null, Url = "/images/happy-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = happy1PuzzleId, LibraryImageId = null, Url = "/images/happy-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = happy1PuzzleId, LibraryImageId = null, Url = "/images/happy-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = happy1PuzzleId, LibraryImageId = null, Url = "/images/happy-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Sad images 
                new Image { Id = Guid.NewGuid(), PuzzleId = sad1PuzzleId, LibraryImageId = null, Url = "/images/sad-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = sad1PuzzleId, LibraryImageId = null, Url = "/images/sad-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = sad1PuzzleId, LibraryImageId = null, Url = "/images/sad-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = sad1PuzzleId, LibraryImageId = null, Url = "/images/sad-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Angry images 
                new Image { Id = Guid.NewGuid(), PuzzleId = angry1PuzzleId, LibraryImageId = null, Url = "/images/angry-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = angry1PuzzleId, LibraryImageId = null, Url = "/images/angry-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = angry1PuzzleId, LibraryImageId = null, Url = "/images/angry-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = angry1PuzzleId, LibraryImageId = null, Url = "/images/angry-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Surprise images
                new Image { Id = Guid.NewGuid(), PuzzleId = surprise1PuzzleId, LibraryImageId = null, Url = "/images/surprise-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = surprise1PuzzleId, LibraryImageId = null, Url = "/images/surprise-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = surprise1PuzzleId, LibraryImageId = null, Url = "/images/surprise-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = surprise1PuzzleId, LibraryImageId = null, Url = "/images/surprise-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // scared images
                new Image { Id = Guid.NewGuid(), PuzzleId = scared1PuzzleId, LibraryImageId = null, Url = "/images/scared-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = scared1PuzzleId, LibraryImageId = null, Url = "/images/scared-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = scared1PuzzleId, LibraryImageId = null, Url = "/images/scared-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = scared1PuzzleId, LibraryImageId = null, Url = "/images/scared-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Beach images 
                new Image { Id = Guid.NewGuid(), PuzzleId = beach1PuzzleId, LibraryImageId = null, Url = "/images/beach-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = beach1PuzzleId, LibraryImageId = null, Url = "/images/beach-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = beach1PuzzleId, LibraryImageId = null, Url = "/images/beach-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = beach1PuzzleId, LibraryImageId = null, Url = "/images/beach-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Mountain images 
                new Image { Id = Guid.NewGuid(), PuzzleId = mountain1PuzzleId, LibraryImageId = null, Url = "/images/mountain-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = mountain1PuzzleId, LibraryImageId = null, Url = "/images/mountain-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = mountain1PuzzleId, LibraryImageId = null, Url = "/images/mountain-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = mountain1PuzzleId, LibraryImageId = null, Url = "/images/mountain-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // City images 
                new Image { Id = Guid.NewGuid(), PuzzleId = city1PuzzleId, LibraryImageId = null, Url = "/images/city-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = city1PuzzleId, LibraryImageId = null, Url = "/images/city-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = city1PuzzleId, LibraryImageId = null, Url = "/images/city-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = city1PuzzleId, LibraryImageId = null, Url = "/images/city-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Forest images
                new Image { Id = Guid.NewGuid(), PuzzleId = forest1PuzzleId, LibraryImageId = null, Url = "/images/forest-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = forest1PuzzleId, LibraryImageId = null, Url = "/images/forest-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = forest1PuzzleId, LibraryImageId = null, Url = "/images/forest-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = forest1PuzzleId, LibraryImageId = null, Url = "/images/forest-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Desert images
                new Image { Id = Guid.NewGuid(), PuzzleId = desert1PuzzleId, LibraryImageId = null, Url = "/images/desert-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = desert1PuzzleId, LibraryImageId = null, Url = "/images/desert-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = desert1PuzzleId, LibraryImageId = null, Url = "/images/desert-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = desert1PuzzleId, LibraryImageId = null, Url = "/images/desert-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Car images 
                new Image { Id = Guid.NewGuid(), PuzzleId = car1PuzzleId, LibraryImageId = null, Url = "/images/car-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = car1PuzzleId, LibraryImageId = null, Url = "/images/car-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = car1PuzzleId, LibraryImageId = null, Url = "/images/car-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = car1PuzzleId, LibraryImageId = null, Url = "/images/car-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Book images 
                new Image { Id = Guid.NewGuid(), PuzzleId = book1PuzzleId, LibraryImageId = null, Url = "/images/book-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = book1PuzzleId, LibraryImageId = null, Url = "/images/book-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = book1PuzzleId, LibraryImageId = null, Url = "/images/book-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = book1PuzzleId, LibraryImageId = null, Url = "/images/book-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Cup images 
                new Image { Id = Guid.NewGuid(), PuzzleId = cup1PuzzleId, LibraryImageId = null, Url = "/images/cup-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = cup1PuzzleId, LibraryImageId = null, Url = "/images/cup-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = cup1PuzzleId, LibraryImageId = null, Url = "/images/cup-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = cup1PuzzleId, LibraryImageId = null, Url = "/images/cup-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Pen images
                new Image { Id = Guid.NewGuid(), PuzzleId = pen1PuzzleId, LibraryImageId = null, Url = "/images/pen-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pen1PuzzleId, LibraryImageId = null, Url = "/images/pen-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pen1PuzzleId, LibraryImageId = null, Url = "/images/pen-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = pen1PuzzleId, LibraryImageId = null, Url = "/images/pen-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow },

                // Bottle images
                new Image { Id = Guid.NewGuid(), PuzzleId = bottle1PuzzleId, LibraryImageId = null, Url = "/images/bottle-1.jpg", Position = 0, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = bottle1PuzzleId, LibraryImageId = null, Url = "/images/bottle-2.jpg", Position = 1, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = bottle1PuzzleId, LibraryImageId = null, Url = "/images/bottle-3.jpg", Position = 2, CreatedAt = DateTime.UtcNow },
                new Image { Id = Guid.NewGuid(), PuzzleId = bottle1PuzzleId, LibraryImageId = null, Url = "/images/bottle-4.jpg", Position = 3, CreatedAt = DateTime.UtcNow }
            };
=======
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
>>>>>>> origin/iteration-5-rydner-obeal

            await context.Images.AddRangeAsync(allImages);
            await context.SaveChangesAsync();
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/iteration-5-rydner-obeal
