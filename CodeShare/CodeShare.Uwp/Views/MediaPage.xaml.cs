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
        private Video _video;
        private WebFile _file;

        public MediaPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (e.Parameter)
            {
                case WebFile file:
                    _file = file;
                    break;
                case Video video:
                    _video = video;
                    break;
                default:
                    Frame.GoBack();
                    break;
            }
        }

        private void ImageView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_file == null || !_file.Exist)
            {
                ImageView.Visibility = Visibility.Collapsed;
                return;
            }

            ImageView.Visibility = Visibility.Visible;
            ImageView.Source = new BitmapImage(_file.Url);
        }

        private void VideoView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_video == null || _video.Empty)
            {
                VideoView.Visibility = Visibility.Collapsed;
                return;
            }

            VideoView.Visibility = Visibility.Visible;
            VideoView.Source = _video.YouTubeUri;
        }
    }
}
