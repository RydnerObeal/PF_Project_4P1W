using Microsoft.AspNetCore.Mvc;
using resource_api.Services;

namespace resource_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacksController : ControllerBase
    {
        private readonly PackService _packService;

        public PacksController(PackService packService)
        {
            _packService = packService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPacks([FromQuery] bool random = false)
        {
            var packs = random 
                ? await _packService.GetRandomizedPacksAsync()
                : await _packService.GetPublishedPacksAsync();

            var packDtos = packs.Select(p => new PackDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            }).ToList();

            return Ok(packDtos);
        }
    }

    public class PackDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
