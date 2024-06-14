using System.Collections.Generic;
using System.Linq;
using Dots.Service;
using DotsCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotsCore.Service
{
    public class CommentServiceEF : ICommentService
    {
        public void AddComment(Comment comment)
        {
            using (var context = new DotsDbContext())
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }
        }
    
        public IList<Comment> GetComments()
        {
            using (var context = new DotsDbContext())
            {
                return (from s in context.Comments select s).ToList();
            }
        }

        public void ResetComment()
        {
            using (var context = new DotsDbContext())
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Comments");
            }
        }
    }
}