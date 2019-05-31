// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="UploadImageDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using CodeShare.Uwp.Services;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class UploadImageDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class UploadImageDialog
    {
        /// <summary>
        /// Gets or sets the image file.
        /// </summary>
        /// <value>The image file.</value>
        private StorageFile ImageFile { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadImageDialog"/> class.
        /// </summary>
        public UploadImageDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the DragOver event of the UploadButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void UploadButton_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        /// <summary>
        /// Handles the Drop event of the UploadButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private async void UploadButton_Drop(object sender, DragEventArgs e)
        {
            if (!e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                return;
            }

            var items = await e.DataView.GetStorageItemsAsync();

            if (!(items.FirstOrDefault() is StorageFile imageFile))
            {
                return;
            }
            
            ImageFile = imageFile;

            Hide();
        }

        /// <summary>
        /// Handles the Click event of the UploadButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var imageFile = await StorageUtilities.PickSingleImage();

            if (imageFile == null)
            {
                return;
            }

            ImageFile = imageFile;

            Hide();
        }

        /// <summary>
        /// Creates the image from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Task&lt;T&gt;.</returns>
        public async Task<T> CreateImageFromFile<T>() where T : WebImage
        {
            NavigationService.Lock();
            var image = await ImageUtilities.CreateNewImageAsync<T>(ImageFile);
            NavigationService.Unlock();
            return image;
        }
    }
}
