using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Views
{
    public sealed partial class UsersPage : Page
    {
        public UsersViewModel ViewModel { get; private set; } = new UsersViewModel();

        public UsersPage()
        {
            InitializeComponent();
        }

        private async void UserList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is User user) || !user.Valid)
            {
                await NotificationService.DisplayErrorMessage("There seems to be something wrong with that user. Sorry about that.");
                return;
            }

            NavigationService.Navigate(typeof(UserPage), user, $"{user.Name}'s Page");
        }
    }
}
