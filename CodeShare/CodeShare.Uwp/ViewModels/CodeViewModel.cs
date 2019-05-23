using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Dialogs;
using CodeShare.RestApi;
using System.Windows.Input;
using CodeShare.Uwp.Utilities;
using CodeShare.Utilities;

namespace CodeShare.Uwp.ViewModels
{
    public class CodeViewModel : ContentViewModel<Code>
    {
        public CodeViewModel(Code code)
            : base(code)
        {
        }

        public override bool OnSetAuthorPrivileges(Code model)
        {
            return (model.User == null) ? false : model.User.Equals(AuthService.CurrentUser);
        }
    }
}
