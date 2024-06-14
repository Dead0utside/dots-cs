using System.Collections.Generic;
using Dots.Core;
using DotsCore.Entity;

namespace DotsWeb.Models
{
    public class GameModel
    {
        public Field Field { get; set; }
        
        public IList<Score> Scores { get; set; }

        public GameModel(Field field, IList<Score> scores)
        {
            Field = field;
            Scores = scores;
        }
    }
}