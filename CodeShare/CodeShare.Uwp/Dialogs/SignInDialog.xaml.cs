using CodeShare.Uwp.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class SignInDialog : ContentDialog
    {
        public SignInViewModel ViewModel { get; set; } = new SignInViewModel();

        public SignInDialog()
        {
            InitializeComponent();

            ViewModel.PropertyChanged += (s, e) => { if (ViewModel.CanClose) Hide();  };
        }
    }
}
