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
    public sealed partial class CodeAuthorBlock : UserControl
    {
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(Code), typeof(CodeAuthorBlock), new PropertyMetadata(default(Code)));

        public Code Code
        {
            get => GetValue(CodeProperty) as Code;
            set
            {
                SetValue(CodeProperty, value);
                IsUserAuthor = value.UserUid.Equals(AuthService.CurrentUser.Uid);
            }
        }

        private bool IsUserAuthor
        {
            get => EditButton.Visibility == Visibility.Visible; 
            set => EditButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        private RelayCommand _editCommand;
        public ICommand EditCommand => _editCommand = _editCommand ?? new RelayCommand(param => Edit());

        public CodeAuthorBlock()
        {
            this.InitializeComponent();
        }

        public void Edit()
        {
             NavigationService.Navigate(typeof(CodeSettingsPage), Code, $"{Code.Name} Settings");
        }
    }
}
