using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using System;
using System.Threading.Tasks;

namespace CodeShare.Uwp.Services
{
    public static class AuthService
    {
        private static readonly AppSettings AppSettings = new AppSettings();

        public static event EventHandler CurrentUserChanged;

        private static User _currentUser;
        public static User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentUserChanged?.Invoke(null, EventArgs.Empty);
            }
        }

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

            CurrentUser = null;

            return true;
        }

        public static async Task<bool> SignInAsync()
        {
            Logger.WriteLine("Attempting to sign in user with local user credential...");

            var credential = CredentialService.Current;

            if (credential == null)
            {
                Logger.WriteLine("Failed to sign in user. There are no current credential.");
                return false;
            }

            if (await AuthenticateAsync(credential.UserName, credential.Password))
            {
                Logger.WriteLine("Failed to sign in user. Local credential could not be verified with the database.");
                return false;
            }

            Logger.WriteLine("Sign in succeeded. Local credential matched a user in the database.");
            return true;
        }

        public static async Task<bool> SignInAsync(string username, string password)
        {
            Logger.WriteLine("Attempting to sign in user with new user credential...");

            var credential = CredentialService.Create(username, password);

            if (credential == null)
            {
                Logger.WriteLine("Failed to sign in user. Failed to create credential with the provided parameters.");
                return false;
            }

            if (await AuthenticateAsync(credential.UserName, credential.Password))
            {
                Logger.WriteLine("Failed to sign in user. The new credential could not be verified with the database.");
                return false;
            }

            Logger.WriteLine("Sign in succeeded. The new credential matched a user in the database.");
            return true;
        }

        private static async Task<bool> AuthenticateAsync(string username, string password)
        {
            CurrentUser = await AuthDataSource.SignIn(username, password);

            AppSettings.DefaultUser = CurrentUser?.Name;

            return CurrentUser != null;
        }

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
