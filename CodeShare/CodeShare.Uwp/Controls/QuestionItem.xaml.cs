// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="QuestionItem.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;
using Windows.UI.Xaml;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class QuestionItem. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class QuestionItem
    {
        /// <summary>
        /// The question property
        /// </summary>
        public static readonly DependencyProperty QuestionProperty = DependencyProperty.Register("Question", typeof(Question), typeof(QuestionItem), new PropertyMetadata(new Question()));

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        public Question Question
        {
            get => GetValue(QuestionProperty) as Question;
            set => SetValue(QuestionProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionItem"/> class.
        /// </summary>
        public QuestionItem()
        {
            InitializeComponent();
        }
    }
}
