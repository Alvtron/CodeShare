﻿// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="DateTimeToDurationString.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;
using Windows.UI.Xaml.Data;

namespace CodeShare.Uwp.Converters
{
    /// <summary>
    /// Class DateTimeToDurationString.
    /// Implements the <see cref="Windows.UI.Xaml.Data.IValueConverter" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class DateTimeToDurationString : IValueConverter
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
            if (!(value is DateTime dateTime))
            {
                return "DateToStringConverter: Invalid object type supplied.";
            }

            var dYears = DateTime.Now.Year - dateTime.Year;
            var dMonths = DateTime.Now.Month - dateTime.Month;
            var timeSpan = DateTime.Now - dateTime;
            var dDays = timeSpan.Days;
            var dHours = timeSpan.Hours;
            var dMinutes = timeSpan.Minutes;
            var dSeconds = timeSpan.Seconds;

            var stringBuilder = new StringBuilder();
            
            if (dYears > 0 && dMonths == 0)
            {
                stringBuilder.Append(dYears == 1
                    ? $"{dYears} year ago"
                    : $"{dYears} years ago");
            }
            else if (dYears > 0 && dMonths > 0)
            {
                stringBuilder.Append(dYears == 1
                    ? $"{dYears} year"
                    : $"{dYears} years");
                stringBuilder.Append(dMonths == 1
                    ? $", {dMonths} month ago"
                    : $", {dMonths} months ago");
            }
            else if (dMonths > 0 && dDays == 0)
            {
                stringBuilder.Append(dMonths == 1
                    ? $"{dMonths} month ago"
                    : $"{dMonths} months ago");
            }
            else if (dMonths > 0 && dDays > 0)
            {
                stringBuilder.Append(dMonths == 1
                    ? $"{dMonths} month"
                    : $"{dMonths} months");
                stringBuilder.Append(dDays == 1
                    ? $", {dDays} day ago"
                    : $", {dDays} days ago");
            }
            else if (dDays > 0 && dHours == 0)
            {
                stringBuilder.Append(dDays == 1
                    ? $"{dDays} day ago"
                    : $"{dDays} days ago");
            }
            else if (dDays > 0 && dHours > 0)
            {
                stringBuilder.Append(dDays == 1
                    ? $"{dDays} day"
                    : $"{dDays} days");
                stringBuilder.Append(dHours == 1
                    ? $", {dHours} hour ago"
                    : $", {dHours} hours ago");
            }
            else if (dHours > 0 && dMinutes == 0)
            {
                stringBuilder.Append(dHours == 1
                    ? $"{dHours} hour ago"
                    : $"{dHours} hours ago");
            }
            else if (dHours > 0 && dMinutes > 0)
            {
                stringBuilder.Append(dHours == 1
                    ? $"{dHours} hour"
                    : $"{dHours} hours");
                stringBuilder.Append(dMinutes == 1
                    ? $", {dMinutes} minute ago"
                    : $", {dMinutes} minutes ago");
            }
            else if (dMinutes > 0 && dSeconds == 0)
            {
                stringBuilder.Append(dMinutes == 1
                    ? $"{dMinutes} minute ago"
                    : $"{dMinutes} minutes ago");
            }
            else if (dMinutes > 0 && dSeconds > 0)
            {
                stringBuilder.Append(dMinutes == 1
                    ? $"{dMinutes} minute"
                    : $"{dMinutes} minutes");
                stringBuilder.Append(dSeconds == 1
                    ? $", {dSeconds} second ago"
                    : $", {dSeconds} seconds ago");
            }
            else if (dSeconds > 0)
            {
                stringBuilder.Append(dSeconds == 1
                    ? $"{dSeconds} second ago"
                    : $"{dSeconds} seconds ago");
            }
            else if (dSeconds == 0)
            {
                stringBuilder.Append($"Just now");
            }

            return stringBuilder.ToString();
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