using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;
using resource_api.Services;

namespace resource_api.Controllers
{
    [ApiController]
    [Route("api/cms/puzzles")]
    [Authorize(Roles = "admin")] // Admin auth required
    public class CmsPuzzlesController : ControllerBase
    {
        private readonly ResourceDbContext _context;
        private readonly PuzzleService _puzzleService;

        public CmsPuzzlesController(ResourceDbContext context, PuzzleService puzzleService)
        {
            _context = context;
            _puzzleService = puzzleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPuzzles()
        {
            var puzzles = await _context.Puzzles
                .Include(p => p.PackPuzzles)
                    .ThenInclude(pp => pp.Pack)
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var result = puzzles.Select(p => new
            {
                id = p.Id,
                answerWord = p.Answer,
                hint = p.Hint,
                difficulty = p.Difficulty,
                images = p.Images.OrderBy(i => i.Position).Select(i => new
                {
                    id = i.Id,
                    url = i.Url
                }).ToList(),
                imageIds = p.Images.OrderBy(i => i.Position).Select(i => i.LibraryImageId).ToList(),
                imageUrls = p.Images.OrderBy(i => i.Position).Select(i => i.Url).ToList(),
                packMemberships = p.PackPuzzles.Select(pp => new
                {
                    packId = pp.PackId,
                    packName = pp.Pack.Name,
                    isPublished = pp.Pack.IsPublished
                }).ToList()
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePuzzle([FromBody] CreatePuzzleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AnswerWord))
                return BadRequest("Answer word is required");

            if (request.ImageIds == null || request.ImageIds.Count != 4)
                return BadRequest("Exactly 4 image IDs are required");

            // Validate images exist
            var existingImages = await _context.LibraryImages
                .Where(li => request.ImageIds.Contains(li.Id))
                .ToListAsync();

            if (existingImages.Count != 4)
                return BadRequest("One or more image IDs do not exist");

            // Normalize answer
            var normalizedAnswer = request.AnswerWord.Trim().ToLower();

            // Check uniqueness within packs (if packs specified, but for creation, maybe not yet)
            // For now, allow duplicates on creation, check on pack assignment

            var puzzle = new Puzzle
            {
                Id = Guid.NewGuid(),
                Answer = normalizedAnswer,
                Hint = request.Hint?.Trim(),
                Difficulty = request.Difficulty?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            // Create images
            var images = request.ImageIds.Select((imageId, index) =>
            {
                var libImage = existingImages.First(li => li.Id == imageId);
                return new Image
                {
                    Id = Guid.NewGuid(),
                    PuzzleId = puzzle.Id,
                    LibraryImageId = imageId,
                    Url = libImage.Url,
                    Position = index,
                    CreatedAt = DateTime.UtcNow
                };
            }).ToList();

            _context.Puzzles.Add(puzzle);
            _context.Images.AddRange(images);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPuzzles), new { id = puzzle.Id }, new { id = puzzle.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePuzzle(Guid id, [FromBody] UpdatePuzzleRequest request)
        {
            var puzzle = await _context.Puzzles
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (puzzle == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(request.AnswerWord))
                return BadRequest("Answer word is required");

            if (request.ImageIds == null || request.ImageIds.Count != 4)
                return BadRequest("Exactly 4 image IDs are required");

            // Validate images exist
            var existingImages = await _context.LibraryImages
                .Where(li => request.ImageIds.Contains(li.Id))
                .ToListAsync();

            if (existingImages.Count != 4)
                return BadRequest("One or more image IDs do not exist");

            // Normalize answer
            var normalizedAnswer = request.AnswerWord.Trim().ToLower();

            // Validate uniqueness within existing packs for this puzzle
            var packIds = await _context.PackPuzzles
                .Where(pp => pp.PuzzleId == puzzle.Id)
                .Select(pp => pp.PackId)
                .ToListAsync();

            if (packIds.Any())
            {
                var duplicateExists = await _context.PackPuzzles
                    .Include(pp => pp.Puzzle)
                    .Where(pp => packIds.Contains(pp.PackId) && pp.PuzzleId != puzzle.Id)
                    .AnyAsync(pp => pp.Puzzle.Answer == normalizedAnswer);

                if (duplicateExists)
                    return BadRequest("A puzzle with this answer already exists in one of the packs");
            }

            puzzle.Answer = normalizedAnswer;
            puzzle.Hint = request.Hint?.Trim();
            puzzle.Difficulty = request.Difficulty?.Trim();

            // Update images
            _context.Images.RemoveRange(puzzle.Images);

            var newImages = request.ImageIds.Select((imageId, index) =>
            {
                var libImage = existingImages.First(li => li.Id == imageId);
                return new Image
                {
                    Id = Guid.NewGuid(),
                    PuzzleId = puzzle.Id,
                    LibraryImageId = imageId,
                    Url = libImage.Url,
                    Position = index,
                    CreatedAt = DateTime.UtcNow
                };
            }).ToList();

            _context.Images.AddRange(newImages);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuzzle(Guid id)
        {
            var puzzle = await _context.Puzzles
                .Include(p => p.PackPuzzles)
                    .ThenInclude(pp => pp.Pack)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (puzzle == null)
                return NotFound();

            // Check if belongs to published packs
            var publishedPacks = puzzle.PackPuzzles
                .Where(pp => pp.Pack != null && pp.Pack.IsPublished)
                .Select(pp => pp.Pack.Name)
                .ToList();

            if (publishedPacks.Any())
            {
                return BadRequest(new
                {
                    message = "Cannot delete puzzle that belongs to published packs",
                    publishedPacks
                });
            }

            _context.Puzzles.Remove(puzzle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class CreatePuzzleRequest
    {
        public string AnswerWord { get; set; } = string.Empty;
        public string? Hint { get; set; }
        public string? Difficulty { get; set; }
        public List<Guid> ImageIds { get; set; } = new();
    }

    public class UpdatePuzzleRequest
    {
        public string AnswerWord { get; set; } = string.Empty;
        public string? Hint { get; set; }
        public string? Difficulty { get; set; }
        public List<Guid> ImageIds { get; set; } = new();
    }
}