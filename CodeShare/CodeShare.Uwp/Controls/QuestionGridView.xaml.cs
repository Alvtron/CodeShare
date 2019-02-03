using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodeShare.Uwp.Controls
{
    public sealed partial class QuestionGridView : UserControl
    {
        public static readonly DependencyProperty QuestionsProperty = DependencyProperty.Register("Questions", typeof(IEnumerable<Question>), typeof(QuestionGridView), new PropertyMetadata(new List<Question>()));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(QuestionGridView), new PropertyMetadata(null));

        public IEnumerable<Question> Questions
        {
            get => GetValue(QuestionsProperty) as IEnumerable<Question>;
            set => SetValue(QuestionsProperty, value);
        }
        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        public QuestionGridView()
        {
            this.InitializeComponent();
        }

        public void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Question question)
                NavigationService.Navigate(typeof(QuestionPage), question.Uid, question.Name);
        }
    }
}
