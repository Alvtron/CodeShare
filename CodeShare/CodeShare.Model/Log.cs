using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Log : Entity, ILog
    {
        public bool IsPublic { get; set; }
        
        public string Action { get; set; }

        public Guid? SubjectUid { get; set; }

        public string SubjectType { get; set; }

        public User Actor { get; set; }

        public Guid? ActorUid { get; set; }

        public Log()
        {
        }

        public Log(bool isPublic, string action, Guid? actor = null, IEntity subject = null)
        {
            Action = action;
            ActorUid = actor;
            SubjectUid = subject?.Uid;
            SubjectType = subject?.GetType().Name;
            IsPublic = isPublic;
        }
    }
}
