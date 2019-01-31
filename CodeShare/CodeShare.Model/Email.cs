using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Mail;
using CodeShare.Utilities;

namespace CodeShare.Model
{
    [ComplexType]
    public class Email
    {
        public string Address { get; set; }

        [NotMapped, JsonIgnore]
        public string Name => Address.Split('@').FirstOrDefault();
        [NotMapped, JsonIgnore]
        public string Domain => Address.Split('@').LastOrDefault();

        public Email() { }

        public Email(string emailAddress)
        {
            if (Validate.Email(emailAddress) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Provided E-mail string was invalid.");
            }

            Address = emailAddress;
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
