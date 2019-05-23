using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class QuestionScreenshotGridView : UserControl
    {
        public static readonly DependencyProperty ScreenshotsProperty = DependencyProperty.Register("Screenshots", typeof(ObservableCollection<QuestionScreenshot>), typeof(QuestionScreenshotGridView), new PropertyMetadata(new ObservableCollection<QuestionScreenshot>()));

        public ObservableCollection<QuestionScreenshot> Screenshots
        {
            get => GetValue(ScreenshotsProperty) as ObservableCollection<QuestionScreenshot>;
            set => SetValue(ScreenshotsProperty, value);
        }

        private RelayCommand<QuestionScreenshot> _deleteScreenshotCommand;
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<QuestionScreenshot>(screenshot => DeleteScreenshot(screenshot));

        public QuestionScreenshotGridView()
        {
            this.InitializeComponent();
        }

        private void DeleteScreenshot(QuestionScreenshot screenshot)
        {
            Screenshots.Remove(screenshot);
        }
    }
}
