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
using System.Diagnostics;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class QuestionCommentsPanel : UserControl
    {
        public static readonly DependencyProperty QuestionProperty = DependencyProperty.Register("Question", typeof(Question), typeof(QuestionCommentsPanel), new PropertyMetadata(default(Question)));

        private RelayCommand<Editor> _uploadCommand;
        public ICommand UploadCommand => _uploadCommand = _uploadCommand ?? new RelayCommand<Editor>(async editor => await UploadCommentAsync(editor));

        public Question Question
        {
            get => GetValue(QuestionProperty) as Question;
            set => SetValue(QuestionProperty, value);
        }

        public QuestionCommentsPanel()
        {
            InitializeComponent();
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
                await NotificationService.DisplayErrorMessage("Can't post empty comment!");
                return;
            }

            NavigationService.Lock();

            var comment = new Reply(Question.Uid, AuthService.CurrentUser, editor.Rtf);

            Question.Reply(AuthService.CurrentUser, comment);

            if (!await RestApiService<Question>.Update(Question, Question.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong when uploading your comment.");
            }
            else
            {
                editor.Clear();
            }

            NavigationService.Unlock();
        }
    }
}
