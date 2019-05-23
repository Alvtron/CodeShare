using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class QuestionVideo : Video
    {
        public virtual Question Question { get; set; }
        public Guid? QuestionUid { get; set; }

        public QuestionVideo() { }

        public QuestionVideo(string youTubeID) : base(youTubeID)
        {
        }
    }
}
