using CodeShare.Model;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public sealed partial class CodeFileView : UserControl
    {
        public static readonly DependencyProperty IsDeletionAllowedProperty = DependencyProperty.Register("IsDeletionAllowed", typeof(bool), typeof(CodeFileView), new PropertyMetadata(false));

        public static readonly DependencyProperty FilesProperty = DependencyProperty.Register("Code", typeof(IEnumerable<CodeShare.Model.File>), typeof(CodeFileView), new PropertyMetadata(new List<CodeShare.Model.File>()));

        private RelayCommand<CodeShare.Model.File> _deleteFileCommand;
        public ICommand DeleteFileCommand => _deleteFileCommand = _deleteFileCommand ?? new RelayCommand<CodeShare.Model.File>(file => DeleteFile(file));

        public bool IsDeletionAllowed
        {
            get => (bool)GetValue(IsDeletionAllowedProperty);
            set => SetValue(IsDeletionAllowedProperty, value);
        }

        public IEnumerable<CodeShare.Model.File> Files
        {
            get => GetValue(FilesProperty) as IEnumerable<CodeShare.Model.File>;
            set
            {
                SetValue(FilesProperty, value);
                UpdateFileListHeader();
                SelectFirstFile();
            }
        }

        public CodeFileView()
        {
            InitializeComponent();
        }

        private void DeleteFile(CodeShare.Model.File file)
        {
            if (file == null) return;

            Files = Files.Where(f => f.Uid != file.Uid);
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

        public void UpdateFileListHeader()
        {
            switch (Files.Count())
            {
                case 1:
                    ListHeader.Text = "1 file";
                    break;
                case 2:
                    ListHeader.Text = $"{Files.Count()} files";
                    break;
                default:
                    ListHeader.Text = "No files";
                    break;
            }
        }
    }
}
