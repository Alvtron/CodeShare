// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-29-2019
// ***********************************************************************
// <copyright file="CodeCommentSection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CodeCommentSection.
    /// Implements the <see cref="CodeShare.Model.CommentSection" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.CommentSection" />
    public class CodeCommentSection : CommentSection
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public Code Code { get; set; }
        /// <summary>
        /// Gets or sets the code uid.
        /// </summary>
        /// <value>The code uid.</value>
        public Guid CodeUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCommentSection"/> class.
        /// </summary>
        public CodeCommentSection()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCommentSection"/> class.
        /// </summary>
        /// <param name="codeUid">The code uid.</param>
        public CodeCommentSection(Guid codeUid)
        {
            CodeUid = codeUid;
        }
    }
}
