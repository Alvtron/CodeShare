using CodeShare.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class MediaPage : Page
    {
        private Video Video { get; set; }
        private WebFile File { get; set; }

        public MediaPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (e.Parameter)
            {
                case WebFile file:
                    File = file;
                    break;
                case Video video:
                    Video = video;
                    break;
                default:
                    Frame.GoBack();
                    break;
            }
        }

        private void ImageView_Loaded(object sender, RoutedEventArgs e)
        {
            if (File == null || !File.Exist)
            {
                ImageView.Visibility = Visibility.Collapsed;
                return;
            }

            ImageView.Visibility = Visibility.Visible;
            ImageView.Source = new BitmapImage(File.Url);
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            if (Video == null || Video.Empty)
            {
                VideoView.Visibility = Visibility.Collapsed;
                return;
            }

            VideoView.Visibility = Visibility.Visible;
            VideoView.Source = Video.YouTubeUri;
        }
    }
}
