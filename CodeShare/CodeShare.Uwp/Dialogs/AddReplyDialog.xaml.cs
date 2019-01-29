using CodeShare.Model;
using CodeShare.Uwp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
