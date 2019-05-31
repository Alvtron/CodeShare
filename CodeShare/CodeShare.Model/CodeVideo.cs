// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-17-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-24-2019
// ***********************************************************************
// <copyright file="CodeVideo.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class CodeVideo.
    /// Implements the <see cref="CodeShare.Model.Video" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Video" />
    public class CodeVideo : Video
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public virtual Code Code { get; set; }
        /// <summary>
        /// Gets or sets the code uid.
        /// </summary>
        /// <value>The code uid.</value>
        public Guid? CodeUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeVideo"/> class.
        /// </summary>
        public CodeVideo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeVideo"/> class.
        /// </summary>
        /// <param name="youTubeId">You tube identifier.</param>
        public CodeVideo(string youTubeId)
            : base(youTubeId)
        {
        }
    }
}
