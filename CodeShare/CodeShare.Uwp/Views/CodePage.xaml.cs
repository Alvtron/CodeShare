using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class CodePage : Page
    {
        public CodeViewModel ViewModel { get; set; } = new CodeViewModel();
        
        public CodePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            switch (e.Parameter)
            {
                case Guid guid:
                    ViewModel.Code = await RestApiService<Code>.Get(guid);
                    break;
                case IEntity entity:
                    ViewModel.Code = await RestApiService<Code>.Get(entity.Uid);
                    break;
            }

            NavigationService.Unlock();

            if (ViewModel.Code == null)
            {
                await NotificationService.DisplayErrorMessage("This resource pack do not exist.");
                NavigationService.GoBack();
            }


            ViewModel.Code.Views++;

            if (!await RestApiService<Code>.Update(ViewModel.Code, ViewModel.Code.Uid))
            {
                Debug.WriteLine($"Failed to increment view counter for code {ViewModel.Code.Uid}.");
            }

            NavigationService.SetHeaderTitle(ViewModel.Code?.Name);
        }
    }
}
