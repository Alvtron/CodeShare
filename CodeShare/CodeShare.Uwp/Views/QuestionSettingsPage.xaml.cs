using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CodeShare.Uwp.Views
{
    public sealed partial class QuestionSettingsPage : Page
    {
        private QuestionSettingsViewModel ViewModel;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            Question question;

            switch (e.Parameter)
            {
                case Question _question:
                    question = _question;
                    break;
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

            ViewModel = new QuestionSettingsViewModel(question);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle($"{ViewModel.Model?.Name} - Settings");
        }
    }
}
