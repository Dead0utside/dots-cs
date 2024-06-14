using System.Collections.Generic;
using DotsCore.Entity;

namespace DotsCore.Service
{
    public interface IScoreService
    {
        void AddScore(Score score);

        IList<Score> GetTopScores();

        void ResetScore();
    }
}