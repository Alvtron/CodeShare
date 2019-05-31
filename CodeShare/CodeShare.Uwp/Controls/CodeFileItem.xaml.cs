// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-25-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodeFileItem.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CodeFileItem. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CodeFileItem
    {
        /// <summary>
        /// The code file property
        /// </summary>
        public static readonly DependencyProperty CodeFileProperty = DependencyProperty.Register("CodeFile", typeof(CodeFile), typeof(CodeFileItem), new PropertyMetadata(default(CodeFile)));
        /// <summary>
        /// The is editable property
        /// </summary>
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(CodeFileItem), new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the code file.
        /// </summary>
        /// <value>The code file.</value>
        public CodeFile CodeFile
        {
            get => GetValue(CodeFileProperty) as CodeFile;
            set => SetValue(CodeFileProperty, value);
        }
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
        /// The edit file command
        /// </summary>
        private RelayCommand _editFileCommand;
        /// <summary>
        /// Gets the edit file command.
        /// </summary>
        /// <value>The edit file command.</value>
        public ICommand EditFileCommand => _editFileCommand = _editFileCommand ?? new RelayCommand(async file => await EditFileAsync());

        /// <summary>
        /// The archive file command
        /// </summary>
        private RelayCommand _archiveFileCommand;
        /// <summary>
        /// Gets the archive file command.
        /// </summary>
        /// <value>The archive file command.</value>
        public ICommand ArchiveFileCommand => _archiveFileCommand = _archiveFileCommand ?? new RelayCommand(async file => await ArchiveFileAsync());

        /// <summary>
        /// The delete file command
        /// </summary>
        private RelayCommand _deleteFileCommand;
        /// <summary>
        /// Gets the delete file command.
        /// </summary>
        /// <value>The delete file command.</value>
        public ICommand DeleteFileCommand => _deleteFileCommand = _deleteFileCommand ?? new RelayCommand(async file => await DeleteFileAsync());

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeFileItem"/> class.
        /// </summary>
        public CodeFileItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// edit file as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task EditFileAsync()
        {
            var dialog = new EditFileDialog(CodeFile);
            await dialog.ShowAsync();
        }

        /// <summary>
        /// Archives the file asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private Task ArchiveFileAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// delete file as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task DeleteFileAsync()
        {
            if (!await RestApiService<CodeFile>.Delete(CodeFile.Uid))
            {
                await NotificationService.DisplayErrorMessage($"Deletion of file '{CodeFile.FullName}' was unsuccessful. Sorry about that.");
            }
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(await RestApiService<CodeFile>.Get(CodeFile.Uid) is CodeFile codeFile))
            {
                return;
            }

            CodeFile = codeFile;
            IsEditable = AuthService.CurrentUser != null && AuthService.CurrentUser.Equals(codeFile.Code.User);
        }
    }
}
