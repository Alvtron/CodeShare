using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class AddCommentDialog : ContentDialog
    {
        public string Text { get; private set; } = string.Empty;

        public AddCommentDialog(string header)
        {
            if (AuthService.CurrentUser == null)
            {
                Hide();
                return;
            }

            Title = header;
            InitializeComponent();
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Text = string.Empty;
        }
    }
}
