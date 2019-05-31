// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="BindableRichTextBlock.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class BindableRichTextBlock. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class BindableRichTextBlock
    {
        /// <summary>
        /// The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BindableRichTextBlock), new PropertyMetadata(default(string)));
        /// <summary>
        /// The header property
        /// </summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(BindableRichTextBlock), new PropertyMetadata(default(string)));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                return RichTextBlock.Blocks.Aggregate(string.Empty, (current, block) => current + block.ToString());
            }
            set
            {
                RichTextBlock.Blocks.Clear();
                var run = new Run { Text = value };
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(run);
                RichTextBlock.Blocks.Add(paragraph);
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public string Header
        {
            get => HeaderBox.Text;
            set => HeaderBox.Text = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableRichTextBlock"/> class.
        /// </summary>
        public BindableRichTextBlock()
        {
            InitializeComponent();
        }
    }
}
