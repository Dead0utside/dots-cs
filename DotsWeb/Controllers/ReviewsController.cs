using System.Collections.Generic;
using DotsCore.Entity;
using DotsCore.Service;
using DotsWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotsWeb.Controllers
{
    public class ReviewsController : Controller
    {
        private IRatingService _ratingService = new RatingServiceEF();
        private ICommentService _commentService = new CommentServiceEF();
        
        // GET
        public IActionResult Index()
        {
            return View("Index", CreateModel());
        }

        private ReviewModel CreateModel()
        {
            return new ReviewModel(_ratingService.GetRatings(), _commentService.GetComments());
        }
    }
}