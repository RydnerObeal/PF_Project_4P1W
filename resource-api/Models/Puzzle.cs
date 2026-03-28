using System;
using System.Collections.Generic;

namespace resource_api.Models
{
    public class Puzzle
    {
        public Guid Id { get; set; }
        public Guid PackId { get; set; }
        public string Answer { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public Pack? Pack { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
