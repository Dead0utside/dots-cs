using System;
using System.Collections.Generic;
using System.Linq;
using Dots.Service;
using DotsCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotsCore.Service
{
    public class RatingServiceEF : IRatingService
    {
        public void AddRating(Rating rating)
        {
            if (rating.Rate > 100 || rating.Rate < 0)
            {
                throw new Exception();
            }
            using (var context = new DotsDbContext())
            {
                context.Ratings.Add(rating);
                context.SaveChanges();
            }
        }

        public IList<Rating> GetRatings()
        {
            using (var context = new DotsDbContext())
            {
                return (from s in context.Ratings select s).ToList();
            }
        }

        public void ResetRating()
        {
            using (var context = new DotsDbContext())
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Ratings");
            }
        }
    }
}