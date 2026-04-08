using Microsoft.AspNetCore.Mvc;
using resource_api.Services;

namespace resource_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PuzzlesController : ControllerBase
    {
        private readonly PuzzleService _puzzleService;

        public PuzzlesController(PuzzleService puzzleService)
        {
            _puzzleService = puzzleService;
        }

        /// <summary>
        /// Get the next puzzle for a pack
        /// </summary>
        [HttpGet("next")]
        public async Task<IActionResult> GetNextPuzzle(
            [FromQuery] Guid packId,
            [FromQuery] Guid userId)
        {
            if (packId == Guid.Empty || userId == Guid.Empty)
            {
                return BadRequest("packId and userId are required");
            }

            var puzzle = await _puzzleService.GetNextPuzzleAsync(packId, userId);
            
            if (puzzle == null)
            {
                return NotFound("No puzzles available for this pack");
            }

            var response = new
            {
                puzzleId = puzzle.Id,
                images = puzzle.Images
                    .OrderBy(i => i.Position)
                    .Select(i => new
                    {
                        id = i.Id,
                        url = i.Url,
                        position = i.Position
                    })
                    .ToList()
            };

            return Ok(response);
        }
    }
}
