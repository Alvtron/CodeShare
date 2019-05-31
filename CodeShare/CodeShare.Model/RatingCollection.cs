// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-24-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="RatingCollection.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CodeShare.Utilities;
using Newtonsoft.Json;

namespace CodeShare.Model
{
    /// <summary>
    /// Class RatingCollection.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.ILikeable{CodeShare.Model.Rating}" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.ILikeable{CodeShare.Model.Rating}" />
    public abstract class RatingCollection : Entity, ILikeable<Rating>
    {
        /// <summary>
        /// Occurs when [collection changed].
        /// </summary>
        public event EventHandler CollectionChanged;

        /// <summary>
        /// The ratings
        /// </summary>
        private ObservableCollection<Rating> _ratings;
        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        /// <value>The ratings.</value>
        public ObservableCollection<Rating> Ratings
        {
            get => _ratings;
            set
            {
                SetField(ref _ratings, value);
                OnCollectionChanged();
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has ratings.
        /// </summary>
        /// <value><c>true</c> if this instance has ratings; otherwise, <c>false</c>.</value>
        [NotMapped, JsonIgnore] public bool HasRatings => Ratings.Any();
        /// <summary>
        /// The number of likes
        /// </summary>
        [NotMapped, JsonIgnore] private int _numberOfLikes;
        /// <summary>
        /// Gets the number of likes.
        /// </summary>
        /// <value>The number of likes.</value>
        [NotMapped, JsonIgnore] public int NumberOfLikes
        {
            get => _numberOfLikes;
            private set => SetField(ref _numberOfLikes, value);
        }
        /// <summary>
        /// The number of dislikes
        /// </summary>
        [NotMapped, JsonIgnore] private int _numberOfDislikes;
        /// <summary>
        /// Gets the number of dislikes.
        /// </summary>
        /// <value>The number of dislikes.</value>
        [NotMapped, JsonIgnore] public int NumberOfDislikes
        {
            get => _numberOfDislikes;
            private set => SetField(ref _numberOfDislikes, value);
        }
        /// <summary>
        /// The balanced score
        /// </summary>
        [NotMapped, JsonIgnore] private int _balancedScore;
        /// <summary>
        /// Gets the balanced score.
        /// </summary>
        /// <value>The balanced score.</value>
        [NotMapped, JsonIgnore] public int BalancedScore
        {
            get => _balancedScore;
            private set => SetField(ref _balancedScore, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingCollection"/> class.
        /// </summary>
        protected RatingCollection()
        {
            _ratings = new ObservableCollection<Rating>();
            Ratings.CollectionChanged += (s, e) => OnCollectionChanged();
        }

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        private void OnCollectionChanged()
        {
            NumberOfLikes = Ratings?.Count(r => r.Value == RatingValue.Positive) ?? 0;
            NumberOfDislikes = Ratings?.Count(r => r.Value == RatingValue.Negative) ?? 0;
            BalancedScore = NumberOfLikes - NumberOfDislikes;
            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Ensures the removed.
        /// </summary>
        /// <param name="user">The user.</param>
        private void EnsureRemoved(User user)
        {
            var rating = Ratings.FirstOrDefault(i => i.UserUid.Equals(user.Uid));
            if (rating != null)
            {
                Ratings.Remove(rating);
            }
        }

        /// <summary>
        /// Determines whether the specified user has liked.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user has liked; otherwise, <c>false</c>.</returns>
        public bool HasLiked(User user) => Ratings.Any(r => r.Value == RatingValue.Positive && r.UserUid.HasValue && r.UserUid.Equals(user.Uid));
        /// <summary>
        /// Determines whether the specified user has disliked.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user has disliked; otherwise, <c>false</c>.</returns>
        public bool HasDisliked(User user) => Ratings.Any(r => r.Value == RatingValue.Negative && r.UserUid.HasValue && r.UserUid.Equals(user.Uid));
        /// <summary>
        /// Determines whether the specified user has rated.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user has rated; otherwise, <c>false</c>.</returns>
        public bool HasRated(User user) => Ratings.Any(r => r.UserUid.HasValue && r.UserUid.Equals(user.Uid));

        /// <summary>
        /// Adds the rating.
        /// </summary>
        /// <param name="rating">The rating.</param>
        public void AddRating(Rating rating)
        {
            EnsureRemoved(rating.User);
            Ratings.Add(rating);
        }
    }
}
