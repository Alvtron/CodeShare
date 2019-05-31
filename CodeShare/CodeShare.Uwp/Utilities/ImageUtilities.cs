// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="ImageUtilities.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CodeShare.Uwp.Utilities
{
    /// <summary>
    /// Class ImageUtilities.
    /// </summary>
    public class ImageUtilities
    {
        /// <summary>
        /// create new image as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storageFile">The storage file.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        public static async Task<T> CreateNewImageAsync<T>(StorageFile storageFile) where T : WebImage
        {
            var imageInBytes = await StorageUtilities.ConvertStorageFileToByteArrayAsync(storageFile);

            using (var stream = await storageFile.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);

                return (T)Activator.CreateInstance(typeof(T), new object[]
                {
                    (int)decoder.PixelWidth,
                    (int)decoder.PixelHeight,
                    imageInBytes,
                    storageFile.FileType
                });
            }
        }

        /// <summary>
        /// Webs the image to cropped image.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webImage">The web image.</param>
        /// <returns>Task&lt;ImageSource&gt;.</returns>
        public static async Task<ImageSource> WebImageToCroppedImage<T>(T webImage) where T : WebImage, ICroppableImage
        {
            // Convert start point and size to unsigned integer. 
            uint startPointX = (uint)webImage.Crop.X;
            uint startPointY = (uint)webImage.Crop.Y;
            uint height = (uint)webImage.Crop.Height;
            uint width = (uint)webImage.Crop.Width;

            var bitmap = new BitmapImage(webImage.Url);
            var randomAccessStreamReference = RandomAccessStreamReference.CreateFromUri(bitmap.UriSource);

            using (var stream = await randomAccessStreamReference.OpenReadAsync())
            {
                // Create a decoder from the stream. With the decoder, we can get the properties of the image. 
                var decoder = await BitmapDecoder.CreateAsync(stream);

                // Create the bitmap bounds
                var bitmapBounds = new BitmapBounds
                {
                    X = startPointX,
                    Y = startPointY,
                    Height = height,
                    Width = width
                };

                // Create cropping BitmapTransform. 
                var bitmapTransform = new BitmapTransform
                {
                    Bounds = bitmapBounds,
                    ScaledWidth = width,
                    ScaledHeight = height
                };

                // Get the cropped pixels within the bounds of transform. 
                PixelDataProvider pixelDataProvider = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    bitmapTransform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.ColorManageToSRgb);

                byte[] pixels = pixelDataProvider.DetachPixelData();

                // Stream the bytes into a WriteableBitmap 
                WriteableBitmap cropBmp = new WriteableBitmap((int)width, (int)height);
                Stream pixStream = cropBmp.PixelBuffer.AsStream();
                pixStream.Write(pixels, 0, (int)(width * height * 4));

                return cropBmp;
            }
        }
    }
}
