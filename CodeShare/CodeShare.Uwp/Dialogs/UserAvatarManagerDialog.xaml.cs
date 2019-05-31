// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="UserAvatarManagerDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class UserAvatarManagerDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    public sealed partial class UserAvatarManagerDialog
    {
        /// <summary>
        /// Gets the image manager.
        /// </summary>
        /// <value>The image manager.</value>
        private ImageManager<UserAvatar> ImageManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAvatarManagerDialog"/> class.
        /// </summary>
        /// <param name="avatarCollection">The avatar collection.</param>
        /// <param name="dialogTitle">The dialog title.</param>
        public UserAvatarManagerDialog(ICollection<UserAvatar> avatarCollection, string dialogTitle)
        {
            ImageManager = new ImageManager<UserAvatar>(avatarCollection);
            InitializeComponent();
            UpdateSelectedItemsCounter();
            Title = dialogTitle;
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
