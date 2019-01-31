using CodeShare.Model;
using CodeShare.Uwp.Services;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class AddReplyDialog : ContentDialog
    {
        private Reply ParentComment { get; set; }
        private string Text { get; set; } = string.Empty;

        public AddReplyDialog(Reply parentComment)
        {
            if (parentComment == null)
            {
                Hide();
            }

            ParentComment = parentComment;
            Title = $"Reply to {ParentComment?.User?.Name}";
            InitializeComponent();
        }

        private void Reply_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return;
            }

            ParentComment.AddReply(new Reply(AuthService.CurrentUser, Text));
        }
    }
}
