namespace WorkMateBE.Dtos.PostDto
{
    public class PostUpdateDto
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public int Status { get; set; }
    }
}
