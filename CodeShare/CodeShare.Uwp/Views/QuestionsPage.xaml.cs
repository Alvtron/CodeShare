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
    public sealed partial class QuestionsPage : Page
    {
        public QuestionsViewModel ViewModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Lock();

            var questions = await RestApiService<Question>.Get();

            if (questions == null)
            {
                await NotificationService.DisplayErrorMessage("Could not retrieve questions from database.");
                NavigationService.GoBack();
            }

            ViewModel = new QuestionsViewModel(questions);

            InitializeComponent();

            NavigationService.Unlock();

            NavigationService.SetHeaderTitle("SearchBox Questions");
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is Question question))
            {
                await NotificationService.DisplayErrorMessage("There seems to be something wrong with that question. Sorry about that.");
                return;
            }

            NavigationService.Navigate(typeof(QuestionPage), question, $"{question.Title}");
        }
    }
}
