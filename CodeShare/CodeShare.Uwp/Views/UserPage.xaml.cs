using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    public sealed partial class UserPage : Page
    {
        private UserViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            User user;

            switch (e.Parameter)
            {
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

            ViewModel = new UserViewModel(user);

            InitializeComponent();

            NavigationService.Unlock();

            ViewModel.Model.Views++;

            if (!await RestApiService<User>.Update(ViewModel.Model, ViewModel.Model.Uid))
            {
                Logger.WriteLine($"Failed to increment view counter for user {ViewModel.Model.Uid}.");
            }

            NavigationService.SetHeaderTitle($"{ViewModel.Model?.Name}'s profile");
        }
    }
}