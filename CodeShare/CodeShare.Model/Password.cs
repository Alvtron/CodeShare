// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Password.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Password.
    /// Implements the <see cref="CodeShare.Model.IPassword" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IPassword" />
    public class Password : IPassword
    {
        /// <summary>
        /// Gets or sets the iterations.
        /// </summary>
        /// <value>The iterations.</value>
        public int Iterations { get; set; } = 100000;
        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        /// <value>The salt.</value>
        public byte[] Salt { get; set; }
        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash { get; set; }

        /// <summary>
        /// The salt size
        /// </summary>
        private const int SaltSize = 24;
        /// <summary>
        /// The hash size
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// The password minimum length
        /// </summary>
        [NotMapped, JsonIgnore]
        public const int PasswordMinLength = 6;
        /// <summary>
        /// The password maximum length
        /// </summary>
        [NotMapped, JsonIgnore]
        public const int PasswordMaxLength = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        public Password() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        public Password(string password)
        {
            // Create salt 24 bytes large
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var saltSizeByte = new byte[SaltSize];
                cryptoProvider.GetBytes(saltSizeByte);

                // Create hash with salt and iterations, 20 bytes large
                Salt = saltSizeByte;
                Hash = CreateHash(password, saltSizeByte, Iterations, HashSize);
            }
        }

        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ValidatePassword(string password)
        {
            var newHash = CreateHash(password, Salt, Iterations, Convert.FromBase64String(Hash).Length);
            return newHash.Equals(Hash);
        }

        /// <summary>
        /// Creates the hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The iterations.</param>
        /// <param name="outputBytes">The output bytes.</param>
        /// <returns>System.String.</returns>
        private static string CreateHash(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = iterations })
            {
                var newHash = pbkdf2.GetBytes(outputBytes);
                return Convert.ToBase64String(newHash);
            }
        }
    }
}
