using Microsoft.AspNetCore.Mvc;
using resource_api.Services;

namespace resource_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;
        private readonly PuzzleService _puzzleService;

        public GameController(GameService gameService, PuzzleService puzzleService)
        {
            _gameService = gameService;
            _puzzleService = puzzleService;
        }

        /// <summary>
        /// Submit a guess for a puzzle
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitGuess(
            [FromBody] GuessRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Guess))
            {
                return BadRequest("Guess is required");
            }

            if (request.PuzzleId == Guid.Empty || request.UserId == Guid.Empty || request.PackId == Guid.Empty)
            {
                return BadRequest("PuzzleId, PackId, and UserId are required");
            }

            var (correct, score) = await _gameService.SubmitGuessAsync(
                request.PuzzleId,
                request.UserId,
                request.PackId,
                request.Guess);

            var response = new
            {
                correct = correct,
                score = score
            };

            return Ok(response);
        }

        /// <summary>
        /// Get user's profile progress (global score + pack-specific scores)
        /// </summary>
        [HttpGet("profile/{userId}")]
        public async Task<IActionResult> GetProfileProgress(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("UserId is required");
            }

            var totalScore = await _gameService.GetUserTotalScoreAsync(userId);
            var puzzlesSolved = await _gameService.GetUserPuzzlesSolvedAsync(userId);

            var response = new
            {
                totalScore = totalScore,
                puzzlesSolved = puzzlesSolved
            };

            return Ok(response);
        }

        /// <summary>
        /// Get user's score for a specific pack
        /// </summary>
        [HttpGet("profile/{userId}/pack/{packId}")]
        public async Task<IActionResult> GetPackProgress(Guid userId, Guid packId)
        {
            if (userId == Guid.Empty || packId == Guid.Empty)
            {
                return BadRequest("UserId and PackId are required");
            }

            var packScore = await _gameService.GetUserPackScoreAsync(userId, packId);
            var packPuzzlesSolved = await _gameService.GetUserPackPuzzlesSolvedAsync(userId, packId);

            var response = new
            {
                packScore = packScore,
                packPuzzlesSolved = packPuzzlesSolved
            };

            return Ok(response);
        }

        /// <summary>
        /// Get the top scorer
        /// </summary>
        [HttpGet("top-scorer")]
        public async Task<IActionResult> GetTopScorer()
        {
            var topScorer = await _gameService.GetTopScorerAsync();
            if (topScorer == null)
            {
                return NotFound("No scores found");
            }

            var response = new
            {
                userId = topScorer.Value.userId,
                totalScore = topScorer.Value.totalScore
            };

            return Ok(response);
        }
    }

    public class GuessRequest
    {
        public Guid PuzzleId { get; set; }
        public Guid UserId { get; set; }
        public Guid PackId { get; set; }
        public string Guess { get; set; } = string.Empty;
    }
}
