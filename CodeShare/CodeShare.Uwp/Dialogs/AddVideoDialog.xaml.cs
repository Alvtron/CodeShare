using CodeShare.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class AddVideoDialog : ContentDialog
    {
        public Video VideoData { get; } = new Video();

        public AddVideoDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ViewVideoButton_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Visibility = Visibility.Visible;
            VideoPlayer.Source = VideoData.YouTubeUri;
        }
    }
}
