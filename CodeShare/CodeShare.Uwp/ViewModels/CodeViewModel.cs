using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Dialogs;
using CodeShare.RestApi;

namespace CodeShare.Uwp.ViewModels
{
    public class CodeViewModel : ContentViewModel<Code>
    {
        public CodeViewModel(Code code)
            : base(code)
        {
            IsUserAuthor = (code.User == null) ? false : code.User.Equals(AuthService.CurrentUser);
        }
    }
}
