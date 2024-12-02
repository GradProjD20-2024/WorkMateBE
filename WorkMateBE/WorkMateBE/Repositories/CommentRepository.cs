using WorkMateBE.Data;
using WorkMateBE.Interfaces;
using WorkMateBE.Models;

namespace WorkMateBE.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateComment(int userId, string content, int postId)
        {
            var account = _context.Accounts.Find(userId);
            var employee = _context.Employees.Find(account.EmployeeId);
            var comment = new Comment
            {
                FullName = employee.FullName,
                Content = content,
                Status = 0,
                PostId = postId,
                AccountId = userId
            };

           _context.Add(comment);
            return Save();
        }

        public bool DeleteComment(int commentId)
        {
            var comment = GetCommentById(commentId);
            _context.Remove(comment);
            return Save();
        }

        public Comment GetCommentById(int commentId)
        {
            var comment = _context.Comments.Find(commentId);
            return comment;
        }

        public List<Comment> GetCommentByPostId(int postId)
        {
            var comments = _context.Comments.Where(p => p.PostId == postId).ToList();
            return comments;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
