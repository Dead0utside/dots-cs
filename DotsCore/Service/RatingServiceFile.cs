using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DotsCore.Entity;

namespace DotsCore.Service  
{
    public class RatingServiceFile : IRatingService
    {
        private const string FileName = "rating.bin";
        
        private List<Rating> _ratings = new List<Rating>();

        public void AddRating(Rating rating)
        {
            if (rating.Rate > 100 || rating.Rate < 0)
            {
                throw new Exception();
            }
            _ratings.Add(rating);
            SaveRatings();
        }

        public IList<Rating> GetRatings()
        {
            LoadRatings();
            return _ratings.ToList();
        }

        public void ResetRating()
        {
            _ratings.Clear();
            File.Delete(FileName);
        }
        
        private void SaveRatings()
        {
            using (var fs = File.OpenWrite(FileName))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, _ratings);
            }
        }

        private void LoadRatings()
        {
            if (File.Exists(FileName))
            {
                using (var fs = File.OpenRead(FileName))
                {
                    var bf = new BinaryFormatter();
                    _ratings = (List<Rating>)bf.Deserialize(fs);
                }
            }
        }
    }
}