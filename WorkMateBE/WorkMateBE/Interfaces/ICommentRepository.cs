using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface ICommentRepository
    {
        bool CreateComment(Comment comment);
        bool DeleteComment(int commentId);
        Comment GetCommentById(int commentId);
        List<Comment> GetCommentByPostId(int postId);
    }
}
