using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class CodePage : Page
    {
        private CodeViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            Code code;

            switch (e.Parameter)
            {
                case Guid guid:
                    code = await RestApiService<Code>.Get(guid);
                    break;
                case IEntity entity:
                    code = await RestApiService<Code>.Get(entity.Uid);
                    break;
                default:
                    Logger.WriteLine("Developer error.");
                    throw new ArgumentException("Developer error.");
            }

            if (code == null)
            {
                await NotificationService.DisplayErrorMessage("This code does not exist.");
                NavigationService.GoBack();
            }

            ViewModel = new CodeViewModel(code);
            InitializeComponent();
            NavigationService.Unlock();
            await ViewModel.IncrementViewsAsync();
            NavigationService.SetHeaderTitle(ViewModel.Model?.Name);
        }


    }
}
