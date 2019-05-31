// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-26-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Syntax.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace CodeShare.Model
{
    /// <summary>
    /// Class Syntax.
    /// </summary>
    public class Syntax
    {
        /// <summary>
        /// Gets or sets the delimiter.
        /// </summary>
        /// <value>The delimiter.</value>
        public string Delimiter { get; set; }
        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        public string Keywords { get; set; }
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Syntax"/> class.
        /// </summary>
        public Syntax() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Syntax"/> class.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="comments">The comments.</param>
        public Syntax(string delimiter, string keywords, string comments)
        {
            Delimiter = delimiter ?? "";
            Keywords = keywords ?? "";
            Comments = comments ?? "";
        }
    }
}
