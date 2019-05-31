// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-19-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Notification.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Notification.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    public class Notification : Entity
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User { get; set; }
        /// <summary>
        /// Gets or sets the user uid.
        /// </summary>
        /// <value>The user uid.</value>
        public Guid? UserUid { get; set; }
        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>The log.</value>
        public Log Log { get; set; }
        /// <summary>
        /// Gets or sets the log uid.
        /// </summary>
        /// <value>The log uid.</value>
        public Guid? LogUid { get; set; }
        /// <summary>
        /// Gets or sets the seen.
        /// </summary>
        /// <value>The seen.</value>
        public DateTime Seen { get; set; }
    }
}
