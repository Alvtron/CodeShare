using System;
using System.Collections.Generic;
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
    public sealed partial class SaveResetPanel : UserControl
    {
        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register("SaveCommand", typeof(ICommand), typeof(SaveResetPanel), new PropertyMetadata(default(ICommand)));
        public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register("ResetCommand", typeof(ICommand), typeof(SaveResetPanel), new PropertyMetadata(default(ICommand)));

        public ICommand SaveCommand
        {
            get => GetValue(SaveCommandProperty) as ICommand;
            set => SetValue(SaveCommandProperty, value);
        }
        public ICommand ResetCommand
        {
            get => GetValue(ResetCommandProperty) as ICommand;
            set => SetValue(ResetCommandProperty, value);
        }

        public SaveResetPanel()
        {
            InitializeComponent();
        }
    }
}
