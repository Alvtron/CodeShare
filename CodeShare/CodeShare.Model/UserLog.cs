// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="UserLog.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class UserLog.
    /// Implements the <see cref="CodeShare.Model.Log" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Log" />
    public class UserLog : Log
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }
        /// <summary>
        /// Gets or sets the user uid.
        /// </summary>
        /// <value>The user uid.</value>
        public Guid? UserUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLog"/> class.
        /// </summary>
        public UserLog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLog"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="action">The action.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="isPublic">if set to <c>true</c> [is public].</param>
        public UserLog(IEntity user, IEntity actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            UserUid = user.Uid;
        }
    }
}
