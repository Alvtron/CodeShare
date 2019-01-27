using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Storage;

namespace CodeShare.Uwp.ViewModels
{
    public class CodeSettingsViewModel : EditorViewModel
    {
        private Code _code = new Code();
        public Code Code
        {
            get => _code;
            set => SetField(ref _code, value);
        }

        private RelayCommand _uploadCodeCommand;
        public ICommand UploadCodeCommand => _uploadCodeCommand = _uploadCodeCommand ?? new RelayCommand(async param => await UploadFiles());

        public CodeSettingsViewModel(Code code)
        {
            Code = code;
            Code.PropertyChanged += (s, e) => { Changed = true; };
        }

        private async Task UploadFiles()
        {
            var files = await StorageUtilities.PickMultipleFiles();

            if (files == null || files.Count == 0)
                return;

            foreach (var file in files)
            {
                Code.AddFile(new File(await FileIO.ReadTextAsync(file), file.DisplayName, file.FileType), AuthService.CurrentUser);
            }
            
            Changed = true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (Code == null)
            {
                await NotificationService.DisplayErrorMessage("Some changes are not valid.");
                return false;
            }

            NavigationService.Lock();

            if (await RestApiService<Code>.Update(Code, Code.Uid) == false)
            {
                await NotificationService.DisplayErrorMessage("An error occurred during the upload. No changes where made.");
                NavigationService.Unlock();
                return false;
            }

            NavigationService.Unlock();
            Changed = false;
            return true;
        }

        public override async Task UploadImagesAsync()
        {
            var imageFiles = await StorageUtilities.PickMultipleImages();
            if (imageFiles == null || imageFiles.Count == 0) return;

            NavigationService.Lock();

            foreach (var imageFile in imageFiles)
            {
                Code?.AddScreenshot(await ImageUtilities.CreateNewImageAsync<Screenshot>(imageFile), AuthService.CurrentUser);
            }

            NavigationService.Unlock();
            Changed = true;
        }

        public override async Task UploadVideoAsync()
        {
            var dialog = new AddVideoDialog();
            NavigationService.Lock();

            if (await dialog.ShowAsync() != ContentDialogResult.Secondary || dialog.VideoData == null)
            {
                await NotificationService.DisplayErrorMessage("Video was invalid");
                NavigationService.Unlock();
                return;
            }

            Code?.AddVideo(AuthService.CurrentUser, dialog.VideoData);
            NavigationService.Unlock();
            Changed = true;
        }

        public override void DeleteVideo(Video video)
        {
            Code?.Videos.Remove(video);
            Changed = true;
        }

        public override void DeleteImage(WebFile screenshot)
        {
            throw new NotImplementedException();
        }

        public override async Task SaveAsync()
        {
            NavigationService.Lock();

            if (await RestApiService<Code>.Update(Code, Code.Uid))
            {
                Changed = false;
            }
            NavigationService.Unlock();
    }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
