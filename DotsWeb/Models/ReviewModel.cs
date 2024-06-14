using System.Collections.Generic;
using DotsCore.Entity;
using DotsCore.Service;

namespace DotsWeb.Models
{
    public class ReviewModel
    {
        private IRatingService _ratingService = new RatingServiceEF();
        private ICommentService _commentService = new CommentServiceEF();
        
        public IList<Rating> Ratings { get; set; }
        
        public IList<Comment> Comments { get; set; }

        public ReviewModel(IList<Rating> ratings, IList<Comment> comments)
        {
            Ratings = ratings;
            Comments = comments;
        }
        
        private ReviewModel CreateModel()
        {
            IList<Rating> ratings = _ratingService.GetRatings();
            IList<Comment> comments = _commentService.GetComments();
            
            return new ReviewModel(ratings, comments);
        }
    }
}