using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System;
using System.Collections.Generic;
using System.IO;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CodeGridView : UserControl
    {
        public static readonly DependencyProperty CodesProperty = DependencyProperty.Register("Codes", typeof(IEnumerable<Code>), typeof(CodeGridView), new PropertyMetadata(new List<Code>()));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(CodeGridView), new PropertyMetadata(null));

        public IEnumerable<Code> Codes
        {
            get => GetValue(CodesProperty) as IEnumerable<Code>;
            set => SetValue(CodesProperty, value);
        }
        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        public CodeGridView()
        {
            this.InitializeComponent();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Code code)
                NavigationService.Navigate(typeof(CodePage), code.Uid, code.Name);
        }
    }
}
