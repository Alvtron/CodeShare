using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CodeShare.Uwp.Dialogs
{
    public sealed partial class LoadingDialog : ContentDialog
    {
        private DispatcherTimer timer;

        public LoadingDialog(int timeInSeconds)
        {
            InitializeComponent();
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, timeInSeconds)
            };

            timer.Tick += (s, e) => Hide();
        }

        public LoadingDialog()
        {
            InitializeComponent();
        }

        public void Close()
        {
            Hide();
        }

        private void ProgressRing_Loaded(object sender, RoutedEventArgs e)
        {
            if (timer != null) timer.Start();
        }
    }
}
