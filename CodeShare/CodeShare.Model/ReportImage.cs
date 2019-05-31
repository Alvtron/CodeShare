// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-31-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="ReportImage.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class ReportImage.
    /// Implements the <see cref="CodeShare.Model.WebImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.WebImage" />
    public class ReportImage : WebImage
    {
        /// <summary>
        /// Gets or sets the report.
        /// </summary>
        /// <value>The report.</value>
        public virtual Report Report { get; set; }
        /// <summary>
        /// Gets or sets the report uid.
        /// </summary>
        /// <value>The report uid.</value>
        public Guid? ReportUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportImage"/> class.
        /// </summary>
        public ReportImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportImage"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <param name="extension">The extension.</param>
        public ReportImage(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, fileInBytes, extension)
        {
        }
    }
}