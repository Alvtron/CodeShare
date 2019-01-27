using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeShare.Model
{
    public class Answer : Comment
    {
        [Required]
        public Question Question { get; set; }

        public Answer() : base()
        {

        }

        public Answer(User user, Guid contentUid, string message)
            : base(user, contentUid, message)
        {

        }
    }
}
