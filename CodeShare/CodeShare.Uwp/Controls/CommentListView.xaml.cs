// ***********************************************************************
// Assembly         : CodeShare.Uwp
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CommentListView.xaml.cs" company="CodeShare">
//     Copyright Thomas Angeland ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using CodeShare.Model;
using CodeShare.Utilities;

namespace CodeShare.Uwp.Controls
{
    /// <summary>
    /// Class CommentListView. This class cannot be inherited.
    /// Implements the <see cref="Windows.UI.Xaml.Controls.UserControl" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// Implements the <see cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class CommentListView
    {
        /// <summary>
        /// The replies property
        /// </summary>
        public static readonly DependencyProperty RepliesProperty = DependencyProperty.Register("Replies", typeof(ObservableCollection<Comment>), typeof(CommentListView), new PropertyMetadata(new ObservableCollection<Comment>()));

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommentListView"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        private bool Initialized { get; set; }

        /// <summary>
        /// Gets or sets the replies.
        /// </summary>
        /// <value>The replies.</value>
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

                if (Initialized)
                {
                    return;
                }

                InitializeComponent();
                Initialized = true;
            }
        }
    }
}
