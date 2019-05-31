// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-15-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="Friendship.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Friendship. This class cannot be inherited.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    public sealed class Friendship : Entity
    {
        /// <summary>
        /// Gets or sets the requester uid.
        /// </summary>
        /// <value>The requester uid.</value>
        public Guid RequesterUid { get; set; }
        /// <summary>
        /// Gets or sets the requester.
        /// </summary>
        /// <value>The requester.</value>
        public User Requester { get; set; }
        /// <summary>
        /// Gets or sets the confirmer uid.
        /// </summary>
        /// <value>The confirmer uid.</value>
        public Guid ConfirmerUid { get; set; }
        /// <summary>
        /// Gets or sets the confirmer.
        /// </summary>
        /// <value>The confirmer.</value>
        public User Confirmer { get; set; }
        /// <summary>
        /// Gets or sets the confirmed.
        /// </summary>
        /// <value>The confirmed.</value>
        public DateTime? Confirmed { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Friendship"/> class.
        /// </summary>
        public Friendship() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Friendship"/> class.
        /// </summary>
        /// <param name="requester">The requester.</param>
        /// <param name="confirmer">The confirmer.</param>
        /// <exception cref="ArgumentNullException">
        /// requester
        /// or
        /// confirmer
        /// </exception>
        /// <exception cref="ArgumentException">requester and confirmer are the same user.</exception>
        public Friendship(User requester, User confirmer)
        {
            if (requester == null)
            {
                throw new ArgumentNullException(nameof(requester));
            }
            if (confirmer == null)
            {
                throw new ArgumentNullException(nameof(confirmer));
            }
            if (requester.Equals(confirmer))
            {
                throw new ArgumentException("requester and confirmer are the same user.");
            }

            RequesterUid = requester.Uid;
            ConfirmerUid = confirmer.Uid;
        }

        /// <summary>
        /// Accepts this instance.
        /// </summary>
        public void Accept()
        {
            Confirmed = DateTime.Now;
        }
    }
}
