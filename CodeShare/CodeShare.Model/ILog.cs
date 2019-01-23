using System;

namespace CodeShare.Model
{
    public interface ILog
    {
        Guid Uid { get; set; }
        bool IsPublic { get; set; }
        string Action { get; set; }
        DateTime? Created { get; set; }
        DateTime? Updated { get; set; }
        Guid? SubjectUid { get; set; }
        string SubjectType { get; set; }
        User Actor { get; set; }
        Guid? ActorUid { get; set; }
    }
}
