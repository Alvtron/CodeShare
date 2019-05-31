// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 02-03-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="QuestionSettingsPage.xaml.cs" company="CodeShare">
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
    /// Class QuestionSettingsPage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class QuestionSettingsPage
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        private QuestionSettingsViewModel ViewModel { get; set; }

        /// <summary>
        /// Handles the <see cref="E:NavigatedTo" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NavigationEventArgs"/> instance containing the event data.</param>
        /// <exception cref="InvalidOperationException"></exception>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            Question question;

            switch (e.Parameter)
            {
                case Question q:
                    question = q;
                    break;
                case Guid guid:
                    question = await RestApiService<Question>.Get(guid);
                    break;
                case IEntity entity:
                    question = await RestApiService<Question>.Get(entity.Uid);
                    break;
                default:
                    await NotificationService.DisplayErrorMessage("Developer error.");
                    throw new InvalidOperationException();
            }

            if (question == null)
            {
                await NotificationService.DisplayErrorMessage("This question does not exist.");
                NavigationService.GoBack();
                return;
            }

            ViewModel = new QuestionSettingsViewModel(question);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle($"{ViewModel.Model?.Name} - Settings");
        }
    }
}
