// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="EditableCodeBanner.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class EditableCodeBanner. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class EditableCodeBanner
    {
        /// <summary>
        /// The content property
        /// </summary>
        public new static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Code", typeof(Code), typeof(EditableCodeBanner), new PropertyMetadata(default(Code)));
        /// <summary>
        /// The is editable property
        /// </summary>
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableCodeBanner), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is editable.
        /// </summary>
        /// <value><c>true</c> if this instance is editable; otherwise, <c>false</c>.</value>
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public Code Code
        {
            get => GetValue(ContentProperty) as Code;
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// The upload image command
        /// </summary>
        private RelayCommand _uploadImageCommand;
        /// <summary>
        /// Gets the upload image command.
        /// </summary>
        /// <value>The upload image command.</value>
        public ICommand UploadImageCommand => _uploadImageCommand = _uploadImageCommand ?? new RelayCommand(async param => await UploadImageAsync());

        /// <summary>
        /// The crop image command
        /// </summary>
        private RelayCommand _cropImageCommand;
        /// <summary>
        /// Gets the crop image command.
        /// </summary>
        /// <value>The crop image command.</value>
        public ICommand CropImageCommand => _cropImageCommand = _cropImageCommand ?? new RelayCommand(async param => await CropImageAsync());

        /// <summary>
        /// The edit images command
        /// </summary>
        private RelayCommand _editImagesCommand;
        /// <summary>
        /// Gets the edit images command.
        /// </summary>
        /// <value>The edit images command.</value>
        public ICommand EditImagesCommand => _editImagesCommand = _editImagesCommand ?? new RelayCommand(async param => await EditImagesAsync());

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableCodeBanner"/> class.
        /// </summary>
        public EditableCodeBanner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// upload image as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
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

        /// <summary>
        /// crop image as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task CropImageAsync()
        {
            if (Code?.Banner == null)
            {
                return;
            }

            var dialog = new BannerCropperDialog(Code.Banner);

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return;
            }

            Code.RefreshBindings();
        }

        /// <summary>
        /// edit images as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task EditImagesAsync()
        {
            if (Code?.Banners?.Count == 0)
            {
                return;
            }

            var dialog = new CodeBannerManagerDialog(Code?.Banners, $"{Code?.Name} - Profile Banners");
            await dialog.ShowAsync();

            Code.RefreshBindings();
        }
    }
}
