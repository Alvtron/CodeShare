// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="IPrimaryImage.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Interface IPrimaryImage
    /// Implements the <see cref="CodeShare.Model.IWebImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.IWebImage" />
    public interface IPrimaryImage : IWebImage
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value><c>true</c> if this instance is primary; otherwise, <c>false</c>.</value>
        bool IsPrimary { get; set; }
    }
}