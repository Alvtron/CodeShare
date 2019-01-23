using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class EditableProfilePicture : UserControl
    {
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(User), typeof(EditableProfilePicture), new PropertyMetadata(default(User)));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(int), typeof(EditableProfilePicture), new PropertyMetadata(100));
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableProfilePicture), new PropertyMetadata(false));
        private static new readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(int), typeof(EditableProfilePicture), new PropertyMetadata(100));
        private static new readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(int), typeof(EditableProfilePicture), new PropertyMetadata(100));

        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public new int Width
        {
            get => (int)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public new int Height
        {
            get => (int)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public User User
        {
            get => GetValue(UserProperty) as User;
            set => SetValue(UserProperty, value);
        }

        private RelayCommand _uploadImageCommand;
        public ICommand UploadImageCommand => _uploadImageCommand = _uploadImageCommand ?? new RelayCommand(async param => await UploadImageAsync());

        private RelayCommand _cropImageCommand;
        public ICommand CropImageCommand => _cropImageCommand = _cropImageCommand ?? new RelayCommand(async param => await CropImageAsync());

        private RelayCommand _editImagesCommand;
        public ICommand EditImagesCommand => _editImagesCommand = _editImagesCommand ?? new RelayCommand(async param => await EditImagesAsync());

        public EditableProfilePicture()
        {
            InitializeComponent();
        }

        public async Task UploadImageAsync()
        {
            var dialog = new UploadImageDialog();
            await dialog.ShowAsync();
            var image = await dialog.CreateImageFromFile<ProfilePicture>();

            if (image == null)
            {
                return;
            }

            User.SetProfilePicture(image);
            User.RefreshBindings();
        }

        public async Task CropImageAsync()
        {
            if (User?.ProfilePicture == null)
            {
                return;
            }

            var dialog = new ProfilePictureCroppingDialog(User.ProfilePicture);

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return;
            }

            User.RefreshBindings();
        }

        public async Task EditImagesAsync()
        {
            if (User?.ProfilePictures?.Count == 0)
            {
                return;
            }

            var dialog = new ProfilePicturesManagerDialog(User.ProfilePictures, $"{User.Name} - Profile Pictures");
            await dialog.ShowAsync();

            User.RefreshBindings();
        }
    }
}
