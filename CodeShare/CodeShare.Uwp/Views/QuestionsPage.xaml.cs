using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class QuestionsPage : Page
    {
        public QuestionsViewModel ViewModel { get; private set; } = new QuestionsViewModel();

        public QuestionsPage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is Question question))
            {
                await NotificationService.DisplayErrorMessage("There seems to be something wrong with that user. Sorry about that.");
                return;
            }

            NavigationService.Navigate(typeof(QuestionPage), question, question.Title);
        }
    }
}
