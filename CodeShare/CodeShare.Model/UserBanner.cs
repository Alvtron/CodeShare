// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="UserBanner.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    /// <summary>
    /// Class UserBanner.
    /// Implements the <see cref="CodeShare.Model.WebImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.WebImage" />
    public class UserBanner : WebImage
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
        /// Initializes a new instance of the <see cref="UserBanner"/> class.
        /// </summary>
        public UserBanner()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBanner"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <param name="extension">The extension.</param>
        public UserBanner(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 20d / 8d, fileInBytes, extension)
        {
            
        }
    }
}