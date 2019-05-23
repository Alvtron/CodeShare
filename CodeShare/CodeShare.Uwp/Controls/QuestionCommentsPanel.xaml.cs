using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Windows.Input;
using System.Threading.Tasks;
using CodeShare.Uwp.Services;
using CodeShare.RestApi;

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
            if (AuthService.CurrentUser == null)
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

            var comment = new Comment(Question.CommentSection, AuthService.CurrentUser, editor.Rtf);

            if (!await RestApiService<Comment>.Update(comment, comment.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong when posting your comment.");
                NavigationService.Unlock();
                return;
            }

            Question.Reply(AuthService.CurrentUser, comment);

            if (!await RestApiService<Question>.Update(Question, Question.Uid))
            {
                await NotificationService.DisplayErrorMessage("Something went wrong when posting your comment.");
                NavigationService.Unlock();
                return;
            }

            editor.Clear();
            NavigationService.Unlock();
        }
    }
}
