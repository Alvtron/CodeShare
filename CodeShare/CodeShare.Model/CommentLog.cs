using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class CommentLog : Log
    {
        public Comment Comment { get; set; }
        public Guid? CommentUid { get; set; }

        public CommentLog()
        {
        }

        public CommentLog(Comment comment, User actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            CommentUid = comment.Uid;
        }
    }
}
