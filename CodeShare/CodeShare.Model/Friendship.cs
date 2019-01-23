using System;
using System.ComponentModel.DataAnnotations;

namespace CodeShare.Model
{
    public class Friendship : Entity
    {
        [Required]
        public User FriendA { get; set; }

        [Required]
        public User FriendB { get; set; }

        public DateTime Since { get; set; } = DateTime.Now;

        public bool Contains(User user) => FriendA.Equals(user) || FriendB.Equals(user);

        public Friendship(User friendA, User friendB)
        {
            FriendA = friendA;
            FriendB = friendB;
        }
    }
}