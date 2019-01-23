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
    public class CodeViewModel : ContentPageViewModel
    {
        private Code _code = new Code();
        public Code Code
        {
            get => _code;
            set
            {
                SetField(ref _code, value);
                IsUserAuthor = value.User.Equals(AuthService.CurrentUser);
            }
        }

        public override async Task<bool> Refresh()
        {
            if (!(await RestApiService<Code>.Get(Code.Uid) is Code code)) return false;

            Code = code;
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
            if (AuthService.CurrentUser == null || Code == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                return;
            }

            var reportDialog = new ReportDialog(Code?.Name);

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
