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

        public CommentLog(bool isPublic, string action, Guid? actor = null, IEntity subject = null)
            : base(isPublic, action, actor, subject)
        {
            Action = action;
            ActorUid = actor;
            SubjectUid = subject?.Uid;
            SubjectType = subject?.GetType().Name;
            IsPublic = isPublic;
        }
    }
}
