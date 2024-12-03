namespace WorkMateBE.Dtos.PostDto
{
    public class PostGetDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public string Avatar {  get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
