// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="ImageCropper.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using System;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace CodeShare.Uwp.Utilities
{
    /// <summary>
    /// Class ImageCropper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImageCropper<T> where T : WebImage
    {
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>The image.</value>
        public T Image { get; }

        /// <summary>
        /// Gets the crop.
        /// </summary>
        /// <value>The crop.</value>
        public Crop Crop { get; }

        /// <summary>
        /// The handle size
        /// </summary>
        public readonly double HandleSize;

        /// <summary>
        /// Gets or sets a value indicating whether [pointer is busy].
        /// </summary>
        /// <value><c>true</c> if [pointer is busy]; otherwise, <c>false</c>.</value>
        private bool PointerIsBusy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCropper{T}"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public ImageCropper(T image)
        {
            Image = image;
            Crop = Image.Crop;
            HandleSize = image.Width * 0.03;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            Image.Crop = Crop;
        }

        /// <summary>
        /// Updates the crop.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        private void UpdateCrop(PointerPoint pointer)
        {
            if (PointerIsBusy || !pointer.Properties.IsLeftButtonPressed)
            {
                return;
            }

            PointerIsBusy = true;

            var x = ConvertXToCoordinate(pointer.Position.X);
            var y = ConvertXToCoordinate(pointer.Position.Y);

            (Crop.Width, Crop.Height) = CreateNewCropSize(x, y);
            PointerIsBusy = false;
        }

        /// <summary>
        /// Creates the new size of the crop.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Tuple&lt;System.Int32, System.Int32&gt;.</returns>
        private Tuple<int, int> CreateNewCropSize(int x, int y)
        {
            var newWidth = Math.Abs(x) * 2;
            var newHeight = Math.Abs(y) * 2;
            
            var ratio = Image.Crop.AspectRatio;

            var suggestedHeight = newWidth / ratio;
            var suggestedWidth = newHeight * ratio;

            return suggestedHeight <= newHeight
                ? Tuple.Create(newWidth, (int)suggestedHeight)
                : Tuple.Create((int)suggestedWidth, newHeight);
        }

        /// <summary>
        /// Converts the x to coordinate.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>System.Int32.</returns>
        private int ConvertXToCoordinate(double x)
        {
            return -(int)(Image.Width / 2.0 - x);
        }

        /// <summary>
        /// Handles the <see cref="E:PointerMoved" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs"/> instance containing the event data.</param>
        public void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!(sender is Grid grid))
            {
                return;
            }
            UpdateCrop(e.GetCurrentPoint(grid));
        }
    }
}
