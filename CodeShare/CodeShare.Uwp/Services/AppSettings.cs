// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 01-31-2019
// ***********************************************************************
// <copyright file="AppSettings.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Utilities;
using Windows.Storage;

/*
 * Source: https://docs.microsoft.com/en-us/windows/uwp/design/app-settings/store-and-retrieve-app-data
 * 
 * Here are data types you can use for app settings:
 * UInt8, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double
 * Boolean
 * Char16, String
 * DateTime, TimeSpan
 * GUID, Point, Size, Rect
 */

namespace CodeShare.Uwp.Services
{
    /// <summary>
    /// Class AppSettings. This class cannot be inherited.
    /// Implements the <see cref="CodeShare.Model.ObservableObject" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.ObservableObject" />
    public sealed class AppSettings : ObservableObject
    {
        /// <summary>
        /// Gets the local settings.
        /// </summary>
        /// <value>The local settings.</value>
        public static ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        /// <summary>
        /// Gets or sets a value indicating whether [stay signed in].
        /// </summary>
        /// <value><c>true</c> if [stay signed in]; otherwise, <c>false</c>.</value>
        public bool StaySignedIn
        {
            get => ReadSettings(nameof(StaySignedIn), false);
            set
            {
                SaveSettings(nameof(StaySignedIn), value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the default user.
        /// </summary>
        /// <value>The default user.</value>
        public string DefaultUser
        {
            get => ReadSettings(nameof(DefaultUser), "");
            set
            {
                SaveSettings(nameof(DefaultUser), value);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Reads the settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public static T ReadSettings<T>(string key, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(key))
                return (T)LocalSettings.Values[key];

            return defaultValue != null
                ? defaultValue
                : default(T);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SaveSettings(string key, object value)
        {
            LocalSettings.Values[key] = value;
        }

        /// <summary>
        /// Deletes the settings.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void DeleteSettings(string key)
        {
            LocalSettings.Values[key] = null;
        }

        /// <summary>
        /// Deletes all settings.
        /// </summary>
        public static void DeleteAllSettings()
        {
            DeleteSettings(nameof(StaySignedIn));
            DeleteSettings(nameof(DefaultUser));
        }

        /// <summary>
        /// Prints the settings.
        /// </summary>
        public static void PrintSettings()
        {
            Logger.WriteLine("App settings stored:");

            foreach (var key in ApplicationData.Current.LocalSettings.Values)
            {
                Logger.WriteLine($"{key.Key}: {key.Value}");
            }
        }
    }
}
