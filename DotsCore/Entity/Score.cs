using System;

namespace DotsCore.Entity
{
    [Serializable]
    public class Score
    {
        public int Id { get; set; }
        
        public string PlayerName { get; set; }
        
        public int Points { get; set; }
    }
}