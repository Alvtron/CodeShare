using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; private set; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void PageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeApplication();
        }

        public async Task InitializeApplication()
        {
            NavigationService.Lock();

            //var loadingDialog = await NotificationService.CreateAndDisplayLoadingDialog();

            AppSettings.DeleteAllSettings();

            await AuthService.SignInAsync();

            AppSettings.PrintSettings();

            //loadingDialog.Close();

            NavigationService.Unlock();
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.Initialize(_contentFrame, _navigationView, _progressRing);
            NavigationService.Navigate(typeof(HomePage), null, "Home");

            if (_navigationView.IsPaneOpen)
            {
                NavigationViewItemSearchIcon.Visibility = Visibility.Collapsed;
                NavigationViewItemSearchBox.Visibility = Visibility.Visible;
            }
            else
            {
                NavigationViewItemSearchBox.Visibility = Visibility.Collapsed;
                NavigationViewItemSearchIcon.Visibility = Visibility.Visible;
            }
        }

        private void NavigationView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private async void NavigationView_Invoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (!(args.InvokedItem is string viewItemContent))
            {
                Debug.WriteLine("Invoked navigation item has no content. No navigation can be done.");
                return;
            }

            if (viewItemContent == "SearchIcon")
            {
                _navigationView.IsPaneOpen = true;
                SearchBox.Focus(FocusState.Keyboard);
            }

            await NavigationService.Navigate(viewItemContent);
        }

        public async void ASB_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                await ViewModel.Search(sender.Text.ToLower());
        }

        private void ASB_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.IsSuggestionListOpen = false;
            ViewModel.SubmitSearch(args);
        }

        private void NavigationView_PaneOpening(NavigationView sender, object args)
        {
            NavigationViewItemSearchIcon.Visibility = Visibility.Collapsed;
            NavigationViewItemSearchBox.Visibility = Visibility.Visible;
        }

        private void NavigationView_PaneClosing(NavigationView sender, object args)
        {
            NavigationViewItemSearchBox.Visibility = Visibility.Collapsed;
            NavigationViewItemSearchIcon.Visibility = Visibility.Visible;
        }
    }
}
