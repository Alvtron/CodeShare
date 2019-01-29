using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Xaml;
using CodeShare.Uwp.Controls;

namespace CodeShare.Uwp.ViewModels
{
    public class CodeViewModel : ContentViewModel<Code>
    {
        public CodeViewModel(Code code)
            : base(code)
        {
            IsUserAuthor = code.User.Equals(AuthService.CurrentUser);
        }

        public override async Task<bool> Refresh()
        {
            if (!(await RestApiService<Code>.Get(Model.Uid) is Code code)) return false;

            Model = code;
            return true;
        }

        public override void ViewVideo(Video video)
        {
            NavigationService.Navigate(typeof(MediaPage), video, video.Title);
        }

        public override void ViewImage(WebFile image)
        {
            NavigationService.Navigate(typeof(MediaPage), image, image.Path);
        }

        public override async Task ReportAsync()
        {
            if (AuthService.CurrentUser == null || Model == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                return;
            }

            var reportDialog = new ReportDialog(Model?.Name);

            if (await reportDialog.ShowAsync() != ContentDialogResult.Secondary || !reportDialog.Valid)
            {
                return;
            }

            // TODO: Implement reporting
            //await RestApiService.Report(new Report(Code.Title, AuthService.CurrentUser, reportDialog.Message));
            await NotificationService.DisplayThankYouMessage("Thanks for contributing to a nicer and safer community! You rock!");
        }

        public override void LogClick(ILog log)
        {
            throw new NotImplementedException();
        }
    }
}
