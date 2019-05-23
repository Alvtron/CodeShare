using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class EditableCodeBanner : UserControl
    {
        public static new readonly DependencyProperty ContentProperty = DependencyProperty.Register("Code", typeof(Code), typeof(EditableCodeBanner), new PropertyMetadata(default(Code)));
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(int), typeof(EditableCodeBanner), new PropertyMetadata(100));
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableCodeBanner), new PropertyMetadata(false));
        private static new readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(int), typeof(EditableCodeBanner), new PropertyMetadata(800));
        private static new readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(int), typeof(EditableCodeBanner), new PropertyMetadata(320));

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

        public Code Code
        {
            get => GetValue(ContentProperty) as Code;
            set => SetValue(ContentProperty, value);
        }

        private RelayCommand _uploadImageCommand;
        public ICommand UploadImageCommand => _uploadImageCommand = _uploadImageCommand ?? new RelayCommand(async param => await UploadImageAsync());

        private RelayCommand _cropImageCommand;
        public ICommand CropImageCommand => _cropImageCommand = _cropImageCommand ?? new RelayCommand(async param => await CropImageAsync());

        private RelayCommand _editImagesCommand;
        public ICommand EditImagesCommand => _editImagesCommand = _editImagesCommand ?? new RelayCommand(async param => await EditImagesAsync());

        public EditableCodeBanner()
        {
            InitializeComponent();
        }

        public async Task UploadImageAsync()
        {
            var dialog = new UploadImageDialog();
            await dialog.ShowAsync();
            var image = await dialog.CreateImageFromFile<CodeBanner>();

            if (image == null)
            {
                return;
            }

            Code.SetBanner(AuthService.CurrentUser, image);
        }

        public async Task CropImageAsync()
        {
            if (Code?.Banner == null)
            {
                return;
            }

            var dialog = new BannerCroppingDialog(Code.Banner);

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return;
            }

            Code.RefreshBindings();
        }

        public async Task EditImagesAsync()
        {
            if (Code?.Banners?.Count == 0)
            {
                return;
            }

            var dialog = new CodeBannersManagerDialog(Code.Banners, $"{Code.Name} - Profile Banners");
            await dialog.ShowAsync();

            Code.RefreshBindings();
        }
    }
}
