// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="SignInViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Xaml.Media;
using CodeShare.Utilities;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class SignInViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.DialogViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.DialogViewModel" />
    public class SignInViewModel : DialogViewModel
    {
        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <value>The credentials.</value>
        public ObservableCollection<PasswordCredential> Credentials => CredentialService.All;

        /// <summary>
        /// The log in command
        /// </summary>
        private RelayCommand<PasswordCredential> _logInCommand;
        /// <summary>
        /// Gets the log in command.
        /// </summary>
        /// <value>The log in command.</value>
        public ICommand LogInCommand => _logInCommand = _logInCommand ?? new RelayCommand<PasswordCredential>(async param => await LogIn(param));
        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Task.</returns>
        public async Task LogIn(PasswordCredential credential)
        {
            ErrorMessage = "";

            credential.RetrievePassword();
            Logger.WriteLine($"Logging in with credential (User name: {credential.UserName}, password:{credential.Password})...");

            if (await AuthService.SignInAsync(credential.UserName, credential.Password) == false)
            {
                Logger.WriteLine("Login failed. The credential does not match any user from the database.");
                ErrorMessage = "That user is no longer registered. Sorry about that.";
                CredentialService.Delete(credential);
                Logger.WriteLine($"Credential (User name: {credential.UserName}, password:{credential.Password}) was deleted.");
                return;
            }

            CanClose = true;
        }

        /// <summary>
        /// The delete users command
        /// </summary>
        private RelayCommand _deleteUsersCommand;
        /// <summary>
        /// Gets the delete users command.
        /// </summary>
        /// <value>The delete users command.</value>
        public ICommand DeleteUsersCommand => _deleteUsersCommand = _deleteUsersCommand ?? new RelayCommand(param => DeleteUsers());
        /// <summary>
        /// Deletes the users.
        /// </summary>
        private void DeleteUsers()
        {
            CredentialService.DeleteAll();
            NotifyPropertyChanged(nameof(Credentials));
        }

        /// <summary>
        /// The sign in command
        /// </summary>
        private RelayCommand _signInCommand;
        /// <summary>
        /// Gets the sign in command.
        /// </summary>
        /// <value>The sign in command.</value>
        public ICommand SignInCommand => _signInCommand = _signInCommand ?? new RelayCommand(async param => await SignIn());

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task SignIn()
        {
            Logger.WriteLine($"Sign in in user with entered user name: '{SignInName}' and password: '{SignInPassword}'");

            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(SignInName) || string.IsNullOrWhiteSpace(SignInPassword))
            {
                Logger.WriteLine("Sign in failed. Username or password are empty.");
                ErrorMessage = "Username or password are empty!";
                return;
            }

            if (await AuthService.SignInAsync(SignInName, SignInPassword) == false)
            {
                Logger.WriteLine("Sign in failed. Username and password does not match anything.");
                ErrorMessage = "Wrong username or password.";
                return;
            }

            CanClose = true;
        }

        /// <summary>
        /// The sign up command
        /// </summary>
        private RelayCommand _signUpCommand;
        /// <summary>
        /// Gets the sign up command.
        /// </summary>
        /// <value>The sign up command.</value>
        public ICommand SignUpCommand => _signUpCommand = _signUpCommand ?? new RelayCommand(async param => await SignUp());
        /// <summary>
        /// Signs up.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task SignUp()
        {
            ErrorMessage = "";

            if (!ValidSignUp)
            {
                ErrorMessage = "Some of the fields are invalid.";
                return;
            }

            var newUser = await AuthService.SignUpAsync(SignUpName, SignUpEmail, SignUpPassword);

            if (newUser == null)
            {
                ErrorMessage = "Could not sign up user.";
                return;
            }

            CanClose = true;
        }

        /// <summary>
        /// The error message
        /// </summary>
        private string _errorMessage;
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        /// <summary>
        /// The sign in name
        /// </summary>
        private string _signInName;
        /// <summary>
        /// Gets or sets the name of the sign in.
        /// </summary>
        /// <value>The name of the sign in.</value>
        public string SignInName
        {
            get => _signInName;
            set => SetField(ref _signInName, value);
        }

        /// <summary>
        /// The sign in password
        /// </summary>
        private string _signInPassword;
        /// <summary>
        /// Gets or sets the sign in password.
        /// </summary>
        /// <value>The sign in password.</value>
        public string SignInPassword
        {
            get => _signInPassword;
            set => SetField(ref _signInPassword, value);
        }

        /// <summary>
        /// The sign up email
        /// </summary>
        private string _signUpEmail;
        /// <summary>
        /// Gets or sets the sign up email.
        /// </summary>
        /// <value>The sign up email.</value>
        public string SignUpEmail
        {
            get => _signUpEmail;
            set
            {
                SetField(ref _signUpEmail, value);
                ValidateEmail();
            }
        }

        /// <summary>
        /// The sign up name
        /// </summary>
        private string _signUpName;
        /// <summary>
        /// Gets or sets the name of the sign up.
        /// </summary>
        /// <value>The name of the sign up.</value>
        public string SignUpName
        {
            get => _signUpName;
            set
            {
                SetField(ref _signUpName, value);
                ValidateName();
            }
        }

        /// <summary>
        /// The sign up password
        /// </summary>
        private string _signUpPassword;
        /// <summary>
        /// Gets or sets the sign up password.
        /// </summary>
        /// <value>The sign up password.</value>
        public string SignUpPassword
        {
            get => _signUpPassword;
            set
            {
                SetField(ref _signUpPassword, value);
                ValidatePassword();
                ValidateReEnteredPassword();
            }
        }

        /// <summary>
        /// The sign up re enter password
        /// </summary>
        private string _signUpReEnterPassword;
        /// <summary>
        /// Gets or sets the sign up re enter password.
        /// </summary>
        /// <value>The sign up re enter password.</value>
        public string SignUpReEnterPassword
        {
            get => _signUpReEnterPassword;
            set
            {
                SetField(ref _signUpReEnterPassword, value);
                ValidatePassword();
                ValidateReEnteredPassword();
            }
        }

        /// <summary>
        /// The sign up email border
        /// </summary>
        private SolidColorBrush _signUpEmailBorder = new SolidColorBrush(Colors.DimGray);
        /// <summary>
        /// Gets or sets the sign up email border.
        /// </summary>
        /// <value>The sign up email border.</value>
        public SolidColorBrush SignUpEmailBorder
        {
            get => _signUpEmailBorder;
            set => SetField(ref _signUpEmailBorder, value);
        }

        /// <summary>
        /// The sign up name border
        /// </summary>
        private SolidColorBrush _signUpNameBorder = new SolidColorBrush(Colors.DimGray);
        /// <summary>
        /// Gets or sets the sign up name border.
        /// </summary>
        /// <value>The sign up name border.</value>
        public SolidColorBrush SignUpNameBorder
        {
            get => _signUpNameBorder;
            set => SetField(ref _signUpNameBorder, value);
        }

        /// <summary>
        /// The sign up password border
        /// </summary>
        private SolidColorBrush _signUpPasswordBorder = new SolidColorBrush(Colors.DimGray);
        /// <summary>
        /// Gets or sets the sign up password border.
        /// </summary>
        /// <value>The sign up password border.</value>
        public SolidColorBrush SignUpPasswordBorder
        {
            get => _signUpPasswordBorder;
            set => SetField(ref _signUpPasswordBorder, value);
        }

        /// <summary>
        /// The sign up re enter password border
        /// </summary>
        private SolidColorBrush _signUpReEnterPasswordBorder = new SolidColorBrush(Colors.DimGray);
        /// <summary>
        /// Gets or sets the sign up re enter password border.
        /// </summary>
        /// <value>The sign up re enter password border.</value>
        public SolidColorBrush SignUpReEnterPasswordBorder
        {
            get => _signUpReEnterPasswordBorder;
            set => SetField(ref _signUpReEnterPasswordBorder, value);
        }

        /// <summary>
        /// Gets a value indicating whether [valid sign up].
        /// </summary>
        /// <value><c>true</c> if [valid sign up]; otherwise, <c>false</c>.</value>
        private bool ValidSignUp => ValidateEmail() && ValidateName() && ValidatePassword() && ValidateReEnteredPassword();

        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateEmail()
        {
            if (Validate.Email(SignUpEmail) != ValidationResponse.Valid)
            {
                SignUpEmailBorder = new SolidColorBrush(Colors.Red);
                return false;
            }
            SignUpEmailBorder = new SolidColorBrush(Colors.Green);
            return true;
        }

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateName()
        {
            if (Validate.UserName(SignUpName) != ValidationResponse.Valid)
            {
                SignUpNameBorder = new SolidColorBrush(Colors.Red);
                return false;
            }
            SignUpNameBorder = new SolidColorBrush(Colors.Green);
            return true;
        }

        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidatePassword()
        {
            if (Validate.Password(SignUpPassword) != ValidationResponse.Valid)
            {
                SignUpPasswordBorder = new SolidColorBrush(Colors.Red);
                return false;
            }
            SignUpPasswordBorder = new SolidColorBrush(Colors.Green);
            return true;
        }

        /// <summary>
        /// Validates the re entered password.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateReEnteredPassword()
        {
            if (SignUpPassword != SignUpReEnterPassword)
            {
                SignUpReEnterPasswordBorder = new SolidColorBrush(Colors.Red);
                return false;
            }
            SignUpReEnterPasswordBorder = new SolidColorBrush(Colors.Green);
            return true;
        }
    }
}
