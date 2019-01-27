using System;

namespace CodeShare.Model
{
    public interface ILog : IEntity
    {
        bool IsPublic { get; set; }
        string Action { get; set; }
        Guid? ActorUid { get; set; }
        string ActorType { get; set; }
        Guid? SubjectUid { get; set; }
        string SubjectType { get; set; }
    }
}
