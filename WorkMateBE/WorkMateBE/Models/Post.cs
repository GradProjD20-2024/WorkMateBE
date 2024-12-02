namespace WorkMateBE.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
