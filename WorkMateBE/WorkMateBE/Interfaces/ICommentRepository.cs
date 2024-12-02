using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface ICommentRepository
    {
        bool CreateComment(int userId, string content, int postId);
        bool DeleteComment(int commentId);
        Comment GetCommentById(int commentId);
        List<Comment> GetCommentByPostId(int postId);
    }
}
