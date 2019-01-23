using System;
using CodeShare.Model;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeShare.Uwp.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        private RelayCommand<object> _goToCommand;
        public ICommand GoToCommand => _goToCommand = _goToCommand ?? new RelayCommand<object>(async param => await GoTo(param));
        private static async Task GoTo(object target)
        {
            switch (target)
            {
                case Code code:
                    NavigationService.Navigate(typeof(CodePage), code, code.Name);
                    return;
                case User user:
                    NavigationService.Navigate(typeof(UserPage), user, user.Name);
                    return;
                case Comment comment:
                    var dialog = new CommentDialog(comment);
                    await dialog.ShowAsync();
                    return;
            }
        }
    }
}
