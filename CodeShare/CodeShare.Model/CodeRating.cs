using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class CodeRating : Rating
    {
        public virtual Code Code { get; set; }
        public Guid? CodeUid { get; set; }
        
        public CodeRating()
        {
        }

        public CodeRating(User user, bool value)
            : base(user, value)
        {
        }
    }
}
