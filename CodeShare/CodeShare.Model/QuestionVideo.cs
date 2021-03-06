﻿// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-17-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="QuestionVideo.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class QuestionVideo.
    /// Implements the <see cref="CodeShare.Model.Video" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Video" />
    public class QuestionVideo : Video
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
        /// Initializes a new instance of the <see cref="QuestionVideo"/> class.
        /// </summary>
        public QuestionVideo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionVideo"/> class.
        /// </summary>
        /// <param name="youTubeId">You tube identifier.</param>
        public QuestionVideo(string youTubeId)
            : base(youTubeId)
        {
        }
    }
}
