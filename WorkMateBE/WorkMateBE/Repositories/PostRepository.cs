using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;

namespace WorkMateBE.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreatPost(Post post)
        {
            _context.Add(post);
            return Saved();

        }

        public bool DeletePost(int postId)
        {
            var post = GetPostById(postId);
            _context.Remove(post);
            return Saved();
        }

        public ICollection<Post> GetPostByAccountId(int accountId)
        {
            var posts = _context.Posts.Where(p => p.AccountId == accountId).ToList();
            return posts;
        }

        public Post GetPostById(int postId)
        {
            var post = _context.Posts.Find(postId);
            return post;
        }

        public ICollection<Post> GetPosts()
        {
            var posts = _context.Posts.ToList();
            return posts;
        }

        public bool Saved()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
