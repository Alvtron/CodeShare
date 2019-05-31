using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CodeShare.Model;

namespace CodeShare.Uwp.Controls
{
    public sealed partial class QuestionListViewItem : UserControl
    {
        /// <summary>
        /// The user property
        /// </summary>
        public static readonly DependencyProperty QuestionProperty = DependencyProperty.Register("Question", typeof(Question), typeof(QuestionListViewItem), new PropertyMetadata(new Question()));

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public Question Question
        {
            get => GetValue(QuestionProperty) as Question;
            set => SetValue(QuestionProperty, value);
        }

        public QuestionListViewItem()
        {
            this.InitializeComponent();
        }
    }
}
