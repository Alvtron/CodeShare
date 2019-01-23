using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
