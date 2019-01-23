using System;

namespace CodeShare.Model
{
    public interface IAccount
    {
        string Email { get; set; }

        Password Password { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        DateTime? Birthday { get; set; }
    }
}
