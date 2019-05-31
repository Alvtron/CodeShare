// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CommentLog.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CommentLog.
    /// Implements the <see cref="CodeShare.Model.Log" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Log" />
    public class CommentLog : Log
    {
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public virtual Comment Comment { get; set; }
        /// <summary>
        /// Gets or sets the comment uid.
        /// </summary>
        /// <value>The comment uid.</value>
        public Guid? CommentUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentLog"/> class.
        /// </summary>
        public CommentLog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentLog"/> class.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="action">The action.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="isPublic">if set to <c>true</c> [is public].</param>
        public CommentLog(IEntity comment, IEntity actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            CommentUid = comment.Uid;
        }
    }
}
