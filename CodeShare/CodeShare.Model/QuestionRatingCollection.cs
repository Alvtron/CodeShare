// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-24-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="QuestionRatingCollection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class QuestionRatingCollection.
    /// Implements the <see cref="CodeShare.Model.RatingCollection" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.RatingCollection" />
    public class QuestionRatingCollection : RatingCollection
    {
        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        public Question Question { get; set; }
        /// <summary>
        /// Gets or sets the question uid.
        /// </summary>
        /// <value>The question uid.</value>
        public Guid QuestionUid { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionRatingCollection"/> class.
        /// </summary>
        public QuestionRatingCollection()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionRatingCollection"/> class.
        /// </summary>
        /// <param name="questionUid">The question uid.</param>
        public QuestionRatingCollection(Guid questionUid)
        {
            QuestionUid = questionUid;
        }
    }
}
