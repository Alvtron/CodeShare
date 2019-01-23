using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Rating : Entity, IRating
    {
        public User User { get; set; }

        public Content Content { get; set; }

        public Guid ContentUid { get; set; }

        public bool Value { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now;

        public Rating()
        {
        }

        public Rating(User user, bool value)
        {
            User = user;
            Value = value;
        }
    }
}
