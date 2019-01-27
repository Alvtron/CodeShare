using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
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
    public sealed partial class FileListView : UserControl
    {
        public static readonly DependencyProperty FilesProperty = DependencyProperty.Register("Code", typeof(ICollection<File>), typeof(FileListView), new PropertyMetadata(new List<File>()));

        public ICollection<File> Files
        {
            get => GetValue(FilesProperty) as ICollection<File>;
            set
            {
                SetValue(FilesProperty, value);
                SelectFirstFile();
            }
        }

        public FileListView()
        {
            InitializeComponent();
        }

        public void SelectFirstFile()
        {
            if (Files.Count() == 0) return;

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
