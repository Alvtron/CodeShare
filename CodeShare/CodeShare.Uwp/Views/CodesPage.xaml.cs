using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class CodesPage : Page
    {
        public CodesViewModel ViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            var codes = await RestApiService<Code>.Get();

            if (codes == null)
            {
                await NotificationService.DisplayErrorMessage("Could not retrieve codes from database.");
                NavigationService.GoBack();
            }

            ViewModel = new CodesViewModel(codes);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle("Codes");
        }
    }
}
