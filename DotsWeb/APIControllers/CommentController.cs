using System.Collections.Generic;
using DotsCore.Entity;
using DotsCore.Service;
using Microsoft.AspNetCore.Mvc;
namespace DotsWeb.APIControllers
{
    //https://localhost:44324/api/Comment
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentService _comment = new CommentServiceEF();

        //GET: /api/Comment
        [HttpGet]
        public IEnumerable<Comment> GetComments()
        {
            return _comment.GetComments();
        }

        //POST: /api/Comment
        [HttpPost]
        public void PostComment(Comment comment)
        {
            _comment.AddComment(comment);
        }
    }
}