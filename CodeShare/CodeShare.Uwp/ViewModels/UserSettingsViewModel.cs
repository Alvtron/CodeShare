// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="UserSettingsViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class UserSettingsViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ModelSettingsViewModel{CodeShare.Model.User}" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ModelSettingsViewModel{CodeShare.Model.User}" />
    public class UserSettingsViewModel : ModelSettingsViewModel<User>
    {
        /// <summary>
        /// The create new color command
        /// </summary>
        private RelayCommand _createNewColorCommand;
        /// <summary>
        /// Gets the create new color command.
        /// </summary>
        /// <value>The create new color command.</value>
        public ICommand CreateNewColorCommand => _createNewColorCommand = _createNewColorCommand ?? new RelayCommand(async param => await CreateNewColor());

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsViewModel"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public UserSettingsViewModel(User user)
            : base(user)
        {
        }

        /// <summary>
        /// Creates the new color.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CreateNewColor()
        {
            var dialog = new ColorPickerDialog(Model.Color);

            if (await dialog.ShowAsync() != ContentDialogResult.Secondary)
            {
                return false;
            }

            Model.SetColor(dialog.Color.R, dialog.Color.G, dialog.Color.B, dialog.Color.A);
            return true;
        }

        /// <summary>
        /// Sets the user administrator privileges.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool SetUserAdministratorPrivileges(User currentUser)
        {
            return Model?.Equals(currentUser) ?? false;
        }

        /// <summary>
        /// Called when [current user changed].
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        protected override void OnCurrentUserChanged(User currentUser)
        {
            
        }

        /// <summary>
        /// Called when [model changed].
        /// </summary>
        /// <param name="model">The model.</param>
        protected override void OnModelChanged(User model)
        {
            
        }

        /// <summary>
        /// on saving model as an asynchronous operation.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        protected override async Task OnSavingModelAsync(User model)
        {
            
        }
    }
}
