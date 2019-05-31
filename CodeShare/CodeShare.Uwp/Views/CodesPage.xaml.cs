// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodesPage.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    /// <summary>
    /// Class CodesPage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodesPage
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        private CodesViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            var codes = await RestApiService<Code>.Get();

            if (codes == null)
            {
                await NotificationService.DisplayErrorMessage("Could not retrieve codes from database.");
                NavigationService.GoBack();
                return;
            }

            ViewModel = new CodesViewModel(codes);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle("Codes");
        }
    }
}
