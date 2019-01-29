using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class ReplyBlock : UserControl
    {
        public static readonly DependencyProperty ReplyProperty = DependencyProperty.Register("Reply", typeof(Reply), typeof(ReplyBlock), new PropertyMetadata(default(Reply)));

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

        public Reply Reply
        {
            get => (Reply)GetValue(ReplyProperty);
            set
            {
                if (value == null)
                {
                    return;
                }
                if (value.Replies == null || value.Replies.Count == 0)
                {
                    SetValue(ReplyProperty, RestApiService<Reply>.Get(value.Uid).Result);
                }
                else
                {
                    SetValue(ReplyProperty, value);
                }
            }
        }

        private AddReplyDialog ReplyDialog { get; set; }

        public ReplyBlock()
        {
            InitializeComponent();
        }

        private void NavigateToUser()
        {
            NavigationService.Navigate(typeof(UserPage), Reply.User);
        }

        private async Task UpdateComment()
        {
            if (!await RestApiService<Reply>.Update(Reply, Reply.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong with updating the comment. Try again later.");
                return;
            }
        }

        private async Task Like()
        {
            if (Reply == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }

            Reply.Like(AuthService.CurrentUser);

            await UpdateComment();
        }

        private async Task AddReply()
        {
            if (Reply == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }
            if (ReplyDialog == null)
            {
                ReplyDialog = new AddReplyDialog(Reply);
            }
            
            await ReplyDialog.ShowAsync();

            await UpdateComment();
        }

        private async Task Share()
        {
            if (Reply == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }

            await NotificationService.DisplayErrorMessage("This is not implemented.");

            //await UpdateComment();
        }

        private async Task Report()
        {
            if (Reply == null)
            {
                await NotificationService.DisplayErrorMessage("Developer error.");
                return;
            }

            var reportDialog = new ReportDialog("Reply by " + Reply.User?.Name);

            if (await reportDialog.ShowAsync() != ContentDialogResult.Secondary || !reportDialog.Valid)
            {
                return;
            }
            // TODO: Implement reporting
            //await RestApiService.Report(new Report(Code.Title, MainViewReference.CurrentUser, reportDialog.Message));
            await NotificationService.DisplayThankYouMessage("Thank you for contributing to a nicer and safer community! You rock!");
        }
    }
}
