// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="IRating.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Interface IRating
    /// Implements the <see cref="CodeShare.Model.IEntity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IEntity" />
    public interface IRating : IEntity
    {
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
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        RatingValue Value { get; set; }
    }
}
