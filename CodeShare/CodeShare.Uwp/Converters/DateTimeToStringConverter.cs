// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="DateTimeToStringConverter.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Windows.UI.Xaml.Data;

namespace CodeShare.Uwp.Converters
{
    /// <summary>
    /// Class DateTimeToStringConverter.
    /// Implements the <see cref="Windows.UI.Xaml.Data.IValueConverter" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class DateTimeToStringConverter : IValueConverter
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
            if (value == null)
            {
                return "";

            }
            if (!(value is DateTime))
            {
                return "DateToStringConverter: Invalid object type supplied.";
            }

            if (!(parameter is string format) || string.IsNullOrWhiteSpace(format))
            {
                format = "MMMMM dd yyyy";
            }

            try
            {
                return DateTime.Now.ToString(format);
            }
            catch (Exception)
            {
#if DEBUG
                return "DateToStringConverter: Invalid format supplied.";
#else
                return "";
#endif
            }
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