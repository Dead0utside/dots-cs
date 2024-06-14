using System.Collections.Generic;
using DotsCore.Entity;

namespace DotsCore.Service
{
    public interface IRatingService
    {
        void AddRating(Rating rating);

        IList<Rating> GetRatings();

        void ResetRating();
    }
}