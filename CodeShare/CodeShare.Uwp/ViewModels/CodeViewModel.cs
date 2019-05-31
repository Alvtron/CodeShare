// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Threading.Tasks;
using System.Windows.Input;
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Controls;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class CodeViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ContentViewModel{CodeShare.Model.Code}" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ContentViewModel{CodeShare.Model.Code}" />
    public class CodeViewModel : ContentViewModel<Code>
    {
        /// <summary>
        /// The edit command
        /// </summary>
        private RelayCommand _editCommand;
        /// <summary>
        /// Gets the edit command.
        /// </summary>
        /// <value>The edit command.</value>
        public ICommand EditCommand => _editCommand = _editCommand ?? new RelayCommand(param => Edit());

        /// <summary>
        /// The upload command
        /// </summary>
        private RelayCommand<Editor> _uploadCommand;
        /// <summary>
        /// Gets the upload command.
        /// </summary>
        /// <value>The upload command.</value>
        public ICommand UploadCommand => _uploadCommand = _uploadCommand ?? new RelayCommand<Editor>(async editor => await UploadCommentAsync(editor));

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeViewModel"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public CodeViewModel(Code code)
            : base(code)
        {
        }

        /// <summary>
        /// Sets the user administrator privileges.
        /// </summary>
        /// <param name="currentUser">The current user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool SetUserAdministratorPrivileges(User currentUser)
        {
            return Model.User?.Equals(currentUser) ?? false;
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
        protected override void OnModelChanged(Code model)
        {
        }

        /// <summary>
        /// Edits this instance.
        /// </summary>
        public void Edit()
        {
            NavigationService.Navigate(typeof(CodeSettingsPage), Model, $"{Model.Name} Settings");
        }

        /// <summary>
        /// upload comment as an asynchronous operation.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <returns>Task.</returns>
        public async Task UploadCommentAsync(Editor editor)
        {
            if (CurrentUser == null)
            {
                await NotificationService.DisplayErrorMessage("You are not logged in!");
                return;
            }

            if (editor == null || string.IsNullOrWhiteSpace(editor.Rtf))
            {
                await NotificationService.DisplayErrorMessage("Can't post empty comment!");
                return;
            }

            NavigationService.Lock();

            var comment = new Comment(Model.CommentSection, CurrentUser, editor.Rtf);

            if (!await RestApiService<Comment>.Update(comment, comment.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong when posting your comment.");
                NavigationService.Unlock();
                return;
            }

            Model.Reply(CurrentUser, comment);

            if (!await RestApiService<Code>.Update(Model, Model.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong when posting your comment.");
                NavigationService.Unlock();
                return;
            }

            editor.Clear();
            NavigationService.Unlock();
        }
    }
}
