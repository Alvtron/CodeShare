using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class Notification : Entity
    {
        public User User { get; set; }
        public Guid? UserUid { get; set; }
        public Log Log { get; set; }
        public Guid? LogUid { get; set; }
        public DateTime Seen { get; set; }
    }
}
