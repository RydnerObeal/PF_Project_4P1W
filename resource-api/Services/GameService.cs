using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;

namespace resource_api.Services
{
    public class GameService
    {
        private readonly ResourceDbContext _context;

        public GameService(ResourceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Submit a game guess and calculate score
        /// </summary>
        public async Task<(bool correct, int score)> SubmitGuessAsync(Guid puzzleId, Guid userId, Guid packId, string guess)
        {
            // Get the puzzle
            var puzzle = await _context.Puzzles.FindAsync(puzzleId);
            if (puzzle == null)
            {
                return (false, 0);
            }

            // Get the pack to determine scoring
            var pack = await _context.Packs.FindAsync(packId);
            if (pack == null)
            {
                return (false, 0);
            }

            // Check if answer is correct (case-insensitive)
            bool isCorrect = puzzle.Answer.Equals(guess, StringComparison.OrdinalIgnoreCase);
            int score = isCorrect ? pack.BaseScore : 0; // Use pack's base score

            // Check if already solved
            var existingScore = await _context.GameScores
                .FirstOrDefaultAsync(gs => gs.UserId == userId && gs.PuzzleId == puzzleId);

            if (existingScore != null)
            {
                // Update existing score
                if (isCorrect && !existingScore.IsSolved)
                {
                    existingScore.IsSolved = true;
                    existingScore.Score = score;
                    existingScore.SolvedAt = DateTime.UtcNow;
                }
            }
            else
            {
                // Create new score record
                var gameScore = new GameScore
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PuzzleId = puzzleId,
                    PackId = packId,
                    Score = score,
                    IsSolved = isCorrect,
                    SolvedAt = DateTime.UtcNow
                };

                _context.GameScores.Add(gameScore);
            }

            await _context.SaveChangesAsync();

            return (isCorrect, score);
        }

        /// <summary>
        /// Get user's total score for all solved puzzles
        /// </summary>
        public async Task<int> GetUserTotalScoreAsync(Guid userId)
        {
            return await _context.GameScores
                .Where(gs => gs.UserId == userId && gs.IsSolved)
                .SumAsync(gs => gs.Score);
        }

        /// <summary>
        /// Get number of puzzles solved by user
        /// </summary>
        public async Task<int> GetUserPuzzlesSolvedAsync(Guid userId)
        {
            return await _context.GameScores
                .Where(gs => gs.UserId == userId && gs.IsSolved)
                .CountAsync();
        }

        /// <summary>
        /// Get user's score for a specific pack
        /// </summary>
        public async Task<int> GetUserPackScoreAsync(Guid userId, Guid packId)
        {
            return await _context.GameScores
                .Where(gs => gs.UserId == userId && gs.PackId == packId && gs.IsSolved)
                .SumAsync(gs => gs.Score);
        }

        /// <summary>
        /// Get number of puzzles solved by user in a specific pack
        /// </summary>
        public async Task<int> GetUserPackPuzzlesSolvedAsync(Guid userId, Guid packId)
        {
            return await _context.GameScores
                .Where(gs => gs.UserId == userId && gs.PackId == packId && gs.IsSolved)
                .CountAsync();
        }

        /// <summary>
        /// Get the user with the highest total score
        /// </summary>
        public async Task<(Guid userId, int totalScore)?> GetTopScorerAsync()
        {
            var topScorer = await _context.GameScores
                .Where(gs => gs.IsSolved)
                .GroupBy(gs => gs.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalScore = g.Sum(gs => gs.Score)
                })
                .OrderByDescending(x => x.TotalScore)
                .FirstOrDefaultAsync();

            if (topScorer == null)
            {
                return null;
            }

            return (topScorer.UserId, topScorer.TotalScore);
        }
    }
}
