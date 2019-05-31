// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="CodeBannerManagerDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class CodeBannerManagerDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    public sealed partial class CodeBannerManagerDialog
    {
        /// <summary>
        /// Gets the image manager.
        /// </summary>
        /// <value>The image manager.</value>
        private ImageManager<CodeBanner> ImageManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBannerManagerDialog"/> class.
        /// </summary>
        /// <param name="bannerCollection">The banner collection.</param>
        /// <param name="dialogTitle">The dialog title.</param>
        public CodeBannerManagerDialog(ICollection<CodeBanner> bannerCollection, string dialogTitle)
        {
            ImageManager = new ImageManager<CodeBanner>(bannerCollection);
            InitializeComponent();
            UpdateSelectedItemsCounter();
            Title = dialogTitle;
        }

        /// <summary>
        /// Contents the dialog secondary button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ContentDialogButtonClickEventArgs"/> instance containing the event data.</param>
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ImageGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ImageGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedItemsCounter();
        }

        /// <summary>
        /// Updates the selected items counter.
        /// </summary>
        private void UpdateSelectedItemsCounter()
        {
            SelectedItemsText.Text = $"{ImageGridView.SelectedItems.Count}";
            SelectedItemsText.Text += ImageGridView.SelectedItems.Count > 0
                ? " items selected"
                : " item selected";
        }
    }
}
