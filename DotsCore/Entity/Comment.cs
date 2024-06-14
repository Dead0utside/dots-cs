using System;

namespace DotsCore.Entity
{
    [Serializable]
    public class Comment
    {
        public int Id { get; set; }
        
        public string PlayerName { get; set; }
        
        public string Text { get; set; }
    }
}