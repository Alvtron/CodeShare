// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeSettingsViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Dialogs;
using Windows.Storage;
using CodeShare.Utilities;
using CodeShare.RestApi;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class CodeSettingsViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ModelSettingsViewModel{CodeShare.Model.Code}" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ModelSettingsViewModel{CodeShare.Model.Code}" />
    public class CodeSettingsViewModel : ModelSettingsViewModel<Code>
    {
        /// <summary>
        /// The upload code files command
        /// </summary>
        private RelayCommand _uploadCodeFilesCommand;
        /// <summary>
        /// Gets the upload code files command.
        /// </summary>
        /// <value>The upload code files command.</value>
        public ICommand UploadCodeFilesCommand => _uploadCodeFilesCommand = _uploadCodeFilesCommand ?? new RelayCommand(async param => await UploadFiles());

        /// <summary>
        /// The upload screenshots command
        /// </summary>
        private RelayCommand _uploadScreenshotsCommand;
        /// <summary>
        /// Gets the upload screenshots command.
        /// </summary>
        /// <value>The upload screenshots command.</value>
        public ICommand UploadScreenshotsCommand => _uploadScreenshotsCommand = _uploadScreenshotsCommand ?? new RelayCommand(async param => await UploadScreenshotsAsync());

        /// <summary>
        /// The delete screenshot command
        /// </summary>
        private RelayCommand<CodeScreenshot> _deleteScreenshotCommand;
        /// <summary>
        /// Gets the delete screenshot command.
        /// </summary>
        /// <value>The delete screenshot command.</value>
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<CodeScreenshot>(DeleteScreenshot);

        /// <summary>
        /// The upload video command
        /// </summary>
        private RelayCommand _uploadVideoCommand;
        /// <summary>
        /// Gets the upload video command.
        /// </summary>
        /// <value>The upload video command.</value>
        public ICommand UploadVideoCommand => _uploadVideoCommand = _uploadVideoCommand ?? new RelayCommand(async param => await UploadVideoAsync());

        /// <summary>
        /// The delete video command
        /// </summary>
        private RelayCommand<CodeVideo> _deleteVideoCommand;
        /// <summary>
        /// Gets the delete video command.
        /// </summary>
        /// <value>The delete video command.</value>
        public ICommand DeleteVideoCommand => _deleteVideoCommand = _deleteVideoCommand ?? new RelayCommand<CodeVideo>(DeleteVideo);

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeSettingsViewModel"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public CodeSettingsViewModel(Code code)
            : base(code)
        {
            
        }

        /// <summary>
        /// Uploads the files.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task UploadFiles()
        {
            var files = await StorageUtilities.PickMultipleFiles();

            if (files == null || files.Count == 0)
                return;

            var codeLanguages = await RestApiService<CodeLanguage>.Get();

            foreach (var file in files)
            {
                var fileType = file.FileType.ToLower();
                var codeLanguage = codeLanguages.FirstOrDefault(cl => cl.Extension.ToLower().Equals(fileType));

                if (codeLanguage == null)
                {
                    Logger.WriteLine($"Rejected file {file.Name}. Extension is not supported.");
                    continue;
                }

                try
                {
                    var codeFile = new CodeFile(codeLanguage, await FileIO.ReadTextAsync(file), file.DisplayName, file.FileType);
                    Model.AddFile(codeFile, CurrentUser);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            IsModelChanged = true;
        }

        /// <summary>
        /// upload screenshots as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task UploadScreenshotsAsync()
        {
            var imageFiles = await StorageUtilities.PickMultipleImages();
            if (imageFiles == null || imageFiles.Count == 0) return;

            NavigationService.Lock();

            foreach (var imageFile in imageFiles)
            {
                Model?.AddScreenshot(CurrentUser, await ImageUtilities.CreateNewImageAsync<CodeScreenshot>(imageFile));
            }

            NavigationService.Unlock();

            IsModelChanged = true;
        }

        /// <summary>
        /// Deletes the screenshot.
        /// </summary>
        /// <param name="screenshot">The screenshot.</param>
        public void DeleteScreenshot(CodeScreenshot screenshot)
        {
            if (screenshot == null)
            {
                return;
            }

            Model.Screenshots.Remove(screenshot);
        }

        /// <summary>
        /// upload video as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task UploadVideoAsync()
        {
            var dialog = new AddCodeVideoDialog();
            NavigationService.Lock();

            if (await dialog.ShowAsync() != ContentDialogResult.Primary || dialog.Video == null)
            {
                await NotificationService.DisplayErrorMessage("Video was invalid");
                NavigationService.Unlock();
                return;
            }

            Model?.AddVideo(CurrentUser, dialog.Video);
            NavigationService.Unlock();

            IsModelChanged = true;
        }

        /// <summary>
        /// Deletes the video.
        /// </summary>
        /// <param name="video">The video.</param>
        public void DeleteVideo(CodeVideo video)
        {
            if (video == null)
            {
                return;
            }

            Model?.Videos.Remove(video);
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
        /// on saving model as an asynchronous operation.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        protected override async Task OnSavingModelAsync(Code model)
        {
            
        }
    }
}
