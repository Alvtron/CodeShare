using CodeShare.Model;
using CodeShare.Uwp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class ProfilePicturesManagerDialog : ContentDialog
    {
        private ImageManager<ProfilePicture> ImageManager { get; }

        public ProfilePicturesManagerDialog(ObservableCollection<ProfilePicture> profilePictures, string dialogTitle)
        {
            ImageManager = new ImageManager<ProfilePicture>(profilePictures);
            InitializeComponent();
            UpdateSelectedItemsCounter();
            Title = dialogTitle;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ImageGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedItemsCounter();
        }

        private void UpdateSelectedItemsCounter()
        {
            SelectedItemsText.Text = $"{ImageGridView.SelectedItems.Count}";
            SelectedItemsText.Text += ImageGridView.SelectedItems.Count > 0 ? " items selected" : " item selected";
        }
    }
}
