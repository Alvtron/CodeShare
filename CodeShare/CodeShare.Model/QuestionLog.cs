using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class QuestionLog : Log
    {
        public virtual Question Question { get; set; }
        public Guid? QuestionUid { get; set; }

        public QuestionLog()
        {
        }

        public QuestionLog(Question question, User actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            QuestionUid = question.Uid;
        }
    }
}
