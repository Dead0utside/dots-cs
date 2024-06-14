using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DotsCore.Entity;

namespace DotsCore.Service
{
    public class ScoreServiceFile : IScoreService
    {
        private const string FileName = "score.bin";
        
        private List<Score> _scores = new List<Score>();

        public void AddScore(Score score)
        {
            _scores.Add(score);
            SaveScores();
        }

        public IList<Score> GetTopScores()
        {
            LoadScores();
            //return _scores.OrderByDescending(s => s.Points).Take(3).ToList();
            return (from s in _scores orderby s.Points descending select s).Take(3).ToList();
        }

        public void ResetScore()
        {
            _scores.Clear();
            File.Delete(FileName);
        }
        
        private void SaveScores()
        {
            using (var fs = File.OpenWrite(FileName))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, _scores);
            }
        }

        private void LoadScores()
        {
            if (File.Exists(FileName))
            {
                using (var fs = File.OpenRead(FileName))
                {
                    var bf = new BinaryFormatter();
                    _scores = (List<Score>)bf.Deserialize(fs);
                }
            }
        }
    }
}