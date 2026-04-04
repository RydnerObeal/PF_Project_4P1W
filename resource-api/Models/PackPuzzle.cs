using System;

namespace resource_api.Models
{
    public class PackPuzzle
    {
        public Guid PackId { get; set; }
        public Guid PuzzleId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Pack Pack { get; set; } = null!;
        public Puzzle Puzzle { get; set; } = null!;
    }
}