// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 01-31-2019
// ***********************************************************************
// <copyright file="EncryptedCredential.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Utilities;
using System.Security.Cryptography;

namespace CodeShare.Model
{
    /// <summary>
    /// Class EncryptedCredential.
    /// </summary>
    public class EncryptedCredential
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedCredential"/> class.
        /// </summary>
        public EncryptedCredential() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedCredential"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="parameters">The parameters.</param>
        public EncryptedCredential(string userName, string password, RSAParameters parameters)
        {
            UserName = AsymmetricEncryptor.Encrypt(userName, parameters);
            Password = AsymmetricEncryptor.Encrypt(password, parameters);
        }

        /// <summary>
        /// Equalses the specified credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Equals(EncryptedCredential credential)
        {
            return UserName.Equals(credential.UserName) && Password.Equals(credential.Password);
        }
    }
}
