using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CodeShare.Model;
using CodeShare.Uwp.Utilities;
using System.Windows.Input;
using System.Threading.Tasks;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.DataSource;
using CodeShare.Uwp.Dialogs;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class ReplyListView : UserControl
    {
        public static readonly DependencyProperty RepliesProperty = DependencyProperty.Register("Replies", typeof(ObservableCollection<Reply>), typeof(ReplyListView), new PropertyMetadata(new ObservableCollection<Reply>()));

        private bool Initialized { get; set; } = false;

        public ObservableCollection<Reply> Replies
        {
            get => GetValue(RepliesProperty) as ObservableCollection<Reply>;
            set
            {
                if (value == null)
                {
                    return;
                }

                SetValue(RepliesProperty, value);

                if (!Initialized)
                {
                    InitializeComponent();
                    Initialized = true;
                }
            }
        }
    }
}
