using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CodeShare.Model.Services
{
    public static class ValidationService
    {
        public enum Response
        {
            Empty,
            Valid,
            Invalid,
            AlreadyExist,
            DoNotExist,
            UserCreated,
            TooShort,
            TooLong,
            ContainsIllegalCharacters,
            Unavailable,
            NoSymbol,
            NoNumber,
            NoLowerCase,
            NoUpperCase,
        };

        public static int UserNameMinLength = 3;
        public static int UserNameMaxLength = 30;

        public static int PasswordMinLength = 6;
        public static int PasswordMaxLength = 100;

        public static Response ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Response.Empty;

            try
            {
                var mailAddress = new MailAddress(email);
                return (mailAddress.Address == email) ? Response.Valid : Response.Invalid;
            }
            catch
            {
                return Response.Invalid;
            }
        }

        public static Response ValidateName(string username)
        {
            var validCharacters = new Regex(@"[a-zA-Z0-9¨_]+$");

            if (string.IsNullOrWhiteSpace(username))
                return Response.Empty;

            if (username.Length <= UserNameMinLength)
                return Response.TooShort;

            if (username.Length >= UserNameMaxLength)
                return Response.TooLong;

            if (!validCharacters.IsMatch(username))
                return Response.ContainsIllegalCharacters;

            return Response.Valid;
        }

        public static Response ValidatePassword(string password)
        {
            var regexNumber = new Regex(@"[0-9]+");
            var regexUpperChar = new Regex(@"[A-Z]+");
            var regexLowerChar = new Regex(@"[a-z]+");
            var regexSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrWhiteSpace(password))
                return Response.Empty;

            if (password.Length <= PasswordMinLength)
                return Response.TooShort;

            if (password.Length >= PasswordMaxLength)
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
