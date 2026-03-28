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
                .Where(p => p.PackId == packId)
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
                    _context.Puzzles.Where(p => p.PackId == packId),
                    gs => gs.PuzzleId,
                    p => p.Id,
                    (gs, p) => new { gs.PuzzleId, gs.SolvedAt }
                )
                .OrderByDescending(x => x.SolvedAt)
                .Select(x => x.PuzzleId)
                .Take(RecentlySolvedThreshold)
                .ToListAsync();

            var recentlySolvedIds = await _context.GameScores
                .Where(gs => gs.UserId == userId && gs.IsSolved)
                .Join(
                    _context.Puzzles.Where(p => p.PackId == packId),
                    gs => gs.PuzzleId,
                    p => p.Id,
                    (gs, p) => new { gs.PuzzleId, gs.SolvedAt }
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
    }
}
