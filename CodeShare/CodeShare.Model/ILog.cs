// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 01-25-2019
// ***********************************************************************
// <copyright file="ILog.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Interface ILog
    /// Implements the <see cref="CodeShare.Model.IEntity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IEntity" />
    public interface ILog : IEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is public.
        /// </summary>
        /// <value><c>true</c> if this instance is public; otherwise, <c>false</c>.</value>
        bool IsPublic { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        string Action { get; set; }
        /// <summary>
        /// Gets or sets the actor uid.
        /// </summary>
        /// <value>The actor uid.</value>
        Guid? ActorUid { get; set; }
        /// <summary>
        /// Gets or sets the type of the actor.
        /// </summary>
        /// <value>The type of the actor.</value>
        string ActorType { get; set; }
        /// <summary>
        /// Gets or sets the subject uid.
        /// </summary>
        /// <value>The subject uid.</value>
        Guid? SubjectUid { get; set; }
        /// <summary>
        /// Gets or sets the type of the subject.
        /// </summary>
        /// <value>The type of the subject.</value>
        string SubjectType { get; set; }
    }
}
