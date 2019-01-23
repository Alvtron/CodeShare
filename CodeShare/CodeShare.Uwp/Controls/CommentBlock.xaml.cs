using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
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
    public sealed partial class CommentBlock : UserControl
    {
        public delegate void CommentChangedHandler();
        public event CommentChangedHandler CommentChanged;

        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentBlock), new PropertyMetadata(new Comment()));

        private RelayCommand _likeCommand;
        public ICommand LikeCommand => _likeCommand = _likeCommand ?? new RelayCommand(async parameter => await Like());

        private RelayCommand _replyCommand;
        public ICommand ReplyCommand => _replyCommand = _replyCommand ?? new RelayCommand(async parameter => await Reply());

        private RelayCommand _shareCommand;
        public ICommand ShareCommand => _shareCommand = _shareCommand ?? new RelayCommand(async parameter => await Share());

        private RelayCommand _reportCommand;
        public ICommand ReportCommand => _reportCommand = _reportCommand ?? new RelayCommand(async parameter => await Report());

        public Comment Comment
        {
            get => (Comment)GetValue(CommentProperty);
            set => SetValue(CommentProperty, value);
        }

        public CommentBlock()
        {
            InitializeComponent();
        }

        public async Task UpdateComment()
        {
            if (!await RestApiService<Comment>.Update(Comment, Comment.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                return;
            }

            CommentChanged();
        }

        public async Task Like()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                throw new NullReferenceException("Comment is null!");
            }

            Comment.Like(AuthService.CurrentUser);

            await UpdateComment();
        }

        public async Task Reply()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                throw new NullReferenceException("Comment is null!");
            }

            throw new NotImplementedException();

            //await UpdateComment();
        }

        public async Task Share()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                throw new NullReferenceException("Comment is null!");
            }

            await NotificationService.DisplayErrorMessage("This is not implemented.");

            await UpdateComment();
        }

        public async Task Report()
        {
            if (Comment == null)
            {
                await NotificationService.DisplayErrorMessage("Something went wrong. Try again later.");
                throw new NullReferenceException("Comment is null!");
            }

            var reportDialog = new ReportDialog("comment by " + Comment.User?.Name);

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
