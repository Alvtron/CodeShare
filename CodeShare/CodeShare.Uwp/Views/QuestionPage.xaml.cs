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
    public sealed partial class QuestionPage : Page
    {
        public QuestionViewModel ViewModel { get; set; } = new QuestionViewModel();

        public QuestionPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            switch (e.Parameter)
            {
                case Guid guid:
                    ViewModel.Question = await RestApiService<Question>.Get(guid);
                    break;
                case IEntity entity:
                    ViewModel.Question = await RestApiService<Question>.Get(entity.Uid);
                    break;
            }

            NavigationService.Unlock();

            if (ViewModel.Question == null)
            {
                await NotificationService.DisplayErrorMessage("This resource pack do not exist.");
                NavigationService.GoBack();
            }


            ViewModel.Question.Views++;

            if (!await RestApiService<Question>.Update(ViewModel.Question, ViewModel.Question.Uid))
            {
                Debug.WriteLine($"Failed to increment view counter for code {ViewModel.Question.Uid}.");
            }

            NavigationService.SetHeaderTitle(ViewModel.Question?.Title);
        }
    }
}
