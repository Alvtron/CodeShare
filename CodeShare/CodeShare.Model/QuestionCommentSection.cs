// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="QuestionCommentSection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class QuestionCommentSection.
    /// Implements the <see cref="CodeShare.Model.CommentSection" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.CommentSection" />
    public class QuestionCommentSection : CommentSection
    {
        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        public Question Question { get; set; }
        /// <summary>
        /// Gets or sets the question uid.
        /// </summary>
        /// <value>The question uid.</value>
        public Guid QuestionUid { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionCommentSection"/> class.
        /// </summary>
        public QuestionCommentSection()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionCommentSection"/> class.
        /// </summary>
        /// <param name="questionUid">The question uid.</param>
        public QuestionCommentSection(Guid questionUid)
        {
            QuestionUid = questionUid;
        }
    }
}
