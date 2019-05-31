// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-25-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodeLog.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CodeLog.
    /// Implements the <see cref="CodeShare.Model.Log" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Log" />
    public class CodeLog : Log
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public virtual Code Code { get; set; }
        /// <summary>
        /// Gets or sets the code uid.
        /// </summary>
        /// <value>The code uid.</value>
        public Guid? CodeUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeLog"/> class.
        /// </summary>
        public CodeLog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeLog"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="actor">The actor.</param>
        /// <param name="action">The action.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="isPublic">if set to <c>true</c> [is public].</param>
        public CodeLog(IEntity code, IEntity actor, string action, IEntity subject = null, bool isPublic = true)
            : base(actor, action, subject, isPublic)
        {
            CodeUid = code.Uid;
        }
    }
}
