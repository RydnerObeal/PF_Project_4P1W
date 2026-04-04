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
                .Include(p => p.PackPuzzles)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<Pack>> GetRandomizedPacksAsync()
        {
            var packs = await _context.Packs
                .Where(p => p.IsPublished)
                .Include(p => p.PackPuzzles)
                .ToListAsync();
            
            // Randomize in memory after loading from database
            return packs.OrderBy(p => Random.Shared.Next()).ToList();
        }

        public async Task<Pack?> GetPackByIdAsync(Guid id)
        {
            return await _context.Packs
                .Include(p => p.PackPuzzles)
                    .ThenInclude(pp => pp.Puzzle)
                        .ThenInclude(pz => pz.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePackAsync(Pack pack)
        {
            _context.Packs.Add(pack);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePackAsync(Pack pack)
        {
            _context.Packs.Update(pack);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePackAsync(Guid id)
        {
            var pack = await _context.Packs.FindAsync(id);
            if (pack != null)
            {
                _context.Packs.Remove(pack);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPuzzleToPackAsync(Guid packId, Guid puzzleId)
        {
            var packPuzzle = new PackPuzzle
            {
                PackId = packId,
                PuzzleId = puzzleId,
                AddedAt = DateTime.UtcNow
            };
            _context.PackPuzzles.Add(packPuzzle);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePuzzleFromPackAsync(Guid packId, Guid puzzleId)
        {
            var packPuzzle = await _context.PackPuzzles
                .FirstOrDefaultAsync(pp => pp.PackId == packId && pp.PuzzleId == puzzleId);
            if (packPuzzle != null)
            {
                _context.PackPuzzles.Remove(packPuzzle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task TogglePublishAsync(Guid id)
        {
            var pack = await _context.Packs.FindAsync(id);
            if (pack != null)
            {
                pack.IsPublished = !pack.IsPublished;
                await _context.SaveChangesAsync();
            }
        }
    }
}
