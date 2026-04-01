namespace resource_api.Models
{
    public class LibraryImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Url { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ImageTag> ImageTags { get; set; } = new List<ImageTag>();
    }
}