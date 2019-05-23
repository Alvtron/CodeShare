using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CodeShare.Model;
using CodeShare.Utilities;
using System.Collections.Generic;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CommentListView : UserControl
    {
        public static readonly DependencyProperty RepliesProperty = DependencyProperty.Register("Replies", typeof(ObservableCollection<Comment>), typeof(CommentListView), new PropertyMetadata(new ObservableCollection<Comment>()));

        private bool Initialized { get; set; } = false;

        public ObservableCollection<Comment> Replies
        {
            get => GetValue(RepliesProperty) as ObservableCollection<Comment>;
            set
            {
                if (value == null)
                {
                    Logger.WriteLine($"Failed to initialize '{nameof(Replies)}' in {nameof(CommentListView)}. Value was null.");
                    return;
                }

                SetValue(RepliesProperty, value);
                Logger.WriteLine($"Successfully initialized '{nameof(Replies)}' in {nameof(CommentListView)}.");

                if (!Initialized)
                {
                    InitializeComponent();
                    Initialized = true;
                }
            }
        }
    }
}
