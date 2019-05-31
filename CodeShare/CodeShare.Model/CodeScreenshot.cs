// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="CodeScreenshot.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CodeScreenshot.
    /// Implements the <see cref="CodeShare.Model.WebImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.WebImage" />
    public class CodeScreenshot : WebImage
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
        /// Initializes a new instance of the <see cref="CodeScreenshot"/> class.
        /// </summary>
        public CodeScreenshot()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeScreenshot"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <param name="extension">The extension.</param>
        public CodeScreenshot(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 16d / 9d, fileInBytes, extension)
        {
        }
    }
}