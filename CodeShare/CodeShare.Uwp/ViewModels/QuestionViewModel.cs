using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.ViewModels
{
    public class QuestionViewModel : ContentViewModel<Question>
    {
        public QuestionViewModel(Question question)
            : base(question)
        {
        }

        public override bool OnSetAuthorPrivileges(Question model)
        {
            return (model.User == null) ? false : model.User.Equals(AuthService.CurrentUser);
        }
    }
}
