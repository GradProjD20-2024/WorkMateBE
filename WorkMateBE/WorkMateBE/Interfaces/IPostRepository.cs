using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IPostRepository
    {
        bool CreatePost (Post post);
        bool UpdatePost (int postId, Post postUpdate);
        bool DeletePost (int postId);
        Post GetPostById (int postId);
        ICollection<Post> GetPostByAccountId (int accountId);
        ICollection<Post> GetPosts();
        bool Saved();
    }
}
