using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;

namespace resource_api.Controllers
{
    [ApiController]
    [Route("api/cms/packs")]
    [Authorize(Roles = "admin")] // Admin auth required
    public class CmsPacksController : ControllerBase
    {
        private readonly ResourceDbContext _context;

        public CmsPacksController(ResourceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPacks()
        {
            var packs = await _context.Packs
                .Include(p => p.PackPuzzles)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var result = packs.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                description = p.Description,
                isPublished = p.IsPublished,
                status = p.IsPublished ? "Published" : "Draft",
                puzzleCount = p.PackPuzzles.Count
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePack([FromBody] CreatePackRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required");

            var pack = new Pack
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim() ?? string.Empty,
                IsPublished = request.Visibility ?? false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Packs.Add(pack);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPacks), new { id = pack.Id }, new { id = pack.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePack(Guid id, [FromBody] UpdatePackRequest request)
        {
            var pack = await _context.Packs.FindAsync(id);
            if (pack == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required");

            pack.Name = request.Name.Trim();
            pack.Description = request.Description?.Trim() ?? string.Empty;
            pack.IsPublished = request.Visibility ?? pack.IsPublished;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePack(Guid id)
        {
            var pack = await _context.Packs.FindAsync(id);
            if (pack == null)
                return NotFound();

            _context.Packs.Remove(pack);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/puzzles")]
        public async Task<IActionResult> AddPuzzleToPack(Guid id, [FromBody] AddPuzzleRequest request)
        {
            var pack = await _context.Packs.FindAsync(id);
            if (pack == null)
                return NotFound();

            var puzzle = await _context.Puzzles.FindAsync(request.PuzzleId);
            if (puzzle == null)
                return BadRequest("Puzzle not found");

            // Check if already added
            var existing = await _context.PackPuzzles
                .AnyAsync(pp => pp.PackId == id && pp.PuzzleId == request.PuzzleId);

            if (existing)
                return BadRequest("Puzzle already in pack");

            // Check uniqueness of answer in pack
            var packPuzzles = await _context.PackPuzzles
                .Where(pp => pp.PackId == id)
                .Include(pp => pp.Puzzle)
                .ToListAsync();

            if (packPuzzles.Any(pp => pp.Puzzle.Answer == puzzle.Answer))
                return BadRequest("A puzzle with this answer already exists in the pack");

            var packPuzzle = new PackPuzzle
            {
                PackId = id,
                PuzzleId = request.PuzzleId,
                AddedAt = DateTime.UtcNow
            };

            _context.PackPuzzles.Add(packPuzzle);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/puzzles/{puzzleId}")]
        public async Task<IActionResult> RemovePuzzleFromPack(Guid id, Guid puzzleId)
        {
            var packPuzzle = await _context.PackPuzzles
                .FirstOrDefaultAsync(pp => pp.PackId == id && pp.PuzzleId == puzzleId);

            if (packPuzzle == null)
                return NotFound();

            _context.PackPuzzles.Remove(packPuzzle);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{id}/publish")]
        public async Task<IActionResult> TogglePublish(Guid id)
        {
            var pack = await _context.Packs.FindAsync(id);
            if (pack == null)
                return NotFound();

            pack.IsPublished = !pack.IsPublished;
            await _context.SaveChangesAsync();

            return Ok(new { isPublished = pack.IsPublished });
        }
    }

    public class CreatePackRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool? Visibility { get; set; }
    }

    public class UpdatePackRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool? Visibility { get; set; }
    }

    public class AddPuzzleRequest
    {
        public Guid PuzzleId { get; set; }
    }
}