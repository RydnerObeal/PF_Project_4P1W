using System;
using System.Collections.Generic;

namespace resource_api.Models
{
    public class Puzzle
    {
        public Guid Id { get; set; }
        public string Answer { get; set; } = string.Empty;
        public string? Hint { get; set; }
        public string? Difficulty { get; set; } // e.g., "Easy", "Medium", "Hard"
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<PackPuzzle> PackPuzzles { get; set; } = new List<PackPuzzle>();
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
