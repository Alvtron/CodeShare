// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="MainPage.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Utilities;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Views
{
    /// <summary>
    /// Class MainPage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MainPage
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        private MainViewModel ViewModel { get; set; } = new MainViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the PageRoot control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void PageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeApplication();
        }

        /// <summary>
        /// Initializes the application.
        /// </summary>
        /// <returns>Task.</returns>
        private static async Task InitializeApplication()
        {
            NavigationService.Lock();
            await AuthService.SignInAsync();
            AppSettings.PrintSettings();
            NavigationService.Unlock();
        }

        /// <summary>
        /// Handles the Loaded event of the NavigationView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.Initialize(ContentFrame, NavigationView, ProgressRing);
            NavigationService.Navigate(typeof(HomePage), null, "Home");

            if (NavigationView.IsPaneOpen)
            {
                NavigationViewItemSearchIcon.Visibility = Visibility.Collapsed;
                NavigationViewItemSearchBox.Visibility = Visibility.Visible;
            }
            else
            {
                NavigationViewItemSearchBox.Visibility = Visibility.Collapsed;
                NavigationViewItemSearchIcon.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Navigations the view back requested.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="NavigationViewBackRequestedEventArgs"/> instance containing the event data.</param>
        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        /// <summary>
        /// Navigations the view invoked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="NavigationViewItemInvokedEventArgs"/> instance containing the event data.</param>
        private async void NavigationView_Invoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (!(args.InvokedItem is string viewItemContent))
            {
                Logger.WriteLine("Invoked navigation item has no content. No navigation can be done.");
                return;
            }

            if (viewItemContent == "SearchIcon")
            {
                NavigationView.IsPaneOpen = true;
                SearchBox.Focus(FocusState.Keyboard);
            }

            await NavigationService.Navigate(viewItemContent);
        }

        /// <summary>
        /// Asbs the text changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs"/> instance containing the event data.</param>
        private async void ASB_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                await ViewModel.Search(sender.Text.ToLower());
            }
        }

        /// <summary>
        /// Asbs the query submitted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        private void ASB_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.IsSuggestionListOpen = false;
            ViewModel.SubmitSearch(args);
        }

        /// <summary>
        /// Navigations the view pane opening.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        private void NavigationView_PaneOpening(NavigationView sender, object args)
        {
            NavigationViewItemSearchIcon.Visibility = Visibility.Collapsed;
            NavigationViewItemSearchBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Navigations the view pane closing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        private void NavigationView_PaneClosing(NavigationView sender, object args)
        {
            NavigationViewItemSearchBox.Visibility = Visibility.Collapsed;
            NavigationViewItemSearchIcon.Visibility = Visibility.Visible;
        }
    }
}
