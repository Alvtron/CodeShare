// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodeSettingsPage.xaml.cs" company="CodeShare">
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
    /// Class CodeSettingsPage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeSettingsPage
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        private CodeSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        /// <exception cref="InvalidOperationException"></exception>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            Code code;

            switch (e.Parameter)
            {
                case Code _code:
                    code = _code;
                    break;
                case Guid guid:
                    code = await RestApiService<Code>.Get(guid);
                    break;
                case IEntity entity:
                    code = await RestApiService<Code>.Get(entity.Uid);
                    break;
                default:
                    await NotificationService.DisplayErrorMessage("Developer error.");
                    throw new InvalidOperationException();
            }

            if (code == null)
            {
                await NotificationService.DisplayErrorMessage("This code does not exist.");
                NavigationService.GoBack();
                return;
            }

            ViewModel = new CodeSettingsViewModel(code);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle($"{ViewModel.Model?.Name} - Settings");
        }
    }
}
