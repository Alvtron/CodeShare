using CodeShare.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class EditFileDialog : ContentDialog
    {
        public File File { get; set; }
        private string OriginalFileData { get; set; }

        public EditFileDialog(File file)
        {
            File = file;
            OriginalFileData = file.Data;
            Title = $"Edit {file}";
            InitializeComponent();
        }

        private void Discard_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            File.Data = OriginalFileData;
        }

        private void Save_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
