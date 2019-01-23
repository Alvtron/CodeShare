using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    public sealed partial class UserPage : Page
    {
        public UserViewModel ViewModel { get; set; } = new UserViewModel();

        public UserPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            switch (e.Parameter)
            {
                case Guid guid:
                    ViewModel.User = await RestApiService<User>.Get(guid);
                    break;
                case IEntity entity:
                    ViewModel.User = await RestApiService<User>.Get(entity.Uid);
                    break;
                default:
                    NavigationService.GoBack();
                    return;
            }

            NavigationService.Unlock();

            if (ViewModel.User == null)
            {
                await NotificationService.DisplayErrorMessage("This resource pack do not exist.");
                NavigationService.GoBack();
            }

            ViewModel.User.Views++;

            if (!await RestApiService<User>.Update(ViewModel.User, ViewModel.User.Uid))
            {
                Debug.WriteLine($"Failed to increment view counter for code {ViewModel.User.Uid}.");
            }

            NavigationService.SetHeaderTitle($"{ViewModel.User.Name}'s profile");
        }
    }
}