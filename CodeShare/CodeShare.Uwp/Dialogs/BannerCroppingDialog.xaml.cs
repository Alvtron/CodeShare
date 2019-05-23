using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class BannerCroppingDialog : ContentDialog
    {
        private ImageCropper<WebImage> ImageCropper { get; }

        public WebImage Result => ImageCropper.Image;

        public BannerCroppingDialog(WebImage banner)
        {
            ImageCropper = new ImageCropper<WebImage>(banner);
            InitializeComponent();
        }
    }
}
