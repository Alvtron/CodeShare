using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class AddCodeDialog : ContentDialog
    {
        private AddCodeViewModel ViewModel { get; }

        public AddCodeDialog()
        {
            if (AuthService.CurrentUser == null)
            {
                Hide();
                return;
            }

            ViewModel = new AddCodeViewModel();
            InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (await ViewModel.UploadCodeAsync())
            {
                Hide();
            }
        }
    }
}
