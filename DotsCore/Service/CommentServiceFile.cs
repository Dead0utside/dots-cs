using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using DotsCore.Entity;

namespace DotsCore.Service  
{
    public class CommentServiceFile : ICommentService
    {
        private const string FileName = "comment.bin";
        
        private List<Comment> _comments = new List<Comment>();

        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
            SaveComments();
        }

        public IList<Comment> GetComments()
        {
            LoadComments();
            return _comments.ToList();
        }

        public void ResetComment()
        {
            _comments.Clear();
            File.Delete(FileName);
        }
        
        private void SaveComments()
        {
            using (var fs = File.OpenWrite(FileName))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, _comments);
            }
        }

        private void LoadComments()
        {
            if (File.Exists(FileName))
            {
                using (var fs = File.OpenRead(FileName))
                {
                    var bf = new BinaryFormatter();
                    _comments = (List<Comment>)bf.Deserialize(fs);
                }
            }
        }
    }
}