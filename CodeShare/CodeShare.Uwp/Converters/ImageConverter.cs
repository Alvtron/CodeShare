// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="ImageConverter.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Utilities;
using CodeShare.Model;

namespace CodeShare.Uwp.Converters
{
    /// <summary>
    /// Class ImageConverter.
    /// Implements the <see cref="Windows.UI.Xaml.Data.IValueConverter" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class ImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case null:
                    break;
                case byte[] bytes:
                    return StorageUtilities.ConvertByteArrayToBitmapImage(bytes);
                case WebFile file when file.Url != null:
                    return new BitmapImage(file.Url);
                case WebFile file when file.Url == null:
                    return StorageUtilities.ConvertByteArrayToBitmapImage(file.Download());
                case Uri uri:
                    return new BitmapImage(uri);
                case Image image:
                    return image.Source;
            }

            if (parameter is string parameterString && parameterString.ToUpper() == "SHOW_NO_IMAGE")
            {
                return StorageUtilities.ConvertFileToBitmapImage(@"Assets\no_image_available.jpg");
            }

            return null;
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
