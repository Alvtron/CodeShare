using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.ViewModels
{
    public class UserSettingsViewModel : EditorViewModel
    {
        private User _user;
        public User User
        {
            get => _user;
            private set => SetField(ref _user, value);
        }

        private RelayCommand _befriendCommand;
        public ICommand BefriendCommand => _befriendCommand = _befriendCommand ?? new RelayCommand(async param => await Befriend());

        private RelayCommand _createNewColorCommand;
        public ICommand CreateNewColorCommand => _createNewColorCommand = _createNewColorCommand ?? new RelayCommand(async param => await CreateNewColor());

        public UserSettingsViewModel(User user)
        {
            User = user;
            User.PropertyChanged += (s, e) => { Changed = true; };
        }
        
        public async Task<bool> Befriend()
        {
            if (AuthService.CurrentUser == null || User == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                return false;
            }

            if (AuthService.CurrentUser.Equals(User))
            {
                await NotificationService.DisplayErrorMessage("You can't be friends with yourself!");
                return false;
            }

            AuthService.CurrentUser.AddFriend(User);
            await RestApiService<User>.Update(User, User.Uid);
            await RestApiService<User>.Update(AuthService.CurrentUser, AuthService.CurrentUser.Uid);
            return true;
        }

        public async Task<bool> CreateNewColor()
        {
            var dialog = new ColorPickerDialog(User.Color);

            if (await dialog.ShowAsync() != ContentDialogResult.Secondary)
            {
                return false;
            }

            User.SetColor(dialog.Color.R, dialog.Color.G, dialog.Color.B, dialog.Color.A);
            return true;
        }

        public override async Task SaveAsync()
        {
            NavigationService.Lock();
            if (await RestApiService<User>.Update(User, User.Uid))
            {
                Changed = false;
            }
            NavigationService.Unlock();
        }

        public override void Reset()
        {
            // TODO: Implement resetting
            Changed = false;
        }

        public override async Task UploadVideoAsync()
        {
            var dialog = new AddVideoDialog();

            if (await dialog.ShowAsync() != ContentDialogResult.Secondary || dialog.VideoData == null)
            {
                await NotificationService.DisplayErrorMessage("Video was invalid");
                NavigationService.Unlock();
                return;
            }

            User?.AddVideo(AuthService.CurrentUser, dialog.VideoData);
            Changed = true;
        }

        public override void DeleteImage(WebFile screenshot)
        {
            throw new NotImplementedException();
        }

        public override void DeleteVideo(Video video)
        {
            throw new NotImplementedException();
        }

        public override Task UploadImagesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
