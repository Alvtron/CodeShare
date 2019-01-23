using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    public sealed partial class CodeSettingsPage : Page
    {
        public CodeSettingsViewModel ViewModel;

        public CodeSettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (e.Parameter)
            {
                case Code code:
                    ViewModel = new CodeSettingsViewModel(code);
                    NavigationService.SetHeaderTitle("Edit " + code.Name);
                    break;
                default:
                    NavigationService.GoBack();
                    return;
            }
        }
    }
}
