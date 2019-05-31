// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 02-02-2019
// ***********************************************************************
// <copyright file="NotificationService.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Uwp.Dialogs;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Services
{
    /// <summary>
    /// Class NotificationService.
    /// </summary>
    public static class NotificationService
    {
        /// <summary>
        /// Displays the general message.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <param name="primaryButtonText">The primary button text.</param>
        /// <param name="secondaryButtonText">The secondary button text.</param>
        /// <param name="closeButtonText">The close button text.</param>
        /// <returns>Task&lt;ContentDialogResult&gt;.</returns>
        public static async Task<ContentDialogResult> DisplayGeneralMessage(string title, string message, string primaryButtonText = "", string secondaryButtonText = "", string closeButtonText = "")
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = primaryButtonText,
                SecondaryButtonText = secondaryButtonText,
                CloseButtonText = closeButtonText
            };

            return await dialog.ShowAsync();
        }

        /// <summary>
        /// Displays the loading dialog.
        /// </summary>
        /// <param name="timeInSeconds">The time in seconds.</param>
        /// <returns>Task&lt;ContentDialogResult&gt;.</returns>
        public static async Task<ContentDialogResult> DisplayLoadingDialog(int timeInSeconds)
        {
            var dialog = new LoadingDialog(timeInSeconds);
            return await dialog.ShowAsync();
        }

        /// <summary>
        /// Creates the and display loading dialog.
        /// </summary>
        /// <returns>Task&lt;LoadingDialog&gt;.</returns>
        public static async Task<LoadingDialog> CreateAndDisplayLoadingDialog()
        {
            var dialog = new LoadingDialog();
            await dialog.ShowAsync();
            return dialog;
        }

        /// <summary>
        /// Displays the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;ContentDialogResult&gt;.</returns>
        public static async Task<ContentDialogResult> DisplayErrorMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Woah, there!",
                Content = message,
                PrimaryButtonText = "OK"
            };

            return await dialog.ShowAsync();
        }

        /// <summary>
        /// Displays the thank you message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;ContentDialogResult&gt;.</returns>
        public static async Task<ContentDialogResult> DisplayThankYouMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Thank you!",
                Content = message,
                PrimaryButtonText = "OK"
            };

            return await dialog.ShowAsync();
        }
    }
}
