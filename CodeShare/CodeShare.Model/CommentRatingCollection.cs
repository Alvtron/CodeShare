// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-24-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="CommentRatingCollection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CommentRatingCollection.
    /// Implements the <see cref="CodeShare.Model.RatingCollection" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.RatingCollection" />
    public class CommentRatingCollection : RatingCollection
    {
        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public Comment Comment { get; set; }
        /// <summary>
        /// Gets or sets the comment uid.
        /// </summary>
        /// <value>The comment uid.</value>
        public Guid CommentUid { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRatingCollection"/> class.
        /// </summary>
        public CommentRatingCollection()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRatingCollection"/> class.
        /// </summary>
        /// <param name="commentUid">The comment uid.</param>
        public CommentRatingCollection(Guid commentUid)
        {
            CommentUid = commentUid;
        }
    }
}
