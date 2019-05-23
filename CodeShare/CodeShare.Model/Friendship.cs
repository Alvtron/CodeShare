using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class Friendship : Entity
    {
        public Guid RequesterUid { get; set; }
        public virtual User Requester { get; set; }
        public Guid ConfirmerUid { get; set; }
        public virtual User Confirmer { get; set; }
        public DateTime? Confirmed { get; set; }
        public Friendship() { }

        public Friendship(User requester, User confirmer)
        {
            if (requester.Equals(confirmer))
            {
                throw new ArgumentException("requester and confirmer are the same user.");
            }

            Requester = requester ?? throw new ArgumentNullException(nameof(requester));
            Confirmer = confirmer ?? throw new ArgumentNullException(nameof(confirmer));
        }
    }
}
