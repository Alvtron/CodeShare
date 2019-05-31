// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-25-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="QuestionLog.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class QuestionLog.
    /// Implements the <see cref="CodeShare.Model.Log" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Log" />
    public class QuestionLog : Log
    {
        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        public virtual Question Question { get; set; }
        /// <summary>
        /// Gets or sets the question uid.
        /// </summary>
        /// <value>The question uid.</value>
        public Guid? QuestionUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionLog"/> class.
        /// </summary>
        public QuestionLog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionLog"/> class.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="action">The action.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="isPublic">if set to <c>true</c> [is public].</param>
        public QuestionLog(IEntity question, IEntity actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            QuestionUid = question.Uid;
        }
    }
}
