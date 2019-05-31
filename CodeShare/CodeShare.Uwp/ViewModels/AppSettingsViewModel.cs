// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="AppSettingsViewModel.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Uwp.Services;

namespace CodeShare.Uwp.ViewModels
{
    /// <summary>
    /// Class AppSettingsViewModel.
    /// Implements the <see cref="CodeShare.Uwp.ViewModels.ViewModel" />
    /// </summary>
    /// <seealso cref="CodeShare.Uwp.ViewModels.ViewModel" />
    public class AppSettingsViewModel : ViewModel
    {
        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        public AppSettings AppSettings { get; } = new AppSettings();
    }
}
