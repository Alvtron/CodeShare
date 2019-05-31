// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="ColorConverter.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CodeShare.Uwp.Converters
{
    /// <summary>
    /// Class ColorConverter.
    /// Implements the <see cref="Windows.UI.Xaml.Data.IValueConverter" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class ColorConverter : IValueConverter
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
            if (value is Model.Color color)
            {
                return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
            }

            return (SolidColorBrush)Application.Current.Resources["CodeShareColor"];
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>System.Object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case Color color:
                    return new Model.Color(color.R, color.G, color.B, color.A);
                case SolidColorBrush brush:
                    return new Model.Color(brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A);
            }

            return null;
        }
    }
}
