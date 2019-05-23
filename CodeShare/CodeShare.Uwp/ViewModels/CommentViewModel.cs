using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.ViewModels
{
    public class CommentViewModel : ObservableObject
    {
        private Comment _comment;
        public Comment Comment
        {
            get => _comment;
            set => SetField(ref _comment, value);
        }
        private RelayCommand _navigateToUserCommand;
        public ICommand NavigateToUserCommand => _navigateToUserCommand = _navigateToUserCommand ?? new RelayCommand(parameter => NavigateToUser());
        private RelayCommand _likeCommand;
        public ICommand LikeCommand => _likeCommand = _likeCommand ?? new RelayCommand(async parameter => await Like());
        private RelayCommand _replyCommand;
        public ICommand ReplyCommand => _replyCommand = _replyCommand ?? new RelayCommand(async parameter => await AddReply());
        private RelayCommand _shareCommand;
        public ICommand ShareCommand => _shareCommand = _shareCommand ?? new RelayCommand(async parameter => await Share());
        private RelayCommand _reportCommand;
        public ICommand ReportCommand => _reportCommand = _reportCommand ?? new RelayCommand(async parameter => await Report());

        private void NavigateToUser()
        {
            if (Comment == null)
            {
                throw new InvalidOperationException("Developer error. Comment is null.");
            }
            NavigationService.Navigate(typeof(UserPage), Comment.User);
        }

        private async Task UpdateComment()
        {
            if (!await RestApiService<Comment>.Update(Comment, Comment.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong with updating the comment. Try again later.");
                return;
            }
        }

        private async Task Like()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }

            Comment.Like(AuthService.CurrentUser);

            await UpdateComment();
        }

        private async Task AddReply()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }
            var replyDialog = new AddCommentDialog($"Reply to {Comment?.User?.Name}");

            if (await replyDialog.ShowAsync() != ContentDialogResult.Primary)
            {
                return;
            }

            var newComment = new Comment(Comment, AuthService.CurrentUser, replyDialog.Text);
            NavigationService.Lock();
            if (!await RestApiService<Comment>.Add(newComment))
            {
                Logger.WriteLine("Failed to upload new comment to REST API.");
                await NotificationService.DisplayErrorMessage("Failed to post comment");
                NavigationService.Unlock();
                return;
            }
            newComment = await RestApiService<Comment>.Get(newComment.Uid);
            if (newComment == null)
            {
                Logger.WriteLine("Failed to retrieve new comment from REST API.");
                await NotificationService.DisplayErrorMessage("The comment was successfully posted, but a page refresh is needed.");
                NavigationService.Unlock();
                return;
            }
            Comment.AddReply(newComment);
            NavigationService.Unlock();
        }

        private async Task Share()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }

            await NotificationService.DisplayErrorMessage("This is not implemented.");
        }

        private async Task Report()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }

            var reportDialog = new ReportDialog($"Reply by {Comment?.User?.Name}");

            var dialogResult = await reportDialog.ShowAsync();

            if (dialogResult != ContentDialogResult.Primary)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(reportDialog.Message))
            {
                await NotificationService.DisplayErrorMessage($"Please provide a reason for why you want to report.");
                return;
            }

            if (!await RestApiService<Report>.Add(new Report(Comment, reportDialog.Message)))
            {
                await NotificationService.DisplayErrorMessage("We where unable to upload that report. Sorry about that. Please try again later.");
                return;
            }

            await NotificationService.DisplayThankYouMessage("Thanks for contributing to a better community for everyone! You rock!");
        }
    }
}
