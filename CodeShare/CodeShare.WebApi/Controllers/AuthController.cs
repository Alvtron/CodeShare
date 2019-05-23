using CodeShare.Model;
using CodeShare.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace CodeShare.WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : UsersController
    {
        // Encryptor that stores the public and private parameters, and encrypts and decrypts
        private static readonly AsymmetricEncryptor _asymmetricEncryptor = new AsymmetricEncryptor();

        [HttpGet("publickey")]
        public ActionResult<RSAParameters> GetPublicKey()
        {
            return Ok(_asymmetricEncryptor.PublicParameters);
        }

        [HttpPost("signin")]
        public ActionResult<User> SignIn([FromBody] EncryptedCredential encryptedCredential)
        {
            if (encryptedCredential == null)
            {
                Logger.WriteLine("Provided encrypted credential is empty.");
                return BadRequest("Provided encrypted credential is empty.");
            }
            if (!IsDatabaseOnline)
            {
                Logger.WriteLine("Internal Server Error.");
                return BadRequest("Internal Server Error.");
            }

            // Decrypt encrypted user credentials
            var userName = _asymmetricEncryptor.Decrypt(encryptedCredential.UserName);
            var password = _asymmetricEncryptor.Decrypt(encryptedCredential.Password);

            // Find user with decrypted credentials
            var user = QueryableEntities.FirstOrDefault(u => u.Name == userName);

            if (user == null || !user.Password.ValidatePassword(password))
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("signup")]
        public ActionResult<User> SignUp([FromBody] User user)
        {
            if (!IsDatabaseOnline)
            {
                Logger.WriteLine("Internal Server Error.");
                return BadRequest("Internal Server Error.");
            }
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
            if (Entities.Any(u => u.Name == user.Name || u.Email.Address == user.Email.Address))
            {
                return BadRequest("A user with that username or email already exists.");
            }

            try
            {
                Context.Users.Add(user);
                Context.SaveChanges();

                return Ok(user);
            }
            catch (Exception exception)
            {
#if DEBUG
                Logger.WriteLine($"{exception.Source}: {exception.Message}");
                throw;
#else
                Logger.WriteLine($"{exception.Source}: {exception.Message}");
                return BadRequest($"{exception.Source}: {exception.Message}");
#endif
            }
        }

        [HttpPost("change/{uid}")]
        public ActionResult<User> ChangeCredentials(Guid uid, [FromBody] EncryptedCredential encryptedCredential)
        {
            if (encryptedCredential == null)
            {
                Logger.WriteLine("Provided encrypted credential is empty.");
                return BadRequest("Provided encrypted credential is empty.");
            }
            if (!IsDatabaseOnline)
            {
                Logger.WriteLine("Internal Server Error.");
                return BadRequest("Internal Server Error.");
            }

            // Decrypt encrypted user credentials
            var userName = _asymmetricEncryptor.Decrypt(encryptedCredential.UserName);
            var password = _asymmetricEncryptor.Decrypt(encryptedCredential.Password);

            // Find user with decrypted credentials
            var user = Context.Users.Find(uid);

            if (user == null) return NotFound();

            // TODO: Create change functions, or update the logs via the observable properties
            user.Password = new Password(password);
            user.Name = userName;

            Context.SaveChanges();

            return Ok(user);
        }
    }
}