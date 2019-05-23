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
    public class QuestionSettingsViewModel : ModelSettingsViewModel<Question>
    {
        private RelayCommand _uploadScreenshotsCommand;
        public ICommand UploadScreenshotsCommand => _uploadScreenshotsCommand = _uploadScreenshotsCommand ?? new RelayCommand(async param => await UploadScreenshotsAsync());

        private RelayCommand<QuestionScreenshot> _deleteScreenshotCommand;
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<QuestionScreenshot>(screenshot => DeleteScreenshot(screenshot));

        private RelayCommand _uploadVideoCommand;
        public ICommand UploadVideoCommand => _uploadVideoCommand = _uploadVideoCommand ?? new RelayCommand(async param => await UploadVideoAsync());

        private RelayCommand<QuestionVideo> _deleteVideoCommand;
        public ICommand DeleteVideoCommand => _deleteVideoCommand = _deleteVideoCommand ?? new RelayCommand<QuestionVideo>(param => DeleteVideo(param));

        public QuestionSettingsViewModel(Question question)
            : base(question)
        {
            
        }

        public async Task UploadScreenshotsAsync()
        {
            var imageFiles = await StorageUtilities.PickMultipleImages();
            if (imageFiles == null || imageFiles.Count == 0) return;

            NavigationService.Lock();

            foreach (var imageFile in imageFiles)
            {
                Model?.AddScreenshot(AuthService.CurrentUser, await ImageUtilities.CreateNewImageAsync<QuestionScreenshot>(imageFile));
            }

            NavigationService.Unlock();

            IsModelChanged = true;
        }

        public void DeleteScreenshot(QuestionScreenshot screenshot)
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

            Model?.AddVideo(AuthService.CurrentUser, dialog.Video as QuestionVideo);
            NavigationService.Unlock();

            IsModelChanged = true;
        }

        public void DeleteVideo(QuestionVideo video)
        {
            if (video == null)
            {
                return;
            }

            Model?.Videos.Remove(video);
        }
    }
}
