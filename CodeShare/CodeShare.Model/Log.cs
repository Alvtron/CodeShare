// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-25-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Log.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Log.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.ILog" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.ILog" />
    public abstract class Log : Entity, ILog
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is public.
        /// </summary>
        /// <value><c>true</c> if this instance is public; otherwise, <c>false</c>.</value>
        public bool IsPublic { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }
        /// <summary>
        /// Gets or sets the actor uid.
        /// </summary>
        /// <value>The actor uid.</value>
        public Guid? ActorUid { get; set; }
        /// <summary>
        /// Gets or sets the type of the actor.
        /// </summary>
        /// <value>The type of the actor.</value>
        public string ActorType { get; set; }
        /// <summary>
        /// Gets or sets the subject uid.
        /// </summary>
        /// <value>The subject uid.</value>
        public Guid? SubjectUid { get; set; }
        /// <summary>
        /// Gets or sets the type of the subject.
        /// </summary>
        /// <value>The type of the subject.</value>
        public string SubjectType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        protected Log()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="action">The action.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="isPublic">if set to <c>true</c> [is public].</param>
        protected Log(IEntity actor, string action, IEntity subject = null, bool isPublic = true)
        {
            Action = action;
            ActorUid = actor?.Uid;
            ActorType = actor?.GetType().Name;
            SubjectUid = subject?.Uid;
            SubjectType = subject?.GetType().Name;
            IsPublic = isPublic;
        }
    }
}
