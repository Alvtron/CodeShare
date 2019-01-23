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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class ActivityListView : UserControl
    {
        public static readonly DependencyProperty LogsProperty = DependencyProperty.Register("Logs", typeof(IEnumerable<ContentLog>), typeof(ActivityListView), new PropertyMetadata(new List<ContentLog>()));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(QuestionGridView), new PropertyMetadata(null));

        public IEnumerable<ContentLog> Logs
        {
            get => GetValue(LogsProperty) as IEnumerable<ContentLog>;
            set => SetValue(LogsProperty, value);
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

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is HyperlinkButton btn)) return;

            var target = btn.Tag;

            switch (target)
            {
                case Code code:
                    NavigationService.Navigate(typeof(CodePage), code, code.Name);
                    return;
                case User user:
                    NavigationService.Navigate(typeof(UserPage), user, user.Name);
                    return;
                case Comment comment:
                    var dialog = new CommentDialog(comment);
                    await dialog.ShowAsync();
                    return;
            }
        }
    }
}
