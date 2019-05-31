// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CommentSection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Class CommentSection.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.ICommentSection" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.ICommentSection" />
    public abstract class CommentSection : Entity, ICommentSection
    {
        /// <summary>
        /// Gets or sets the replies.
        /// </summary>
        /// <value>The replies.</value>
        public virtual SortedObservableCollection<Comment> Replies { get; set; } = new SortedObservableCollection<Comment>(c => c.Created, true);
    }
}
