﻿using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class ProfilePictureCroppingDialog : ContentDialog
    {
        private ImageCropper<ProfilePicture> ImageCropper { get; }

        public ProfilePicture Result => ImageCropper.Image;

        public ProfilePictureCroppingDialog(ProfilePicture profilePicture)
        {
            ImageCropper = new ImageCropper<ProfilePicture>(profilePicture);
            InitializeComponent();
        }
    }
}
