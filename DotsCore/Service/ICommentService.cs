using System.Collections.Generic;
using DotsCore.Entity;

namespace DotsCore.Service
{
    public interface ICommentService
    {
        void AddComment(Comment comment);

        IList<Comment> GetComments();

        void ResetComment();
    }
}