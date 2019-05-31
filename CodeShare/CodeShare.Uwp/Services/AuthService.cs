// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="AuthService.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using System;
using System.Threading.Tasks;

namespace CodeShare.Uwp.Services
{
    /// <summary>
    /// Class AuthService.
    /// </summary>
    public static class AuthService
    {
        /// <summary>
        /// The application settings
        /// </summary>
        private static readonly AppSettings AppSettings = new AppSettings();

        /// <summary>
        /// Occurs when [current user changed].
        /// </summary>
        public static event EventHandler CurrentUserChanged;

        /// <summary>
        /// The current user
        /// </summary>
        private static User _currentUser;
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>The current user.</value>
        public static User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentUserChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// refresh as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> RefreshAsync()
        {
            Logger.WriteLine("Attempting to refresh signed-in user...");

            if (CurrentUser == null)
            {
                Logger.WriteLine("Could not refresh user. No user is signed in.");
                await NotificationService.DisplayErrorMessage("Could not update user. No user is signed in.");
                return false;
            }

            var refreshedUser = await RestApiService<User>.Get(CurrentUser.Uid);
            if (refreshedUser == null)
            {
                Logger.WriteLine("Could not refresh user. No user was returned from the database.");
            }

            CurrentUser = new User();
            CurrentUser = refreshedUser;
            return true;
        }

        /// <summary>
        /// Signs out an existing user asynchronously.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> SignOutAsync()
        {
            Logger.WriteLine("Attempting to sign out user...");

            if (CurrentUser == null)
            {
                Logger.WriteLine("Could not sign out user. No user is signed in.");
                await NotificationService.DisplayErrorMessage("Could not sign out user. No user is signed in.");
                return false;
            }

            CurrentUser.SignOut();
            if (!await RestApiService<User>.Update(CurrentUser, CurrentUser.Uid))
            {
                Logger.WriteLine("Could not update signed out user. Sign out was silent.");
            }

            // Resetting current user. Assigning empty user first to trigger property change for all children.
            CurrentUser = new User();
            CurrentUser = null;

            return true;
        }

        /// <summary>
        /// Signs in an existing user with a stored user credential asynchronously.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> SignInAsync()
        {
            Logger.WriteLine("Attempting to sign in user with local user credential...");

            var credential = CredentialService.Current;

            if (credential == null)
            {
                Logger.WriteLine("Failed to sign in user. There are no current credential.");
                return false;
            }

            if (!await AuthenticateAsync(credential.UserName, credential.Password))
            {
                Logger.WriteLine("Failed to sign in user. Local credential could not be verified with the database.");
                return false;
            }

            CurrentUser.SignOut();

            if (!await RestApiService<User>.Update(CurrentUser, CurrentUser.Uid))
            {
                Logger.WriteLine("Failed to update signed-in user. Sign-in was silent.");
            }

            Logger.WriteLine("Sign in succeeded. Local credential matched a user in the database.");
            return true;
        }

        /// <summary>
        /// Signs in an existing user asynchronously.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public static async Task<bool> SignInAsync(string userName, string password)
        {
            Logger.WriteLine("Attempting to sign in user with new user credential...");

            var credential = CredentialService.Create(userName, password);

            if (credential == null)
            {
                Logger.WriteLine("Failed to sign in user. Failed to create credential with the provided parameters.");
                return false;
            }

            if (!await AuthenticateAsync(credential.UserName, credential.Password))
            {
                Logger.WriteLine("Failed to sign in user. The new credential could not be verified with the database.");
                return false;
            }

            Logger.WriteLine("Sign in succeeded. The new credential matched a user in the database.");
            return true;
        }

        /// <summary>
        /// Authenticates user with a provided user name and password.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private static async Task<bool> AuthenticateAsync(string userName, string password)
        {
            CurrentUser = await AuthDataSource.SignIn(userName, password);

            AppSettings.DefaultUser = CurrentUser?.Name;

            return CurrentUser != null;
        }

        /// <summary>
        /// Signs up a new user asynchronously.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;User&gt;.</returns>
        public static async Task<User> SignUpAsync(string userName, string email, string password)
        {
            var newUser = await AuthDataSource.SignUp(userName, email, password);

            if (newUser == null)
            {
                Logger.WriteLine("Failed to create new user. Returning empty object.");
                return null;
            }

            CurrentUser = newUser;

            CredentialService.Create(userName, password);

            AppSettings.DefaultUser = userName;

            return newUser;
        }
    }
}
