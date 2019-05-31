// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="ActivityListView.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using CodeShare.RestApi;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class ActivityListView. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class ActivityListView
    {
        /// <summary>
        /// The logs source property
        /// </summary>
        public static readonly DependencyProperty LogsSourceProperty = DependencyProperty.Register("LogsSource", typeof(object), typeof(ActivityListView), new PropertyMetadata(null));

        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(ActivityListView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the logs source.
        /// </summary>
        /// <value>The logs source.</value>
        public object LogsSource
        {
            get => GetValue(LogsSourceProperty);
            set
            {
                if (value is IEnumerable<ILog> logs)
                {
                    SetValue(LogsSourceProperty, logs.OrderByDescending(l => l.Created));
                }
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityListView"/> class.
        /// </summary>
        public ActivityListView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the Actor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Actor_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;

            await NavigationService.Navigate(log.ActorType, log.ActorUid);
        }

        /// <summary>
        /// Handles the Click event of the Subject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Subject_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;

            await NavigationService.Navigate(log.SubjectType, log.SubjectUid);
        }

        /// <summary>
        /// Handles the Loaded event of the Actor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Actor_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;
            if (!log.ActorUid.HasValue) return;

            await PopulateLinkContent(link, log.ActorType, log.ActorUid.Value);
        }

        /// <summary>
        /// Handles the Loaded event of the Subject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Subject_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;
            if (!log.SubjectUid.HasValue) return;

            await PopulateLinkContent(link, log.SubjectType, log.SubjectUid.Value);
        }

        /// <summary>
        /// Populates the content of the link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="type">The type.</param>
        /// <param name="uid">The uid.</param>
        /// <returns>Task.</returns>
        private async Task PopulateLinkContent(HyperlinkButton link, string type, Guid uid)
        {
            switch (type)
            {
                case "Code":
                    var code = await RestApiService<Code>.Get(uid);
                    link.Content = code?.Name ?? string.Empty;
                    return;
                case "Question":
                    var question = await RestApiService<Question>.Get(uid);
                    link.Content = question?.Name ?? string.Empty;
                    return;
                case "File":
                    var file = await RestApiService<File>.Get(uid);
                    link.Content = file?.Name ?? string.Empty;
                    return;
                case "User":
                    var user = await RestApiService<User>.Get(uid);
                    link.Content = user?.Name ?? string.Empty;
                    return;
            }
        }
    }
}
