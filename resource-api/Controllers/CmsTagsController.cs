using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;

namespace resource_api.Controllers
{
    [ApiController]
    [Route("api/cms/tags")]
    [Authorize(Roles = "admin")]
    public class CmsTagsController : ControllerBase
    {
        private readonly ResourceDbContext _context;
        public CmsTagsController(ResourceDbContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetTags() =>
            Ok(await _context.Tags.Select(t => new { t.Id, t.Name }).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Tag name is required.");

            var name = dto.Name.Trim().ToLower();
            if (await _context.Tags.AnyAsync(t => t.Name == name))
                return BadRequest("Tag already exists.");

            var tag = new Tag { Name = name };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return Ok(new { tag.Id, tag.Name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CreateTagDto
    {
        public string Name { get; set; } = "";
    }
}