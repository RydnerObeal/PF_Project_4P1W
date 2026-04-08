using System;

namespace resource_api.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public Guid PuzzleId { get; set; }
        public Guid? LibraryImageId { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Position { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Puzzle? Puzzle { get; set; }
        public LibraryImage? LibraryImage { get; set; }
    }
}
