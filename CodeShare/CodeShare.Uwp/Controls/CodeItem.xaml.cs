using CodeShare.Model;
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
    public sealed partial class CodeItem : UserControl
    {
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(Code), typeof(CodeItem), new PropertyMetadata(new Code()));

        public Code Code
        {
            get => GetValue(CodeProperty) as Code;
            set => SetValue(CodeProperty, value);
        }

        public CodeItem()
        {
            this.InitializeComponent();
        }
    }
}
