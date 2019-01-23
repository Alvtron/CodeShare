using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class UserSettingsPage : Page
    {
        public UserSettingsViewModel ViewModel;

        public UserSettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is User user)
            {
                ViewModel = new UserSettingsViewModel(user);
                NavigationService.SetHeaderTitle("Account Settings");
            }
            else
            {
                Frame.GoBack();
            }
        }
    }
}
