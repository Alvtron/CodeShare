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
            var credential = CredentialService.Current;

            if (credential == null)
                return false;

            Logger.WriteLine("Attempting to sign in user...");

            return await AuthenticateAsync(credential.UserName, credential.Password);
        }

        public static async Task<bool> SignInAsync(string username, string password)
        {
            Logger.WriteLine("Attempting to sign in user...");

            var credential = CredentialService.Create(username, password);

            return await AuthenticateAsync(credential.UserName, credential.Password);
        }

        private static async Task<bool> AuthenticateAsync(string username, string password)
        {
            CurrentUser = await AuthDataSource.SignIn(username, password);

            AppSettings.DefaultUser = _currentUser?.Name;

            return _currentUser != null;
        }

        public static async Task<User> SignUpAsync(string userName, string email, string password)
        {
            // Create user
            var newUser = new User(userName, email, password);

            if (!newUser.Valid)
                return null;

            newUser = await AuthDataSource.SignUp(newUser);

            if (newUser == null)
                return null;

            CurrentUser = newUser;
            CredentialService.Create(userName, password);

            var settings = new AppSettings();
            settings.DefaultUser = userName;

            return newUser;
        }
    }
}
