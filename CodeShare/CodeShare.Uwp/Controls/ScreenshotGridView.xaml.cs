using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ScreenshotGridView : UserControl
    {
        public static readonly DependencyProperty ScreenshotsProperty = DependencyProperty.Register("Screenshots", typeof(ObservableCollection<Screenshot>), typeof(ScreenshotGridView), new PropertyMetadata(new ObservableCollection<Screenshot>()));

        public ObservableCollection<Screenshot> Screenshots
        {
            get => GetValue(ScreenshotsProperty) as ObservableCollection<Screenshot>;
            set => SetValue(ScreenshotsProperty, value);
        }

        private RelayCommand<Screenshot> _deleteScreenshotCommand;
        public ICommand DeleteScreenshotCommand => _deleteScreenshotCommand = _deleteScreenshotCommand ?? new RelayCommand<Screenshot>(screenshot => DeleteScreenshot(screenshot));

        private void DeleteScreenshot(Screenshot screenshot)
        {
            Screenshots.Remove(screenshot);
        }

        public ScreenshotGridView()
        {
            this.InitializeComponent();
        }
    }
}
