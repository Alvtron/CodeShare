using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class CodeLog : Log
    {
        public virtual Code Code { get; set; }
        public Guid? CodeUid { get; set; }

        public CodeLog()
        {
        }

        public CodeLog(Code code, User actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            CodeUid = code.Uid;
        }
    }
}
