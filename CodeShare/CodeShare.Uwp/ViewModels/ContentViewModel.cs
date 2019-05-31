// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 05-22-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="ContentViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using CodeShare.RestApi;
using CodeShare.Utilities;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class ContentViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.EntityViewModel{TContent}" />
    /// </summary>
    /// <typeparam name="TContent">The type of the t content.</typeparam>
    /// <seealso cref="CodeShare.Uwp.ViewModels.EntityViewModel{TContent}" />
    public abstract class ContentViewModel<TContent> : EntityViewModel<TContent> where TContent : class, IContent, new()
    {
        /// <summary>
        /// The report command
        /// </summary>
        private RelayCommand _reportCommand;
        /// <summary>
        /// Gets the report command.
        /// </summary>
        /// <value>The report command.</value>
        public ICommand ReportCommand => _reportCommand = _reportCommand ?? new RelayCommand(async param => await ReportAsync());

        /// <summary>
        /// The view video command
        /// </summary>
        private RelayCommand<Video> _viewVideoCommand;
        /// <summary>
        /// Gets the view video command.
        /// </summary>
        /// <value>The view video command.</value>
        public ICommand ViewVideoCommand => _viewVideoCommand = _viewVideoCommand ?? new RelayCommand<Video>(ViewVideo);

        /// <summary>
        /// The view image command
        /// </summary>
        private RelayCommand<WebFile> _viewImageCommand;
        /// <summary>
        /// Gets the view image command.
        /// </summary>
        /// <value>The view image command.</value>
        public ICommand ViewImageCommand => _viewImageCommand = _viewImageCommand ?? new RelayCommand<WebFile>(ViewImage);

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewModel{TContent}"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        protected ContentViewModel(TContent model)
            : base(model)
        {
        }

        /// <summary>
        /// Views the video.
        /// </summary>
        /// <param name="video">The video.</param>
        private static void ViewVideo(Video video)
        {
            NavigationService.Navigate(typeof(MediaPage), video, video.Title);
        }

        /// <summary>
        /// Views the image.
        /// </summary>
        /// <param name="image">The image.</param>
        private static void ViewImage(WebFile image)
        {
            NavigationService.Navigate(typeof(MediaPage), image, image.Path);
        }

        /// <summary>
        /// increment views as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task IncrementViewsAsync()
        {
            Model.Views++;

            if (!await RestApiService<TContent>.Update(Model, nameof(Model.Views)))
            {
                Logger.WriteLine($"Failed to increment view counter for code {Model.Uid}.");
                Model.Views--;
            }
        }

        /// <summary>
        /// report as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ReportAsync()
        {
            if (Model == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                return;
            }

            var reportDialog = new ReportDialog(Model?.Name);

            var dialogResult = await reportDialog.ShowAsync();

            if (dialogResult != ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(reportDialog.Message))
            {
                await NotificationService.DisplayErrorMessage($"Please provide a reason for why you want to report '{Model?.Name}'.");
                return;
            }

            if (!await RestApiService<Report>.Add(new Report(Model, reportDialog.Message)))
            {
                await NotificationService.DisplayErrorMessage("We were unable to register that report. Sorry about that. Please try again later.");
                return;
            }

            await NotificationService.DisplayThankYouMessage("Thanks for contributing to a better community for everyone! You rock!");
        }
    }
}