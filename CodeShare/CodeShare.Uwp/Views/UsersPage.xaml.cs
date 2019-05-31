// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="UsersPage.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    /// <summary>
    /// Class UsersPage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class UsersPage
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        private UsersViewModel ViewModel { get; set; }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();
            var users = await RestApiService<User>.Get();

            if (users == null)
            {
                await NotificationService.DisplayErrorMessage("Could not retrieve users from database.");
                NavigationService.GoBack();
                return;
            }

            ViewModel = new UsersViewModel(users);
            InitializeComponent();
            NavigationService.Unlock();
            NavigationService.SetHeaderTitle("Users");
        }

        /// <summary>
        /// Handles the ItemClick event of the UserList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private async void UserList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is User user))
            {
                await NotificationService.DisplayErrorMessage("There seems to be something wrong with that user. Sorry about that.");
                return;
            }

            NavigationService.Navigate(typeof(UserPage), user, $"{user.Name}'s Page");
        }
    }
}
