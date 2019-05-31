// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="AddCodeVideoDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Services;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class AddCodeVideoDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class AddCodeVideoDialog
    {
        /// <summary>
        /// Gets the video.
        /// </summary>
        /// <value>The video.</value>
        public CodeVideo Video { get; private set; } = new CodeVideo();

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCodeVideoDialog"/> class.
        /// </summary>
        public AddCodeVideoDialog()
        {
            if (AuthService.CurrentUser == null)
            {
                Hide();
                return;
            }

            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the ViewVideoButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewVideoButton_Click(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Visibility = Visibility.Visible;
            VideoPlayer.Source = Video.YouTubeUri;
        }
    }
}
