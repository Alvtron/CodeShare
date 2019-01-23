using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Report : Entity
    {
        [Required]
        public User User { get; set; }

        [Required]
        public DateTime? Date { get; set; } = DateTime.Now;

        [Required]
        public Guid Target { get; set; }

        public string Message { get; set; }

        [NotMapped]
        public bool Valid => User != null && string.IsNullOrWhiteSpace(Message);

        public Report()
        {
        }

        public Report(Guid target, User user, string message)
        {
            Target = target;
            User = user;
            Message = message;
        }
    }
}
