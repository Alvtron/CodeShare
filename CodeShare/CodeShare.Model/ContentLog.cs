using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class ContentLog : Log
    {
        public Content Content { get; set; }
        public Guid? ContentUid { get; set; }

        public ContentLog()
        {
        }

        public ContentLog(bool isPublic, string action, Guid? actor = null, IEntity subject = null)
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
