using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class CodeFileListView : UserControl
    {
        public static readonly DependencyProperty FilesProperty = DependencyProperty.Register("CodeFiles", typeof(ICollection<CodeFile>), typeof(CodeFileListView), new PropertyMetadata(new ObservableCollection<CodeFile>()));

        public ObservableCollection<CodeFile> CodeFiles
        {
            get => GetValue(FilesProperty) as ObservableCollection<CodeFile>;
            set
            {
                value.OrderBy(f => f.Name);
                SetValue(FilesProperty, value);
                SelectFirstFile();
            }
        }

        public CodeFileListView()
        {
            InitializeComponent();
        }

        public void SelectFirstFile()
        {
            if (CodeFiles.Count() == 0) return;

            try
            {
                FilesList.SelectedIndex = 0;
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
