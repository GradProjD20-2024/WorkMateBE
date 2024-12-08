namespace WorkMateBE.Dtos.CommentDto
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl {  get; set; }
        public string Content { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
       
    }
}
