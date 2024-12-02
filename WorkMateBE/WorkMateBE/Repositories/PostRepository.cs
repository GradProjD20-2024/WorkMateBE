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
        public bool CreatePost(int accountId, string content, string imageUrl)
        {
            var account = _context.Accounts.Find(accountId);
            var employee = _context.Employees.Find(account.EmployeeId);
            var post = new Post
            {
                FullName = employee.FullName,
                AccountId = accountId,
                Content = content,
                ImageUrl = imageUrl
            };
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

        public bool UpdatePost(int postId, Post postUpdate)
        {
            var post = GetPostById(postId);
            post.Content = postUpdate.Content;
            post.ImageUrl = postUpdate.ImageUrl;
            post.Status = postUpdate.Status;
            _context.Update(post);
            return Saved();

        }
    }
}
