// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="Rating.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Rating. This class cannot be inherited.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.IRating" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.IRating" />
    public sealed class Rating : Entity, IRating
    {
        /// <summary>
        /// Gets or sets the ratings uid.
        /// </summary>
        /// <value>The ratings uid.</value>
        public Guid? RatingsUid { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User { get; set; }
        /// <summary>
        /// Gets or sets the user uid.
        /// </summary>
        /// <value>The user uid.</value>
        public Guid? UserUid { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public RatingValue Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        public Rating()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="value">The value.</param>
        private Rating(User user, RatingValue value)
        {
            UserUid = user.Uid;
            Value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        /// <param name="ratingCollection">The rating collection.</param>
        /// <param name="user">The user.</param>
        /// <param name="value">The value.</param>
        public Rating(RatingCollection ratingCollection, User user, RatingValue value)
            : this(user, value)
        {
            RatingsUid = ratingCollection.Uid;
        }

        /// <summary>
        /// Updates the specified rating value.
        /// </summary>
        /// <param name="ratingValue">The rating value.</param>
        public void Update(RatingValue ratingValue)
        {
            Value = Value == ratingValue ? RatingValue.None : ratingValue;
        }
    }
}
