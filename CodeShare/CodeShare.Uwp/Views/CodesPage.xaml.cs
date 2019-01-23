using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class CodesPage : Page
    {
        public CodesViewModel ViewModel = new CodesViewModel();

        public CodesPage()
        {
            InitializeComponent();
        }
    }
}
