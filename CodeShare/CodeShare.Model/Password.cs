using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CodeShare.Model
{
    [ComplexType]
    public class Password : IPassword
    {
        public enum Response
        {
            Valid,
            Empty,
            TooShort,
            TooLong,
            NoSymbol,
            NoNumber,
            NoLowerCase,
            NoUpperCase,
        };

        public int Iterations { get; set; } = 100000;
        public byte[] Salt { get; set; }
        public string Hash { get; set; }

        private const int SaltSize = 24;
        private const int HashSize = 20;

        [NotMapped, JsonIgnore]
        public const int PasswordMinLength = 6;
        [NotMapped, JsonIgnore]
        public const int PasswordMaxLength = 100;

        public Password() { }
            
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
        
        public bool ValidatePassword(string password)
        {
            var newHash = CreateHash(password, Salt, Iterations, Convert.FromBase64String(Hash).Length);

            return newHash.Equals(Hash);
        }
        
        private static string CreateHash(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = iterations })
            {
                var newHash = pbkdf2.GetBytes(outputBytes);

                return Convert.ToBase64String(newHash);
            }
        }

        public static Response Validate(string password)
        {
            var regexNumber = new Regex(@"[0-9]+");
            var regexUpperChar = new Regex(@"[A-Z]+");
            var regexLowerChar = new Regex(@"[a-z]+");
            var regexSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrWhiteSpace(password))
                return Response.Empty;

            if (password.Length < PasswordMinLength)
                return Response.TooShort;

            if (password.Length > PasswordMaxLength)
                return Response.TooLong;

            if (!regexLowerChar.IsMatch(password))
                return Response.NoLowerCase;

            if (!regexUpperChar.IsMatch(password))
                return Response.NoUpperCase;

            if (!regexNumber.IsMatch(password))
                return Response.NoNumber;

            if (!regexSymbols.IsMatch(password))
                return Response.NoSymbol;

            return Response.Valid;
        }
    }
}
