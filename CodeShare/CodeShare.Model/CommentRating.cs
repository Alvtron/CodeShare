using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class CommentRating : Rating
    {
        public Comment Comment { get; set; }
        public Guid CommentUid { get; set; }

        public CommentRating()
        {
        }

        public CommentRating(User user, bool value)
            : base(user, value)
        {
        }
    }
}
