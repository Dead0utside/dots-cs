using Dots.Service;
using DotsCore.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DotsCore.Service
{
    public class ScoreServiceEF : IScoreService
    {
        public void AddScore(Score score)
        {
            using (var context = new DotsDbContext())
            {
                context.Scores.Add(score);
                context.SaveChanges();
            }
        }

        public IList<Score> GetTopScores()
        {
            using (var context = new DotsDbContext())
            {
                return (from s in context.Scores orderby s.Points descending select s).Take(3).ToList();
            }
        }

        public void ResetScore()
        {
            using (var context = new DotsDbContext())
            {
                context.Database.ExecuteSqlRaw("DELETE FROM Scores");
            }
        }
    }
}
