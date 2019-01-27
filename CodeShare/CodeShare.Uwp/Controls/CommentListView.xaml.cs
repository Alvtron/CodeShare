using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Windows.Input;
using System.Threading.Tasks;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Dialogs;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CommentListView : UserControl
    {
        public static readonly DependencyProperty EntityProperty = DependencyProperty.Register("Entity", typeof(Content), typeof(CommentListView), new PropertyMetadata(default(Content)));

        private RelayCommand<Editor> _uploadCommand;
        public ICommand UploadCommand => _uploadCommand = _uploadCommand ?? new RelayCommand<Editor>(async editor => await UploadCommentAsync(editor));

        public Content Entity
        {
            get => GetValue(EntityProperty) as Content;
            set
            {
                if (!(value is Content content)) return;
                SetValue(EntityProperty, content);
                UpdateCommentList();
            }
        }

        public CommentListView()
        {
            InitializeComponent();
        }

        public void UpdateCommentList()
        {
            CommentList.ItemsSource = Entity.Comments.OrderByDescending(c => c.Created);
        }

        public async Task Refresh()
        {
            for (var index = 0; index < Entity.Comments.Count; index++)
            {
                Entity.Comments[index] = await RestApiService<Comment>.Get(Entity.Comments[index].Uid);
            }
            UpdateCommentList();
        }

        public async Task UploadCommentAsync(Editor editor)
        {
            if (AuthService.CurrentUser == null || !AuthService.CurrentUser.Valid)
            {
                await NotificationService.DisplayErrorMessage("You are not logged in!");
                return;
            }

            if (editor == null || string.IsNullOrWhiteSpace(editor.Rtf))
            {
                await NotificationService.DisplayErrorMessage("Comment is empty");
                return;
            }

            NavigationService.Lock();

            var comment = new Comment(AuthService.CurrentUser, Entity.Uid, editor.Rtf);

            if (!await RestApiService<Comment>.Add(comment))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong when uploading your comment.");
            }
            comment.User = AuthService.CurrentUser;
            Entity.Comments.Add(comment);
            UpdateCommentList();

            NavigationService.Unlock();
        }

        private async void OnCommentChanged()
        {
            await Refresh();
        }
    }
}
