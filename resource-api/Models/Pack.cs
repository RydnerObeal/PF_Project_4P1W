using System;
using System.Collections.Generic;

namespace resource_api.Models
{
    public class Pack
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public int BaseScore { get; set; } = 10; // Base points per puzzle (e.g., Animals=10, Sports=15, Food=12)
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public ICollection<PackPuzzle> PackPuzzles { get; set; } = new List<PackPuzzle>();
    }
}
