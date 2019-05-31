// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="Video.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Video.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    public class Video : Entity
    {
        /// <summary>
        /// You tube identifier
        /// </summary>
        private string _youTubeId;
        /// <summary>
        /// Gets or sets you tube identifier.
        /// </summary>
        /// <value>You tube identifier.</value>
        public string YouTubeId
        {
            get => _youTubeId;
            set => SetField(ref _youTubeId, value);
        }

        /// <summary>
        /// The title
        /// </summary>
        private string _title;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        public Video() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        /// <param name="youTubeId">You tube identifier.</param>
        public Video(string youTubeId)
        {
            YouTubeId = youTubeId;
        }

        /// <summary>
        /// Gets you tube URI.
        /// </summary>
        /// <value>You tube URI.</value>
        [NotMapped]
        public Uri YouTubeUri => new Uri(@"https://www.youtube.com/embed/" + YouTubeId);

        /// <summary>
        /// Gets you tube thumbnail.
        /// </summary>
        /// <value>You tube thumbnail.</value>
        [NotMapped]
        public Uri YouTubeThumbnail => new Uri(@"https://img.youtube.com/vi/" + YouTubeId + @"/0.jpg");

        /// <summary>
        /// Gets a value indicating whether this <see cref="Video"/> is empty.
        /// </summary>
        /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool Empty => string.IsNullOrWhiteSpace(YouTubeId);
    }
}
