using CodeShare.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Report : Entity
    {
        public Guid? TargetUid { get; set; }
        public string TargetType { get; set; }
        public string Message { get; set; }
        public bool Valid => string.IsNullOrWhiteSpace(Message);
        public virtual ICollection<ReportImage> ImageAttachments { get; set; } = new List<ReportImage>();

        public Report()
        {
        }

        public Report(IEntity target, string message)
        {
            TargetUid = target?.Uid;
            TargetType = target?.GetType()?.Name;
            Message = message;
        }
    }
}
