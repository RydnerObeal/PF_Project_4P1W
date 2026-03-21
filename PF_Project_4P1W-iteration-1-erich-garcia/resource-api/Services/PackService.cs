using Microsoft.EntityFrameworkCore;
using resource_api.Data;
using resource_api.Models;

namespace resource_api.Services
{
    public class PackService
    {
        private readonly ResourceDbContext _context;

        public PackService(ResourceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pack>> GetPublishedPacksAsync()
        {
            return await _context.Packs
                .Where(p => p.IsPublished)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<Pack>> GetRandomizedPacksAsync()
        {
            return await _context.Packs
                .Where(p => p.IsPublished)
                .OrderBy(p => Guid.NewGuid())
                .ToListAsync();
        }
    }
}
