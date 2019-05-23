using CodeShare.Model;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class EditFileDialog : ContentDialog
    {
        private File File { get; }
        private string OriginalFileData { get; set; }

        public EditFileDialog(File file)
        {
            File = file;
            OriginalFileData = file.Data;
            Title = $"Edit {file}";
            InitializeComponent();
        }

        private void Discard_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            File.Data = OriginalFileData;
        }

        private void Save_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
