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
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public ICollection<Puzzle> Puzzles { get; set; } = new List<Puzzle>();
    }
}
