using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class UserSettingsPage : Page
    {
        private UserSettingsViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            User user;

            switch (e.Parameter)
            {
                case User _user:
                    user = _user;
                    break;
                case Guid guid:
                    user = await RestApiService<User>.Get(guid);
                    break;
                case IEntity entity:
                    user = await RestApiService<User>.Get(entity.Uid);
                    break;
                default:
                    await NotificationService.DisplayErrorMessage("Developer error.");
                    throw new InvalidOperationException();
            }

            if (user == null)
            {
                await NotificationService.DisplayErrorMessage("This user does not exist.");
                NavigationService.GoBack();
            }

            ViewModel = new UserSettingsViewModel(user);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle($"{ViewModel.Model?.Name} - Settings");
        }
    }
}
