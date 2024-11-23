namespace WorkMateBE.Dtos.PostDto
{
    public class PostCreateDto
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public int Status { get; set; }
        public int AccountId { get; set; }
    }
}
