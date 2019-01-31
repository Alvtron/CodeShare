using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class QuestionPage : Page
    {
        private QuestionViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            Question question;

            switch (e.Parameter)
            {
                case Guid guid:
                    question = await RestApiService<Question>.Get(guid);
                    break;
                case IEntity entity:
                    question = await RestApiService<Question>.Get(entity.Uid);
                    break;
                default:
                    await NotificationService.DisplayErrorMessage("Developer error.");
                    throw new InvalidOperationException();
            }

            if (question == null)
            {
                await NotificationService.DisplayErrorMessage("This question does not exist.");
                NavigationService.GoBack();
            }

            ViewModel = new QuestionViewModel(question);

            InitializeComponent();

            NavigationService.Unlock();

            ViewModel.Model.Views++;

            if (!await RestApiService<Question>.Update(ViewModel.Model, ViewModel.Model.Uid))
            {
                Logger.WriteLine($"Failed to increment view counter for question {ViewModel.Model.Uid}.");
            }

            NavigationService.SetHeaderTitle(ViewModel.Model?.Name);
        }
    }
}
