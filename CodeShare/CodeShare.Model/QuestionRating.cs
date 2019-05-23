using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class QuestionRating : Rating
    {
        public virtual Question Question { get; set; }
        public Guid? QuestionUid { get; set; }
        
        public QuestionRating()
        {
        }

        public QuestionRating(User user, bool value)
            : base(user, value)
        {
        }
    }
}
