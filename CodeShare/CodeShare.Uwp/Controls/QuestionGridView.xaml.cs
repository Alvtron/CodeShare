// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="QuestionGridView.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using CodeShare.Uwp.Services;
using CodeShare.Uwp.Views;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class QuestionGridView. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class QuestionGridView
    {
        /// <summary>
        /// The questions property
        /// </summary>
        public static readonly DependencyProperty QuestionsProperty = DependencyProperty.Register("Questions", typeof(IEnumerable<Question>), typeof(QuestionGridView), new PropertyMetadata(new List<Question>()));
        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(QuestionGridView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>The questions.</value>
        public IEnumerable<Question> Questions
        {
            get => GetValue(QuestionsProperty) as IEnumerable<Question>;
            set => SetValue(QuestionsProperty, value);
        }
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get => GetValue(HeaderProperty) as string;
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionGridView"/> class.
        /// </summary>
        public QuestionGridView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the ItemClick event of the GridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        public void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Question question)
                NavigationService.Navigate(typeof(QuestionPage), question.Uid, question.Name);
        }
    }
}
