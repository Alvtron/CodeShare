// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="UserPage.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    /// <summary>
    /// Class UserPage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class UserPage
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        private UserViewModel ViewModel { get; set; }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        /// <exception cref="InvalidOperationException"></exception>
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
            await ViewModel.IncrementViewsAsync();
            NavigationService.SetHeaderTitle($"{ViewModel.Model?.Name}'s profile");
        }
    }
}