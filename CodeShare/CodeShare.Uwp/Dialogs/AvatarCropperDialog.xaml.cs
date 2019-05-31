// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 05-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="AvatarCropperDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Windows.Media.Audio;
using CodeShare.Model;
using CodeShare.Uwp.Utilities;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class AvatarCropperDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    public sealed partial class AvatarCropperDialog
    {
        /// <summary>
        /// Gets the image cropper.
        /// </summary>
        /// <value>The image cropper.</value>
        private ImageCropper<WebImage> ImageCropper { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public WebImage Result => ImageCropper.Image;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarCropperDialog"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public AvatarCropperDialog(WebImage image)
        {
            ImageCropper = new ImageCropper<WebImage>(image);
            InitializeComponent();
        }
    }
}
