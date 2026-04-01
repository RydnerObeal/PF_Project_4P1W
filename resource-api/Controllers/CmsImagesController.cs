using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;

namespace resource_api.Controllers
{
    [ApiController]
    [Route("api/cms/images")]
    [Authorize(Roles = "admin")]
    public class CmsImagesController : ControllerBase
    {
        private readonly ResourceDbContext _context;
        public CmsImagesController(ResourceDbContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetImages([FromQuery] string? tag = null)
        {
            var query = _context.LibraryImages
                .Include(i => i.ImageTags)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                var tagEntity = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tag);
                if (tagEntity != null)
                    query = query.Where(i => i.ImageTags.Any(it => it.TagId == tagEntity.Id));
            }

            var images = await query.ToListAsync();
            var allTagIds = images.SelectMany(i => i.ImageTags).Select(it => it.TagId).Distinct().ToList();
            var allTags = await _context.Tags.Where(t => allTagIds.Contains(t.Id)).ToListAsync();

            return Ok(images.Select(i => new
            {
                i.Id,
                i.Url,
                i.FileName,
                i.CreatedAt,
                tags = i.ImageTags.Select(it => allTags.FirstOrDefault(t => t.Id == it.TagId)?.Name).Where(n => n != null)
            }));
        }

        [HttpPost]
        public async Task<IActionResult> AddImage([FromBody] AddImageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Url))
                return BadRequest("URL is required.");

            var image = new LibraryImage { Url = dto.Url, FileName = dto.FileName };
            _context.LibraryImages.Add(image);
            await _context.SaveChangesAsync();
            return Ok(new { image.Id, image.Url, image.FileName, image.CreatedAt, tags = new List<string>() });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            var image = await _context.LibraryImages.FindAsync(id);
            if (image == null) return NotFound();
            _context.LibraryImages.Remove(image);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AddTag(Guid id, [FromBody] TagMapDto dto)
        {
            var image = await _context.LibraryImages.FindAsync(id);
            if (image == null) return NotFound("Image not found.");

            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == dto.TagName.Trim().ToLower());
            if (tag == null) return NotFound("Tag not found.");

            var exists = await _context.ImageTags.AnyAsync(it => it.LibraryImageId == id && it.TagId == tag.Id);
            if (!exists)
            {
                _context.ImageTags.Add(new ImageTag { LibraryImageId = id, TagId = tag.Id });
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpDelete("{id}/tags/{tagName}")]
        public async Task<IActionResult> RemoveTag(Guid id, string tagName)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
            if (tag == null) return NotFound();

            var imageTag = await _context.ImageTags
                .FirstOrDefaultAsync(it => it.LibraryImageId == id && it.TagId == tag.Id);
            if (imageTag == null) return NotFound();

            _context.ImageTags.Remove(imageTag);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class AddImageDto
    {
        public string Url { get; set; } = "";
        public string? FileName { get; set; }
    }

    public class TagMapDto
    {
        public string TagName { get; set; } = "";
    }
}