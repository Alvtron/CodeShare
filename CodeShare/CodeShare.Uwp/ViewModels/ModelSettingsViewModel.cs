// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="ModelSettingsViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Extensions;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using CodeShare.Utilities;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class ModelSettingsViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.EntityViewModel{TEntity}" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="CodeShare.Uwp.ViewModels.EntityViewModel{TEntity}" />
    public abstract class ModelSettingsViewModel<TEntity> : EntityViewModel<TEntity> where TEntity : class, IEntity, INotifyPropertyChanged, new()
    {
        /// <summary>
        /// The model original
        /// </summary>
        private readonly TEntity _modelOriginal;

        /// <summary>
        /// The delete command
        /// </summary>
        private RelayCommand _deleteCommand;
        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public ICommand DeleteCommand => _deleteCommand = _deleteCommand ?? new RelayCommand(async param => await Delete());

        /// <summary>
        /// The save command
        /// </summary>
        private RelayCommand _saveCommand;
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>The save command.</value>
        public ICommand SaveCommand => _saveCommand = _saveCommand ?? new RelayCommand(async param => await SaveChangesAsync());

        /// <summary>
        /// The reset command
        /// </summary>
        private RelayCommand _resetCommand;
        /// <summary>
        /// Gets the reset command.
        /// </summary>
        /// <value>The reset command.</value>
        public ICommand ResetCommand => _resetCommand = _resetCommand ?? new RelayCommand(async param => await Reset());

        /// <summary>
        /// Called when [saving model asynchronous].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        protected abstract Task OnSavingModelAsync(TEntity model);

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSettingsViewModel{TEntity}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected ModelSettingsViewModel(TEntity model)
            : base(model)
        {
            _modelOriginal = model.Clone();
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task Delete()
        {
            Logger.WriteLine($"User requests deletion of {Model}.");

            if (CurrentUser == null)
            {
                Logger.WriteLine($"Could not delete {Model}. User was not signed in.");
                await NotificationService.DisplayErrorMessage("You are not signed in.");
                NavigationService.GoBack();
            }

            var confirmation = await NotificationService.DisplayGeneralMessage($"Delete {Model}", $"Are you sure you want to delete {Model}?", "Yes", "", "No");

            if (confirmation == ContentDialogResult.None)
            {
                return;
            }

            if (!await RestApiService<TEntity>.Delete(Model.Uid))
            {
                Logger.WriteLine($"Could not delete {Model}. An error occurred during the deletion. No changes where made.");
                await NotificationService.DisplayErrorMessage("Deletion failed. Please try again later.");
                return;
            }

            await NavigationService.Navigate("Home");
        }

        /// <summary>
        /// save changes as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SaveChangesAsync()
        {
            Logger.WriteLine($"User requests updating of {Model}.");

            if (CurrentUser == null)
            {
                Logger.WriteLine($"Could not update {Model}. User was not signed in.");
                await NotificationService.DisplayErrorMessage("You are not signed in.");
                NavigationService.GoBack();
            }

            NavigationService.Lock();

            await OnSavingModelAsync(Model);

            if (await UpdateModelAsync())
            {
                Model.CopyProperties(_modelOriginal);
                IsModelChanged = false;
            }

            NavigationService.Unlock();
            NavigationService.GoBack();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Reset()
        {
            Logger.WriteLine($"User requests reset of {Model}.");

            if (CurrentUser == null)
            {
                Logger.WriteLine($"Could not reset {Model}. User was not signed in.");
                await NotificationService.DisplayErrorMessage("You are not signed in.");
                NavigationService.GoBack();
            }

            _modelOriginal.CopyProperties(Model);
            IsModelChanged = false;
            NavigationService.GoBack();
        }

        /// <summary>
        /// update model as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> UpdateModelAsync()
        {
            if (Model == null)
            {
                Logger.WriteLine($"Could not update {Model}. Model was null.");
                await NotificationService.DisplayErrorMessage("Some changes are not valid.");
                return false;
            }

            if (await RestApiService<TEntity>.Update(Model, Model.Uid) == false)
            {
                Logger.WriteLine($"Could not update {Model}. An error occurred during the upload. No changes where made.");
                await NotificationService.DisplayErrorMessage("An error occurred during the upload. No changes where made.");
                NavigationService.Unlock();
                return false;
            }

            return true;
        }
    }
}
