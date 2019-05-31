// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CommentDialog.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Model;

namespace CodeShare.Uwp.Dialogs
{
    /// <summary>
    /// Class CommentDialog. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CommentDialog
    {
        /// <summary>
        /// Gets the comment.
        /// </summary>
        /// <value>The comment.</value>
        private Comment Comment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDialog"/> class.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public CommentDialog(Comment comment)
        {
            InitializeComponent();
            Comment = comment;
        }
    }
}
