// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Report.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Report.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    public class Report : Entity
    {
        /// <summary>
        /// Gets or sets the target uid.
        /// </summary>
        /// <value>The target uid.</value>
        public Guid? TargetUid { get; set; }
        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>The type of the target.</value>
        public string TargetType { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the image attachments.
        /// </summary>
        /// <value>The image attachments.</value>
        public virtual ICollection<ReportImage> ImageAttachments { get; set; } = new List<ReportImage>();
        /// <summary>
        /// Gets a value indicating whether this <see cref="Report"/> is valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
        [NotMapped, JsonIgnore] public bool Valid => string.IsNullOrWhiteSpace(Message);

        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        public Report()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Report"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">
        /// target
        /// or
        /// message
        /// </exception>
        public Report(IEntity target, string message)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            TargetUid = target.Uid;
            TargetType = target.GetType().Name;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
