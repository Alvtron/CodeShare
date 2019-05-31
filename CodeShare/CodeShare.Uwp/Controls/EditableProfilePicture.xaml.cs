// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="EditableProfilePicture.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class EditableProfilePicture. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class EditableProfilePicture
    {
        /// <summary>
        /// The user property
        /// </summary>
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(User), typeof(EditableProfilePicture), new PropertyMetadata(default(User)));
        /// <summary>
        /// The size property
        /// </summary>
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(int), typeof(EditableProfilePicture), new PropertyMetadata(100));
        /// <summary>
        /// The is editable property
        /// </summary>
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableProfilePicture), new PropertyMetadata(false));
        /// <summary>
        /// The width property
        /// </summary>
        private new static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(int), typeof(EditableProfilePicture), new PropertyMetadata(100));
        /// <summary>
        /// The height property
        /// </summary>
        private new static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(int), typeof(EditableProfilePicture), new PropertyMetadata(100));

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
        /// Gets or sets the width of a FrameworkElement.
        /// </summary>
        /// <value>The width.</value>
        public new int Width
        {
            get => (int)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the suggested height of a FrameworkElement.
        /// </summary>
        /// <value>The height.</value>
        public new int Height
        {
            get => (int)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User
        {
            get => GetValue(UserProperty) as User;
            set => SetValue(UserProperty, value);
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
        /// Initializes a new instance of the <see cref="EditableProfilePicture"/> class.
        /// </summary>
        public EditableProfilePicture()
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
            var image = await dialog.CreateImageFromFile<UserAvatar>();

            if (image == null)
            {
                return;
            }

            User.SetProfilePicture(image);
            User.RefreshBindings();
        }

        /// <summary>
        /// crop image as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task CropImageAsync()
        {
            if (User?.Avatar == null)
            {
                return;
            }

            var dialog = new AvatarCropperDialog(User.Avatar);

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return;
            }

            User.RefreshBindings();
        }

        /// <summary>
        /// edit images as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task EditImagesAsync()
        {
            if (User?.Avatars?.Count == 0)
            {
                return;
            }

            var dialog = new UserAvatarManagerDialog(User.Avatars, $"{User.Name} - Profile Pictures");
            await dialog.ShowAsync();

            User.RefreshBindings();
        }
    }
}
