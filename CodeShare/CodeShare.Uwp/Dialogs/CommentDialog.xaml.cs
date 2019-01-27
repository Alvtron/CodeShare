using CodeShare.Model;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class CommentDialog : ContentDialog
    {
        public Comment Comment { get; private set; }

        public CommentDialog(Comment comment)
        {
            InitializeComponent();
            Comment = comment;
        }
    }
}
