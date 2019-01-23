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
                NavigationService.Navigate(typeof(QuestionPage), question.Uid, question.Title);
        }
    }
}
