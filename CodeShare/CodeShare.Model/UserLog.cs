using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class UserLog : Log
    {
        public User User { get; set; }
        public Guid UserUid { get; set; }

        public UserLog()
        {
        }

        public UserLog(User user, User actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            UserUid = user.Uid;
        }
    }
}
