// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 05-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-28-2019
// ***********************************************************************
// <copyright file="Comment.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using CodeShare.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Comment. This class cannot be inherited.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.IComment" />
    /// Implements the <see cref="CodeShare.Model.IComment{CodeShare.Model.Comment}" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.IComment" />
    /// <seealso cref="CodeShare.Model.IComment{CodeShare.Model.Comment}" />
    public sealed class Comment : Entity, IComment, IComment<Comment>
    {
        /// <summary>
        /// The text
        /// </summary>
        private string _text;
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }
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
        /// Gets or sets the comment section uid.
        /// </summary>
        /// <value>The comment section uid.</value>
        public Guid? CommentSectionUid { get; set; }
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public Comment Parent { get; set; }
        /// <summary>
        /// Gets or sets the parent uid.
        /// </summary>
        /// <value>The parent uid.</value>
        public Guid? ParentUid { get; set; }
        /// <summary>
        /// Gets or sets the replies.
        /// </summary>
        /// <value>The replies.</value>
        public SortedObservableCollection<Comment> Replies { get; set; } = new SortedObservableCollection<Comment>(c => c.Created, true);
        /// <summary>
        /// Gets or sets the logs.
        /// </summary>
        /// <value>The logs.</value>
        public ObservableCollection<CommentLog> Logs { get; set; } = new ObservableCollection<CommentLog>();
        /// <summary>
        /// Gets or sets the rating collection.
        /// </summary>
        /// <value>The rating collection.</value>
        public CommentRatingCollection RatingCollection { get; set; }
        /// <summary>
        /// Gets or sets the ratings uid.
        /// </summary>
        /// <value>The ratings uid.</value>
        public Guid? RatingsUid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        public Comment()
        {
            RatingCollection = new CommentRatingCollection(Uid);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="text">The text.</param>
        public Comment(User user, string text)
            : this()
        {
            UserUid = user.Uid;
            Text = text;

            Logs.Add(new CommentLog(this, user, "created this"));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="user">The user.</param>
        /// <param name="text">The text.</param>
        public Comment(CommentSection parent, User user, string text)
            : this(user, text)
        {
            CommentSectionUid = parent.Uid;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="user">The user.</param>
        /// <param name="text">The text.</param>
        public Comment(Comment parent, User user, string text)
            : this(user, text)
        {
            ParentUid = parent.Uid;
        }

        /// <summary>
        /// Adds the reply.
        /// </summary>
        /// <param name="reply">The reply.</param>
        public void AddReply(Comment reply)
        {
            if (reply == null)
            {
                Logger.WriteLine($"Failed to add reply as it is a null object.");
                return;
            }
            if (Replies == null)
            {
                Replies = new SortedObservableCollection<Comment>(c => c.Created, true);
            }
            if (Replies.Any(r => r.Equals(reply)))
            {
                Logger.WriteLine($"Failed to add reply {reply.Uid.ToString()} to {Uid.ToString()} as this reply already exists.");
                return;
            }
            Replies.Add(reply);
            Logs.Add(new CommentLog(this, reply.User, "replied", reply));
        }
    }
}
