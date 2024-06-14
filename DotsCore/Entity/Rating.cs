using System;

namespace DotsCore.Entity
{
    [Serializable]
    public class Rating
    {
        public int Id { get; set; }

        public string PlayerName { get; set; }
        
        public int Rate { get; set; }
    }
}