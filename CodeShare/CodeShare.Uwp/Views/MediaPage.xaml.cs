// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="MediaPage.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    /// <summary>
    /// Class MediaPage. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.Page" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Page" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class MediaPage
    {
        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        private Video Video { get; set; }
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        private WebFile File { get; set; }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
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

            InitializeComponent();
        }

        /// <summary>
        /// Handles the Loaded event of the ImageView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Loaded event of the VideoView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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
