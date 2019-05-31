// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 05-29-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="ViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class ViewModel.
    /// Implements the <see cref="CodeShare.Model.ObservableObject" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.ObservableObject" />
    public abstract class ViewModel : ObservableObject
    {
        /// <summary>
        /// The current user
        /// </summary>
        private User _currentUser;
        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>The current user.</value>
        public User CurrentUser
        {
            get => _currentUser;
            private set => SetField(ref _currentUser, value);
        }

        /// <summary>
        /// The is user logged in
        /// </summary>
        private bool _isUserLoggedIn;
        /// <summary>
        /// Gets a value indicating whether this instance is user logged in.
        /// </summary>
        /// <value><c>true</c> if this instance is user logged in; otherwise, <c>false</c>.</value>
        public bool IsUserLoggedIn
        {
            get => _isUserLoggedIn;
            private set => SetField(ref _isUserLoggedIn, value);
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
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        protected ViewModel()
        {
            OnCurrentUserChanged(null, EventArgs.Empty);
            AuthService.CurrentUserChanged += OnCurrentUserChanged;
        }

        /// <summary>
        /// Handles the <see cref="E:CurrentUserChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCurrentUserChanged(object sender, EventArgs eventArgs)
        {
            CurrentUser = AuthService.CurrentUser;
            IsUserLoggedIn = CurrentUser != null;
        }

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <returns>Task.</returns>
        private static async Task SignIn()
        {
            var signInDialog = new SignInDialog();
            await signInDialog.ShowAsync();
        }
    }
}
