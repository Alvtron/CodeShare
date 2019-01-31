using CodeShare.Model;
using CodeShare.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Description;

namespace CodeShare.WebApi.Controllers
{
    public class AuthController : UsersController
    {
        // Encryptor that stores the public and private parameters, and encrypts and decrypts
        private static readonly AsymmetricEncryptor _asymmetricEncryptor = new AsymmetricEncryptor();

        [ResponseType(typeof(RSAParameters)), Route("api/auth/publickey")]
        public IHttpActionResult GetPublicKey()
        {
            return Ok(_asymmetricEncryptor.PublicParameters);
        }

        [ResponseType(typeof(User)), Route("api/auth/signin")]
        public IHttpActionResult SignIn([FromBody] EncryptedCredential encryptedCredential)
        {
            if (encryptedCredential == null) return BadRequest("Provided encrypted credential is empty.");
            if (!IsDatabaseOnline) return InternalServerError();

            // Decrypt encrypted user credentials
            var userName = _asymmetricEncryptor.Decrypt(encryptedCredential.UserName);
            var password = _asymmetricEncryptor.Decrypt(encryptedCredential.Password);

            // Find user with decrypted credentials
            var user = Entities.FirstOrDefault(u => u.Name == userName);

            if (user == null || !user.Password.ValidatePassword(password))
                return NotFound();

            return Content(HttpStatusCode.OK, user);
        }

        [ResponseType(typeof(User)), Route("api/auth/signup")]
        public IHttpActionResult SignUp([FromBody] User user)
        {
            if (!IsDatabaseOnline)
                return InternalServerError();
            if (user == null)
                return BadRequest("Provided user is empty.");
            if (!user.Valid)
                return BadRequest("Provided user is invalid.");
            if (Entities.Any(u => u.Name == user.Name || u.Email == user.Email))
                return BadRequest("A user with that username or email already exists.");

            try
            {
                Context.Users.Add(user);
                Context.SaveChanges();

                return Content(HttpStatusCode.OK, user);
            }
            catch (Exception exception)
            {
#if DEBUG
                Logger.WriteLine($"{exception.Source}: {exception.Message}");
                throw;
#else
                return BadRequest($"{exception.Source}: {exception.Message}");
#endif
            }
        }

        [ResponseType(typeof(User)), Route("api/auth/change/{uid}")]
        public IHttpActionResult ChangeCredentials(Guid uid, [FromBody] EncryptedCredential encryptedCredential)
        {
            if (encryptedCredential == null)
                return BadRequest("Provided encrypted credential is empty.");
            if (!IsDatabaseOnline)
                return InternalServerError();

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

            return Content(HttpStatusCode.OK, user);
        }
    }
}