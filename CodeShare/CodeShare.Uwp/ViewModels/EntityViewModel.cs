// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 05-29-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="EntityViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class EntityViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ViewModel" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ViewModel" />
    public abstract class EntityViewModel<TEntity> : ViewModel where TEntity : class, IEntity, INotifyPropertyChanged, new()
    {
        /// <summary>
        /// The model
        /// </summary>
        private TEntity _model;
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public TEntity Model
        {
            get => _model;
            set => SetField(ref _model, value);
        }

        /// <summary>
        /// The is user admin
        /// </summary>
        private bool _isUserAdmin;
        /// <summary>
        /// Gets a value indicating whether this instance is user admin.
        /// </summary>
        /// <value><c>true</c> if this instance is user admin; otherwise, <c>false</c>.</value>
        public bool IsUserAdmin
        {
            get => _isUserAdmin;
            private set => SetField(ref _isUserAdmin, value);
        }

        /// <summary>
        /// The is model changed
        /// </summary>
        private bool _isModelChanged;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is model changed.
        /// </summary>
        /// <value><c>true</c> if this instance is model changed; otherwise, <c>false</c>.</value>
        public bool IsModelChanged
        {
            get => _isModelChanged;
            set => SetField(ref _isModelChanged, value);
        }

        /// <summary>
        /// Sets the user administrator privileges.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected abstract bool SetUserAdministratorPrivileges(User currentUser);
        /// <summary>
        /// Called when [current user changed].
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        protected abstract void OnCurrentUserChanged(User currentUser);
        /// <summary>
        /// Called when [model changed].
        /// </summary>
        /// <param name="model">The model.</param>
        protected abstract void OnModelChanged(TEntity model);

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewModel{TEntity}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected EntityViewModel(TEntity model)
        {
            Model = model;
            OnCurrentUserChanged(null, EventArgs.Empty);
            AuthService.CurrentUserChanged += OnCurrentUserChanged;
            Model.PropertyChanged += OnModelChanged;
        }

        /// <summary>
        /// Handles the <see cref="E:CurrentUserChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCurrentUserChanged(object sender, EventArgs eventArgs)
        {
            IsUserAdmin = SetUserAdministratorPrivileges(CurrentUser);
            OnCurrentUserChanged(CurrentUser);
        }

        /// <summary>
        /// Handles the <see cref="E:ModelChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnModelChanged(object sender, PropertyChangedEventArgs e)
        {
            IsModelChanged = true;
            OnModelChanged(Model);
        }

        /// <summary>
        /// refresh as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> RefreshAsync()
        {
            var model = await RestApiService<TEntity>.Get(Model.Uid);
            if (model == null)
            {
                return false;
            }
            Model = model;
            return await AuthService.RefreshAsync();
        }
    }
}
