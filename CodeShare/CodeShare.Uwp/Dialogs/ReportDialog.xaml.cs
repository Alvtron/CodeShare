using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class ReportDialog : ContentDialog
    {
        public string Message { get; set; }
        public bool Valid => !string.IsNullOrWhiteSpace(Message);

        public ReportDialog(string targetName = null)
        {
            InitializeComponent();
            Title = "Report " + targetName;
        }

        private void ReportMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            Message = ReportMessage.Text;
            IsSecondaryButtonEnabled = Valid;
        }
    }
}
