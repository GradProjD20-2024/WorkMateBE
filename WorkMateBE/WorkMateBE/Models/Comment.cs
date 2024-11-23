namespace WorkMateBE.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Status { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
