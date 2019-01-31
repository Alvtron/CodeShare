using CodeShare.Model;
using CodeShare.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                Logger.WriteLine(e.Message);
            }
        }
    }
}
