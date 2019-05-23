﻿using CodeShare.Model;
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
    public class CodeSettingsViewModel : ModelSettingsViewModel<Code>
    {
        private RelayCommand _uploadCodeFilesCommand;
        public ICommand UploadCodeFilesCommand => _uploadCodeFilesCommand = _uploadCodeFilesCommand ?? new RelayCommand(async param => await UploadFiles());

        private RelayCommand _uploadScreenshotsCommand;
        public ICommand UploadScreenshotsCommand => _uploadScreenshotsCommand = _uploadScreenshotsCommand ?? new RelayCommand(async param => await UploadScreenshotsAsync());

        private RelayCommand<CodeScreenshot> _deleteScreenshotCommand;
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<CodeScreenshot>(screenshot => DeleteScreenshot(screenshot));

        private RelayCommand _uploadVideoCommand;
        public ICommand UploadVideoCommand => _uploadVideoCommand = _uploadVideoCommand ?? new RelayCommand(async param => await UploadVideoAsync());

        private RelayCommand<CodeVideo> _deleteVideoCommand;
        public ICommand DeleteVideoCommand => _deleteVideoCommand = _deleteVideoCommand ?? new RelayCommand<CodeVideo>(param => DeleteVideo(param));

        public CodeSettingsViewModel(Code code)
            : base(code)
        {
            
        }

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
                    Model.AddFile(codeFile, AuthService.CurrentUser);
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue;
                }
            }

            IsModelChanged = true;
        }

        public async Task UploadScreenshotsAsync()
        {
            var imageFiles = await StorageUtilities.PickMultipleImages();
            if (imageFiles == null || imageFiles.Count == 0) return;

            NavigationService.Lock();

            foreach (var imageFile in imageFiles)
            {
                Model?.AddScreenshot(AuthService.CurrentUser, await ImageUtilities.CreateNewImageAsync<CodeScreenshot>(imageFile));
            }

            NavigationService.Unlock();

            IsModelChanged = true;
        }

        public void DeleteScreenshot(CodeScreenshot screenshot)
        {
            if (screenshot == null)
            {
                return;
            }

            Model.Screenshots.Remove(screenshot);
        }

        public async Task UploadVideoAsync()
        {
            var dialog = new AddVideoDialog();
            NavigationService.Lock();

            if (await dialog.ShowAsync() != ContentDialogResult.Secondary || dialog.Video == null)
            {
                await NotificationService.DisplayErrorMessage("Video was invalid");
                NavigationService.Unlock();
                return;
            }

            Model?.AddVideo(AuthService.CurrentUser, dialog.Video as CodeVideo);
            NavigationService.Unlock();

            IsModelChanged = true;
        }

        public void DeleteVideo(CodeVideo video)
        {
            if (video == null)
            {
                return;
            }

            Model?.Videos.Remove(video);
        }
    }
}
