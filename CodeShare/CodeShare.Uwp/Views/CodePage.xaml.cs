// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodePage.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    /// <summary>
    /// Class CodePage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodePage
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        private CodeViewModel ViewModel { get; set; }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        /// <exception cref="ArgumentException">Developer error.</exception>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            Code code;

            switch (e.Parameter)
            {
                case Guid guid:
                    code = await RestApiService<Code>.Get(guid);
                    break;
                case IEntity entity:
                    code = await RestApiService<Code>.Get(entity.Uid);
                    break;
                default:
                    Logger.WriteLine("Developer error.");
                    throw new ArgumentException("Developer error.");
            }

            if (code == null)
            {
                await NotificationService.DisplayErrorMessage("This code does not exist.");
                NavigationService.GoBack();
                return;
            }

            ViewModel = new CodeViewModel(code);
            InitializeComponent();
            NavigationService.Unlock();
            await ViewModel.IncrementViewsAsync();
            NavigationService.SetHeaderTitle(ViewModel.Model?.Name);
        }


    }
}
