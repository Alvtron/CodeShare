using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CodeScreenshotGridView : UserControl
    {
        public static readonly DependencyProperty ScreenshotsProperty = DependencyProperty.Register("Screenshots", typeof(ObservableCollection<CodeScreenshot>), typeof(CodeScreenshotGridView), new PropertyMetadata(new ObservableCollection<CodeScreenshot>()));

        public ObservableCollection<CodeScreenshot> Screenshots
        {
            get => GetValue(ScreenshotsProperty) as ObservableCollection<CodeScreenshot>;
            set => SetValue(ScreenshotsProperty, value);
        }

        private RelayCommand<CodeScreenshot> _deleteScreenshotCommand;
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<CodeScreenshot>(screenshot => DeleteScreenshot(screenshot));

        public CodeScreenshotGridView()
        {
            this.InitializeComponent();
        }

        private void DeleteScreenshot(CodeScreenshot screenshot)
        {
            Screenshots.Remove(screenshot);
        }
    }
}
