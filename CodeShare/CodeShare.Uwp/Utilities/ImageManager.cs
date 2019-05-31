// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="ImageManager.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeShare.Uwp.Utilities
{
    /// <summary>
    /// Class ImageManager.
    /// Implements the <see cref="CodeShare.Model.ObservableObject" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="CodeShare.Model.ObservableObject" />
    public class ImageManager<T> : ObservableObject where T : class, IWebFile, new()
    {
        /// <summary>
        /// The download images command
        /// </summary>
        private RelayCommand<IList<object>> _downloadImagesCommand;
        /// <summary>
        /// Gets the download images command.
        /// </summary>
        /// <value>The download images command.</value>
        public ICommand DownloadImagesCommand => _downloadImagesCommand = _downloadImagesCommand ?? new RelayCommand<IList<object>>(async images => await DownloadImagesAsync(images));

        /// <summary>
        /// The delete images command
        /// </summary>
        private RelayCommand<IList<object>> _deleteImagesCommand;
        /// <summary>
        /// Gets the delete images command.
        /// </summary>
        /// <value>The delete images command.</value>
        public ICommand DeleteImagesCommand => _deleteImagesCommand = _deleteImagesCommand ?? new RelayCommand<IList<object>>(DeleteImagesAsync);

        /// <summary>
        /// Gets the image collection.
        /// </summary>
        /// <value>The image collection.</value>
        public ICollection<T> ImageCollection { get; }

        /// <summary>
        /// The is busy
        /// </summary>
        private bool _isBusy;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetField(ref _isBusy, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageManager{T}"/> class.
        /// </summary>
        /// <param name="imageCollection">The image collection.</param>
        public ImageManager(ICollection<T> imageCollection)
        {
            ImageCollection = imageCollection;
        }

        /// <summary>
        /// download images as an asynchronous operation.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>Task.</returns>
        private async Task DownloadImagesAsync(IList<object> objects)
        {
            if (objects == null)
            {
                return;
            }

            IsBusy = true;

            var images = objects.Cast<T>();

            var folder = await StorageUtilities.PickFolderDestination();

            if (folder == null)
            {
                return;
            }

            foreach (var image in images)
            {
                await StorageUtilities.SaveWebFileToStorageFolder(image, folder);
            }

            IsBusy = false;
        }

        /// <summary>
        /// Deletes the images asynchronous.
        /// </summary>
        /// <param name="objects">The objects.</param>
        private void DeleteImagesAsync(IList<object> objects)
        {
            if (objects == null)
            {
                return;
            }

            IsBusy = true;

            var images = objects.Cast<T>();

            foreach (var image in images)
            {
                ImageCollection.Remove(image);
                image.Delete();
            }

            IsBusy = false;
        }
    }
}
