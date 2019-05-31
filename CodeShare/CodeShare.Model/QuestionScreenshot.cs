// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-30-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="QuestionScreenshot.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    /// <summary>
    /// Class QuestionScreenshot.
    /// Implements the <see cref="CodeShare.Model.WebImage" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.WebImage" />
    public class QuestionScreenshot : WebImage
    {
        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        public virtual Question Question { get; set; }
        /// <summary>
        /// Gets or sets the question uid.
        /// </summary>
        /// <value>The question uid.</value>
        public Guid? QuestionUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionScreenshot"/> class.
        /// </summary>
        public QuestionScreenshot()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionScreenshot"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="fileInBytes">The file in bytes.</param>
        /// <param name="extension">The extension.</param>
        public QuestionScreenshot(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 16 / 9, fileInBytes, extension)
        {
        }
    }
}