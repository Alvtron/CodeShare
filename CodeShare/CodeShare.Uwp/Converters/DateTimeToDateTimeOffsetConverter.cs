// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 12-13-2018
// ***********************************************************************
// <copyright file="DateTimeToDateTimeOffsetConverter.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Windows.UI.Xaml.Data;

namespace CodeShare.Uwp.Converters
{
    /// <summary>
    /// Class DateTimeToDateTimeOffsetConverter.
    /// Implements the <see cref="Windows.UI.Xaml.Data.IValueConverter" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
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
                    return new DateTimeOffset(DateTime.Now);
                case DateTime dateTime:
                    return new DateTimeOffset(dateTime);
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
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case null:
                    return null;
                case DateTimeOffset dateTimeOffset:
                    return dateTimeOffset.LocalDateTime;
            }

            return null;
        }
    }
}