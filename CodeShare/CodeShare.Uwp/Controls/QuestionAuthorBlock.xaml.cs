using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.Views;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class QuestionAuthorBlock : UserControl
    {
        public static readonly DependencyProperty QuestionProperty = DependencyProperty.Register("Question", typeof(Question), typeof(QuestionAuthorBlock), new PropertyMetadata(default(Question)));

        public Question Question
        {
            get => GetValue(QuestionProperty) as Question;
            set
            {
                SetValue(QuestionProperty, value);

                IsUserAuthor = AuthService.CurrentUser == null
                    ? IsUserAuthor = false
                    : value.UserUid.Equals(AuthService.CurrentUser.Uid);
            }
        }

        private bool IsUserAuthor
        {
            get => EditButton.Visibility == Visibility.Visible; 
            set => EditButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        private RelayCommand _editCommand;
        public ICommand EditCommand => _editCommand = _editCommand ?? new RelayCommand(param => Edit());

        public QuestionAuthorBlock()
        {
            this.InitializeComponent();
        }

        public void Edit()
        {
             NavigationService.Navigate(typeof(QuestionSettingsPage), Question, $"{Question.Name} Settings");
        }
    }
}
