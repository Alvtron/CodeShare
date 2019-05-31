// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-25-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Email.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using CodeShare.Utilities;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Email.
    /// </summary>
    [ComplexType]
    public class Email
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [NotMapped, JsonIgnore]
        public string Name => Address.Split('@').FirstOrDefault();
        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>The domain.</value>
        [NotMapped, JsonIgnore]
        public string Domain => Address.Split('@').LastOrDefault();

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        public Email() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <exception cref="ArgumentException">Provided E-mail string was invalid.</exception>
        public Email(string emailAddress)
        {
            if (Validate.Email(emailAddress) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Provided E-mail string was invalid.");
            }

            Address = emailAddress;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Address;
        }
    }
}
