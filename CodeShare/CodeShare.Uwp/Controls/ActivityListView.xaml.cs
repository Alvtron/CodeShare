using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class ActivityListView : UserControl
    {
        public static readonly DependencyProperty LogsSourceProperty = DependencyProperty.Register("LogsSource", typeof(object), typeof(ActivityListView), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(ActivityListView), new PropertyMetadata(null));

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

        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        public ActivityListView()
        {
            InitializeComponent();
        }

        private async void Actor_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;

            await NavigationService.Navigate(log.ActorType, log.ActorUid);
        }

        private async void Subject_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;

            await NavigationService.Navigate(log.SubjectType, log.SubjectUid);
        }

        private async void Actor_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;
            if (!log.ActorUid.HasValue) return;

            await PopulateLinkContent(link, log.ActorType, log.ActorUid.Value);
        }

        private async void Subject_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton link)) return;
            if (!(link.Tag is ILog log)) return;
            if (!log.SubjectUid.HasValue) return;

            await PopulateLinkContent(link, log.SubjectType, log.SubjectUid.Value);
        }

        private async Task PopulateLinkContent(HyperlinkButton link, string type, Guid uid)
        {
            switch (type)
            {
                case "Code":
                    var code = await RestApiService<Code>.Get(uid);
                    if (code != null) link.Content = code?.Name;
                    return;
                case "Question":
                    var question = await RestApiService<Question>.Get(uid);
                    if (question != null) link.Content = question?.Name;
                    return;
                case "File":
                    var file = await RestApiService<Model.File>.Get(uid);
                    if (file != null) link.Content = file?.Name;
                    return;
                case "User":
                    var user = await RestApiService<User>.Get(uid);
                    if (user != null) link.Content = user?.Name;
                    return;
                default:
                    break;
            }
        }
    }
}
