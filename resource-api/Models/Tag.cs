namespace resource_api.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public ICollection<ImageTag> ImageTags { get; set; } = new List<ImageTag>();
    }
}