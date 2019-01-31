using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class AddCodeDialog : ContentDialog
    {
        public AddCodeViewModel ViewModel { get; private set; } = new AddCodeViewModel();

        public AddCodeDialog()
        {
            InitializeComponent();
        }

        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (await ViewModel.UploadCodeAsync())
            {
                Hide();
            }
        }
    }
}
