using System.Collections.Generic;
using DotsCore.Entity;
using DotsCore.Service;
using Microsoft.AspNetCore.Mvc;

namespace DotsWeb.APIControllers
{
    //https://localhost:44324/api/Rating
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private IRatingService _rating = new RatingServiceEF();

        //GET: /api/Rating
        [HttpGet]
        public IEnumerable<Rating> GetRatings()
        {
            return _rating.GetRatings();
        }

        //POST: /api/Rating
        [HttpPost]
        public void PostRating(Rating rating)
        {
            _rating.AddRating(rating);
        }
    }
}