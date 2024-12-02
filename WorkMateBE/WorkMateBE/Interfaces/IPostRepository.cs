using WorkMateBE.Models;

namespace WorkMateBE.Interfaces
{
    public interface IPostRepository
    {
        bool CreatePost(int accountId, string content, string imageUrl);
        bool UpdatePost (int postId, Post postUpdate);
        bool DeletePost (int postId);
        Post GetPostById (int postId);
        ICollection<Post> GetPostByAccountId (int accountId);
        ICollection<Post> GetPosts();
        bool Saved();
    }
}
