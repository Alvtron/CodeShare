using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CodeShare.Model.Services
{
    public static partial class ValidationService
    {
        public static int UserNameMinLength = 3;
        public static int UserNameMaxLength = 30;

        public static int PasswordMinLength = 6;
        public static int PasswordMaxLength = 100;

        public static ValidationResponse ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ValidationResponse.Empty;

            try
            {
                var mailAddress = new MailAddress(email);
                return (mailAddress.Address == email) ? ValidationResponse.Valid : ValidationResponse.Invalid;
            }
            catch
            {
                return ValidationResponse.Invalid;
            }
        }

        public static ValidationResponse ValidateUserName(string username)
        {
            var validCharacters = new Regex(@"[a-zA-Z0-9¨_]+$");

            if (string.IsNullOrWhiteSpace(username))
                return ValidationResponse.Empty;

            if (username.Length <= UserNameMinLength)
                return ValidationResponse.TooShort;

            if (username.Length >= UserNameMaxLength)
                return ValidationResponse.TooLong;

            if (!validCharacters.IsMatch(username))
                return ValidationResponse.ContainsIllegalCharacters;

            return ValidationResponse.Valid;
        }

        public static ValidationResponse ValidatePassword(string password)
        {
            var regexNumber = new Regex(@"[0-9]+");
            var regexUpperChar = new Regex(@"[A-Z]+");
            var regexLowerChar = new Regex(@"[a-z]+");
            var regexSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrWhiteSpace(password))
                return ValidationResponse.Empty;

            if (password.Length <= PasswordMinLength)
                return ValidationResponse.TooShort;

            if (password.Length >= PasswordMaxLength)
                return ValidationResponse.TooLong;

            if (!regexLowerChar.IsMatch(password))
                return ValidationResponse.NoLowerCase;

            if (!regexUpperChar.IsMatch(password))
                return ValidationResponse.NoUpperCase;

            if (!regexNumber.IsMatch(password))
                return ValidationResponse.NoNumber;

            if (!regexSymbols.IsMatch(password))
                return ValidationResponse.NoSymbol;

            return ValidationResponse.Valid;
        }
    }
}
