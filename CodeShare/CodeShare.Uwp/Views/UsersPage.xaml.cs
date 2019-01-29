﻿using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class UsersPage : Page
    {
        private UsersViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            var users = await RestApiService<User>.Get();

            if (users == null)
            {
                await NotificationService.DisplayErrorMessage("Could not retrieve users from database.");
                NavigationService.GoBack();
            }

            ViewModel = new UsersViewModel(users);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle("Users");
        }

        private async void UserList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is User user) || !user.Valid)
            {
                await NotificationService.DisplayErrorMessage("There seems to be something wrong with that user. Sorry about that.");
                return;
            }

            NavigationService.Navigate(typeof(UserPage), user, $"{user.Name}'s Page");
        }
    }
}
