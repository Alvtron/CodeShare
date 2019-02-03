using CodeShare.Model;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class ReplyDialog : ContentDialog
    {
        private Reply Comment { get; }

        public ReplyDialog(Reply comment)
        {
            InitializeComponent();
            Comment = comment;
        }
    }
}
