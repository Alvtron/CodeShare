using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class ContentRating : Rating
    {
        public Content Content { get; set; }
        public Guid? ContentUid { get; set; }
        

        public ContentRating()
        {
        }

        public ContentRating(User user, bool value)
            : base(user, value)
        {
        }
    }
}
