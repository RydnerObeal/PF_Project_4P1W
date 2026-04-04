using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;

namespace resource_api.Services
{
    public class PuzzleService
    {
        private readonly ResourceDbContext _context;
        private const int RecentlySolvedThreshold = 5; // Exclude last 5 solved puzzles

        public PuzzleService(ResourceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the next puzzle for a pack, excluding recently solved puzzles
        /// </summary>
        public async Task<Puzzle?> GetNextPuzzleAsync(Guid packId, Guid userId)
        {
            // Load all puzzles in the pack
            var allPuzzles = await _context.Puzzles
                .Where(p => p.PackPuzzles.Any(pp => pp.PackId == packId))
                .Include(p => p.Images)
                .ToListAsync();

            if (!allPuzzles.Any())
            {
                return null;
            }

            // Get recently attempted puzzle IDs for this user in this pack (last 5), including wrong guesses
            var recentlyAttemptedIds = await _context.GameScores
                .Where(gs => gs.UserId == userId)
                .Join(
                    _context.PackPuzzles.Where(pp => pp.PackId == packId),
                    gs => gs.PuzzleId,
                    pp => pp.PuzzleId,
                    (gs, pp) => new { gs.PuzzleId, gs.SolvedAt }
                )
                .OrderByDescending(x => x.SolvedAt)
                .Select(x => x.PuzzleId)
                .Take(RecentlySolvedThreshold)
                .ToListAsync();

            var recentlySolvedIds = await _context.GameScores
                .Where(gs => gs.UserId == userId && gs.IsSolved)
                .Join(
                    _context.PackPuzzles.Where(pp => pp.PackId == packId),
                    gs => gs.PuzzleId,
                    pp => pp.PuzzleId,
                    (gs, pp) => new { gs.PuzzleId, gs.SolvedAt }
                )
                .OrderByDescending(x => x.SolvedAt)
                .Select(x => x.PuzzleId)
                .Take(RecentlySolvedThreshold)
                .ToListAsync();

            // Prefer puzzles not in recently attempted set (last 5 attempts), then not in recently solved
            var candidatePuzzles = allPuzzles
                .Where(p => !recentlyAttemptedIds.Contains(p.Id))
                .ToList();

            if (!candidatePuzzles.Any())
            {
                candidatePuzzles = allPuzzles
                    .Where(p => !recentlySolvedIds.Contains(p.Id))
                    .ToList();
            }

            if (!candidatePuzzles.Any())
            {
                candidatePuzzles = allPuzzles;
            }

            // Randomize in-memory and pick one
            var puzzle = candidatePuzzles.OrderBy(p => Random.Shared.Next()).FirstOrDefault();

            return puzzle;
        }

        /// <summary>
        /// Get all images for a puzzle
        /// </summary>
        public async Task<List<Image>> GetPuzzleImagesAsync(Guid puzzleId)
        {
            return await _context.Images
                .Where(i => i.PuzzleId == puzzleId)
                .OrderBy(i => i.Position)
                .ToListAsync();
        }

        /// <summary>
        /// Get user's profile stats (total score, puzzles solved)
        /// </summary>
        public async Task<(int totalScore, int puzzlesSolved)> GetUserStatsAsync(Guid userId)
        {
            var stats = await _context.GameScores
                .Where(gs => gs.UserId == userId && gs.IsSolved)
                .GroupBy(gs => gs.UserId)
                .Select(g => new
                {
                    TotalScore = g.Sum(gs => gs.Score),
                    PuzzlesSolved = g.Count()
                })
                .FirstOrDefaultAsync();

            return stats != null 
                ? (stats.TotalScore, stats.PuzzlesSolved)
                : (0, 0);
        }

        public async Task<List<Puzzle>> GetAllPuzzlesAsync()
        {
            return await _context.Puzzles
                .Include(p => p.PackPuzzles)
                    .ThenInclude(pp => pp.Pack)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Puzzle?> GetPuzzleByIdAsync(Guid id)
        {
            return await _context.Puzzles
                .Include(p => p.PackPuzzles)
                    .ThenInclude(pp => pp.Pack)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePuzzleAsync(Puzzle puzzle, List<Image> images)
        {
            _context.Puzzles.Add(puzzle);
            _context.Images.AddRange(images);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePuzzleAsync(Puzzle puzzle, List<Image> newImages)
        {
            var existingImages = await _context.Images.Where(i => i.PuzzleId == puzzle.Id).ToListAsync();
            _context.Images.RemoveRange(existingImages);
            _context.Images.AddRange(newImages);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePuzzleAsync(Guid id)
        {
            var puzzle = await _context.Puzzles.FindAsync(id);
            if (puzzle != null)
            {
                _context.Puzzles.Remove(puzzle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
