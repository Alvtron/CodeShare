using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
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
    public sealed partial class FileItem : UserControl
    {
        public static readonly DependencyProperty FileProperty = DependencyProperty.Register("File", typeof(File), typeof(FileItem), new PropertyMetadata(new File()));
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(FileItem), new PropertyMetadata(false));

        public File File
        {
            get => GetValue(FileProperty) as File;
            set => SetValue(FileProperty, value);
        }
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        private RelayCommand _editFileCommand;
        public ICommand EditFileCommand => _editFileCommand = _editFileCommand ?? new RelayCommand(async file => await EditFileAsync());

        private RelayCommand _archiveFileCommand;
        public ICommand ArchiveFileCommand => _archiveFileCommand = _archiveFileCommand ?? new RelayCommand(async file => await ArchiveFileAsync());

        private RelayCommand _deleteFileCommand;
        public ICommand DeleteFileCommand => _deleteFileCommand = _deleteFileCommand ?? new RelayCommand(async file => await DeleteFileAsync());

        public FileItem()
        {
            InitializeComponent();
        }

        private async Task EditFileAsync()
        {
            var dialog = new EditFileDialog(File);
            await dialog.ShowAsync();
        }

        private Task ArchiveFileAsync()
        {
            throw new NotImplementedException();
        }

        private async Task DeleteFileAsync()
        {
            if (!await RestApiService<File>.Delete(File.Uid))
            {
                await NotificationService.DisplayErrorMessage($"Deletion of file '{File.FullName}' was unsuccessful. Sorry about that.");
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(await RestApiService<File>.Get(File.Uid) is File file))
            {
                return;
            }

            File = file;
            IsEditable = AuthService.CurrentUser.Uid.Equals(file.Code.User.Uid);
        }
    }
}
