// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-22-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="IComment.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Interface IComment
    /// Implements the <see cref="CodeShare.Model.IEntity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IEntity" />
    public interface IComment : IEntity
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        User User { get; set; }
        /// <summary>
        /// Gets or sets the user uid.
        /// </summary>
        /// <value>The user uid.</value>
        Guid? UserUid { get; set; }
    }

    /// <summary>
    /// Interface IComment
    /// Implements the <see cref="CodeShare.Model.IEntity" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="CodeShare.Model.IEntity" />
    public interface IComment<T> : IEntity where T : IComment
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        string Text { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        User User { get; set; }
        /// <summary>
        /// Gets or sets the user uid.
        /// </summary>
        /// <value>The user uid.</value>
        Guid? UserUid { get; set; }
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        T Parent { get; set; }
        /// <summary>
        /// Gets or sets the parent uid.
        /// </summary>
        /// <value>The parent uid.</value>
        Guid? ParentUid { get; set; }
        /// <summary>
        /// Gets or sets the replies.
        /// </summary>
        /// <value>The replies.</value>
        SortedObservableCollection<T> Replies { get; set; }
    }
}