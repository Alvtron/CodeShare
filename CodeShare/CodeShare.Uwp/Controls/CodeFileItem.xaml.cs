using CodeShare.Model;
using CodeShare.RestApi;
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
    public sealed partial class CodeFileItem : UserControl
    {
        public static readonly DependencyProperty CodeFileProperty = DependencyProperty.Register("CodeFile", typeof(CodeFile), typeof(CodeFileItem), new PropertyMetadata(default(CodeFile)));
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(CodeFileItem), new PropertyMetadata(false));

        public CodeFile CodeFile
        {
            get => GetValue(CodeFileProperty) as CodeFile;
            set => SetValue(CodeFileProperty, value);
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

        public CodeFileItem()
        {
            InitializeComponent();
        }

        private async Task EditFileAsync()
        {
            var dialog = new EditFileDialog(CodeFile);
            await dialog.ShowAsync();
        }

        private Task ArchiveFileAsync()
        {
            throw new NotImplementedException();
        }

        private async Task DeleteFileAsync()
        {
            if (!await RestApiService<CodeFile>.Delete(CodeFile.Uid))
            {
                await NotificationService.DisplayErrorMessage($"Deletion of file '{CodeFile.FullName}' was unsuccessful. Sorry about that.");
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(await RestApiService<CodeFile>.Get(CodeFile.Uid) is CodeFile codeFile))
            {
                return;
            }

            CodeFile = codeFile;
            IsEditable = (AuthService.CurrentUser == null)
                ? false
                : AuthService.CurrentUser.Equals(codeFile.Code.User);
        }
    }
}
