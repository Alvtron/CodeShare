// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-22-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="ILikeable.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeShare.Model
{
    /// <summary>
    /// Interface ILikeable
    /// Implements the <see cref="CodeShare.Model.IEntity" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="CodeShare.Model.IEntity" />
    public interface ILikeable<T> : IEntity where T : IRating, new()
    {
        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        /// <value>The ratings.</value>
        ObservableCollection<Rating> Ratings { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance has ratings.
        /// </summary>
        /// <value><c>true</c> if this instance has ratings; otherwise, <c>false</c>.</value>
        bool HasRatings { get; }
        /// <summary>
        /// Gets the number of likes.
        /// </summary>
        /// <value>The number of likes.</value>
        int NumberOfLikes { get; }
        /// <summary>
        /// Gets the number of dislikes.
        /// </summary>
        /// <value>The number of dislikes.</value>
        int NumberOfDislikes { get; }
        /// <summary>
        /// Determines whether the specified user has liked.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user has liked; otherwise, <c>false</c>.</returns>
        bool HasLiked(User user);
        /// <summary>
        /// Determines whether the specified user has disliked.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user has disliked; otherwise, <c>false</c>.</returns>
        bool HasDisliked(User user);
        /// <summary>
        /// Determines whether the specified user has rated.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user has rated; otherwise, <c>false</c>.</returns>
        bool HasRated(User user);
        /// <summary>
        /// Adds the rating.
        /// </summary>
        /// <param name="rating">The rating.</param>
        void AddRating(Rating rating);
    }
}