using System;
using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel { get; set; } = new HomeViewModel();

        public HomePage()
        {
            InitializeComponent();
        }

        private async void AddCode_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddCodeDialog();
            await dialog.ShowAsync();
        }

        private async void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddQuestionDialog();
            await dialog.ShowAsync();
        }

        private async void MostPopularCodesGridView_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.RefreshMostPopularCodes();
        }

        private async void NewQuestionsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.RefreshNewestQuestions();
        }

        private async void NewUsersGridView_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.RefreshNewestUsers();
        }
    }
}
