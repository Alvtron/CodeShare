// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeBanner.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CodeBanner.
    /// Implements the <see cref="CodeShare.Model.WebImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.WebImage" />
    public class CodeBanner : WebImage
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
        /// Initializes a new instance of the <see cref="CodeBanner"/> class.
        /// </summary>
        public CodeBanner()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBanner"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <param name="extension">The extension.</param>
        public CodeBanner(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 20d / 8d, fileInBytes, extension)
        {
            
        }
    }
}