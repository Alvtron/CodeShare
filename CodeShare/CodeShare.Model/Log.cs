using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class Log : Entity, ILog
    {
        public bool IsPublic { get; set; }
        public string Action { get; set; }
        public Guid? ActorUid { get; set; }
        public string ActorType { get; set; }
        public Guid? SubjectUid { get; set; }
        public string SubjectType { get; set; }

        public Log()
        {
        }

        public Log(IEntity actor, string action, IEntity subject = null, bool isPublic = true)
        {
            Action = action;
            ActorUid = actor?.Uid;
            ActorType = actor?.GetType()?.Name;
            SubjectUid = subject?.Uid;
            SubjectType = subject?.GetType()?.Name;
            IsPublic = isPublic;
        }
    }
}
