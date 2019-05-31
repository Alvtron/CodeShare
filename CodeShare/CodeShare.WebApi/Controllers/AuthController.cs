// ***********************************************************************
// Assembly         : CodeShare.WebApi
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="AuthController.cs" company="CodeShare.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using CodeShare.DataAccess;

namespace CodeShare.WebApi.Controllers
{
    /// <summary>
    /// Class AuthController.
    /// Implements the <see cref="CodeShare.WebApi.Controllers.UsersController" />
    /// </summary>
    /// <seealso cref="CodeShare.WebApi.Controllers.UsersController" />
    [Route("api/auth")]
    [ApiController]
    public class AuthController : UsersController
    {
        // Encryptor that stores the public and private parameters, and encrypts and decrypts
        /// <summary>
        /// The asymmetric encryptor
        /// </summary>
        private static readonly AsymmetricEncryptor AsymmetricEncryptor = new AsymmetricEncryptor();

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <returns>ActionResult&lt;RSAParameters&gt;.</returns>
        [HttpGet("publickey")]
        public ActionResult<RSAParameters> GetPublicKey()
        {
            return Ok(AsymmetricEncryptor.PublicParameters);
        }

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="encryptedCredential">The encrypted credential.</param>
        /// <returns>ActionResult&lt;User&gt;.</returns>
        [HttpPost("signin")]
        public ActionResult<User> SignIn([FromBody] EncryptedCredential encryptedCredential)
        {
            if (encryptedCredential == null)
            {
                Logger.WriteLine("Provided encrypted credential is empty.");
                return BadRequest("Provided encrypted credential is empty.");
            }

            // Decrypt encrypted user credentials
            var userName = AsymmetricEncryptor.Decrypt(encryptedCredential.UserName);
            var password = AsymmetricEncryptor.Decrypt(encryptedCredential.Password);
            try
            {
                using (var context = new DataContext())
                {
                    var set = GetDatabaseSet(context);
                    var entities = GetNavigationalEntities(set);
                    var user = entities.FirstOrDefault(u => u.Name == userName);

                    if (user == null || !user.Password.ValidatePassword(password))
                    {
                        return NotFound();
                    }

                    return Ok(user);
                }
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Signs up.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>ActionResult&lt;User&gt;.</returns>
        [HttpPost("signup")]
        public ActionResult<User> SignUp([FromBody] User user)
        {
            if (user == null)
            {
                Logger.WriteLine("Provided user is empty.");
                return BadRequest("Provided user is empty.");
            }
            if (!user.IsValid)
            {
                Logger.WriteLine("Provided user is invalid.");
                return BadRequest("Provided user is invalid.");
            }

            using (var context = new DataContext())
            {
                var set = GetDatabaseSet(context);
                var entities = GetNavigationalEntities(set);
                if (entities.Any(u => u.Name == user.Name || u.Email.Address == user.Email.Address))
                {
                    return BadRequest("A user with that username or email already exists.");
                }

                try
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                catch (Exception exception)
                {
                    Logger.WriteLine($"{exception.Source}: {exception.Message}");
                    throw;
                }

                return Ok(user);
            }
        }

        /// <summary>
        /// Changes the credentials.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="encryptedCredential">The encrypted credential.</param>
        /// <returns>ActionResult&lt;User&gt;.</returns>
        [HttpPost("change/{uid}")]
        public ActionResult<User> ChangeCredentials(Guid uid, [FromBody] EncryptedCredential encryptedCredential)
        {
            if (encryptedCredential == null)
            {
                Logger.WriteLine("Provided encrypted credential is empty.");
                return BadRequest("Provided encrypted credential is empty.");
            }

            // Decrypt encrypted user credentials
            var userName = AsymmetricEncryptor.Decrypt(encryptedCredential.UserName);
            var password = AsymmetricEncryptor.Decrypt(encryptedCredential.Password);

            try
            {
                using (var context = new DataContext())
                {
                    var user = context.Users.Find(uid);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    user.Password = new Password(password);
                    user.Name = userName;
                    context.SaveChanges();
                    return Ok(user);
                }
            }
            catch (Exception exception)
            {
                Logger.WriteLine($"{exception.Source}: {exception.Message}");
                throw;
            }
        }
    }
}