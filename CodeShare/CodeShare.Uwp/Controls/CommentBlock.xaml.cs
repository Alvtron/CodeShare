using CodeShare.Model;
using CodeShare.RestApi;
using CodeShare.Utilities;
using CodeShare.Uwp.Dialogs;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Utilities;
using CodeShare.Uwp.ViewModels;
using CodeShare.Uwp.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class CommentBlock : UserControl
    {
        public bool Initialized { get; private set; }

        public static readonly DependencyProperty ReplyProperty = DependencyProperty.Register("Comment", typeof(Comment), typeof(CommentBlock), new PropertyMetadata(default(Comment)));

        public CommentViewModel ViewModel { get; set; }

        public Comment Comment
        {
            get => ViewModel.Comment;
            set
            {
                if (value == null)
                {
                    return;
                }
                else if (value.Replies == null || value.Replies.Count == 0)
                {
                    ViewModel.Comment = RestApiService<Comment>.Get(value.Uid).Result;
                }
                else
                {
                    ViewModel.Comment = value;
                }
                if (!Initialized)
                {
                    InitializeComponent();
                    Initialized = true;
                }
            }
        }

        public CommentBlock()
        {
            ViewModel = new CommentViewModel();
        }
    }
}
