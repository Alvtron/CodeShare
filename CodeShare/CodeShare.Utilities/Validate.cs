// ***********************************************************************
// Assembly         : CodeShare.Utilities
// Author           : Thomas Angeland
// Created          : 01-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Validate.cs" company="CodeShare.Utilities">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CodeShare.Utilities
{
    /// <summary>
    /// Class Validate.
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// The user name minimum length
        /// </summary>
        public static int UserNameMinLength = 3;
        /// <summary>
        /// The user name maximum length
        /// </summary>
        public static int UserNameMaxLength = 30;
        /// <summary>
        /// The password minimum length
        /// </summary>
        public static int PasswordMinLength = 6;
        /// <summary>
        /// The password maximum length
        /// </summary>
        public static int PasswordMaxLength = 100;

        /// <summary>
        /// Emails the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>ValidationResponse.</returns>
        public static ValidationResponse Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return ValidationResponse.Empty;
            }

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    var domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return ValidationResponse.Invalid;
            }
            catch (ArgumentException)
            {
                return ValidationResponse.Invalid;
            }

            try
            {
                const string regex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

                return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))
                    ? ValidationResponse.Valid
                    : ValidationResponse.Invalid;
            }
            catch (RegexMatchTimeoutException)
            {
                return ValidationResponse.Invalid;
            }
        }

        /// <summary>
        /// Users the name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>ValidationResponse.</returns>
        public static ValidationResponse UserName(string username)
        {
            var validCharacters = new Regex(@"[a-zA-Z0-9¨_]+$");

            if (string.IsNullOrWhiteSpace(username))
            {
                return ValidationResponse.Empty;
            }
            if (username.Length <= UserNameMinLength)
            {
                return ValidationResponse.TooShort;
            }
            if (username.Length >= UserNameMaxLength)
            {
                return ValidationResponse.TooLong;
            }
            if (!validCharacters.IsMatch(username))
            {
                return ValidationResponse.ContainsIllegalCharacters;
            }

            return ValidationResponse.Valid;
        }

        /// <summary>
        /// Passwords the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>ValidationResponse.</returns>
        public static ValidationResponse Password(string password)
        {
            var regexNumber = new Regex(@"[0-9]+");
            var regexUpperChar = new Regex(@"[A-Z]+");
            var regexLowerChar = new Regex(@"[a-z]+");
            var regexSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrWhiteSpace(password))
            {
                return ValidationResponse.Empty;
            }
            if (password.Length <= PasswordMinLength)
            {
                return ValidationResponse.TooShort;
            }
            if (password.Length >= PasswordMaxLength)
            {
                return ValidationResponse.TooLong;
            }
            if (!regexLowerChar.IsMatch(password))
            {
                return ValidationResponse.NoLowerCase;
            }
            if (!regexUpperChar.IsMatch(password))
            {
                return ValidationResponse.NoUpperCase;
            }
            if (!regexNumber.IsMatch(password))
            {
                return ValidationResponse.NoNumber;
            }
            if (!regexSymbols.IsMatch(password))
            {
                return ValidationResponse.NoSymbol;
            }
            return ValidationResponse.Valid;
        }
    }
}
