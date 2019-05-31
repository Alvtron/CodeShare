// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="LoadingDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class LoadingDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class LoadingDialog
    {
        /// <summary>
        /// The timer
        /// </summary>
        private readonly DispatcherTimer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingDialog"/> class.
        /// </summary>
        /// <param name="timeInSeconds">The time in seconds.</param>
        public LoadingDialog(int timeInSeconds)
        {
            InitializeComponent();
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, timeInSeconds)
            };

            _timer.Tick += (s, e) => Hide();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingDialog"/> class.
        /// </summary>
        public LoadingDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            Hide();
        }

        /// <summary>
        /// Handles the Loaded event of the ProgressRing control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ProgressRing_Loaded(object sender, RoutedEventArgs e)
        {
            _timer?.Start();
        }
    }
}
