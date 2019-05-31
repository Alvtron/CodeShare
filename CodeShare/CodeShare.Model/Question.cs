// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="Question.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Question. This class cannot be inherited.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.IContent" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.IContent" />
    public sealed class Question : Entity, IContent
    {
        #region Properties
        /// <summary>
        /// The name
        /// </summary>
        private string _name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        /// <summary>
        /// The views
        /// </summary>
        private int _views;
        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        /// <value>The views.</value>
        public int Views
        {
            get => _views;
            set => SetField(ref _views, value);
        }
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
        /// Gets or sets the code language.
        /// </summary>
        /// <value>The code language.</value>
        public CodeLanguage CodeLanguage { get; set; }
        /// <summary>
        /// Gets or sets the code language uid.
        /// </summary>
        /// <value>The code language uid.</value>
        public Guid? CodeLanguageUid { get; set; }
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
        /// Gets or sets the solution.
        /// </summary>
        /// <value>The solution.</value>
        public Comment Solution { get; set; }
        /// <summary>
        /// Gets or sets the solution uid.
        /// </summary>
        /// <value>The solution uid.</value>
        public Guid? SolutionUid { get; set; }
        /// <summary>
        /// Gets or sets the date time solved.
        /// </summary>
        /// <value>The date time solved.</value>
        public DateTime? DateTimeSolved { get; set; }
        /// <summary>
        /// Gets or sets the comment section.
        /// </summary>
        /// <value>The comment section.</value>
        public QuestionCommentSection CommentSection { get; set; }
        /// <summary>
        /// Gets or sets the comment section uid.
        /// </summary>
        /// <value>The comment section uid.</value>
        public Guid? CommentSectionUid { get; set; }
        /// <summary>
        /// Gets or sets the rating collection.
        /// </summary>
        /// <value>The rating collection.</value>
        public QuestionRatingCollection RatingCollection { get; set; }
        /// <summary>
        /// Gets or sets the rating collection uid.
        /// </summary>
        /// <value>The rating collection uid.</value>
        public Guid? RatingCollectionUid { get; set; }
        /// <summary>
        /// Gets or sets the screenshots.
        /// </summary>
        /// <value>The screenshots.</value>
        public SortedObservableCollection<QuestionScreenshot> Screenshots { get; set; } = new SortedObservableCollection<QuestionScreenshot>(f => f.Created, true);
        /// <summary>
        /// Gets or sets the logs.
        /// </summary>
        /// <value>The logs.</value>
        public ObservableCollection<QuestionLog> Logs { get; set; } = new ObservableCollection<QuestionLog>();
        /// <summary>
        /// Gets or sets the videos.
        /// </summary>
        /// <value>The videos.</value>
        public ObservableCollection<QuestionVideo> Videos { get; set; } = new ObservableCollection<QuestionVideo>();
        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        [NotMapped, JsonIgnore] public string Type => GetType().Name;

        #endregion
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Question"/> class.
        /// </summary>
        public Question()
        {
            CommentSection = new QuestionCommentSection(Uid);
            RatingCollection = new QuestionRatingCollection(Uid);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Question"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="codeLanguage">The code language.</param>
        public Question(IEntity user, string title, string text, IEntity codeLanguage)
            : this()
        {
            UserUid = user.Uid;
            Name = title;
            Text = text;
            CodeLanguageUid = codeLanguage.Uid;
            Logs.Add(new QuestionLog(this, user, "created this"));
        }
        #endregion
        #region Methods
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Adds the screenshot.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="screenshot">The screenshot.</param>
        /// <exception cref="ArgumentNullException">screenshot</exception>
        public void AddScreenshot(User user, QuestionScreenshot screenshot)
        {
            Screenshots.Add(screenshot ?? throw new ArgumentNullException(nameof(screenshot)));

            user.IncreaseExperience(Experience.Action.UploadImage);
            Logs.Add(new QuestionLog(this, user, "added", screenshot));
            RefreshBindings();
        }

        /// <summary>
        /// Adds the video.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="video">The video.</param>
        /// <exception cref="NullReferenceException">Video was null.</exception>
        public void AddVideo(User user, QuestionVideo video)
        {
            if (video == null || video.Empty)
            {
                throw new NullReferenceException("Video was null.");
            }
            if (Videos == null)
            {
                Videos = new ObservableCollection<QuestionVideo>();
            }

            Videos.Add(video);

            user.IncreaseExperience(Experience.Action.UploadVideo);
            Logs.Add(new QuestionLog(this, user, "added", video));
        }

        /// <summary>
        /// Replies the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="comment">The comment.</param>
        /// <exception cref="NullReferenceException">Comment was null.</exception>
        public void Reply(User user, Comment comment)
        {
            if (comment == null)
            {
                throw new NullReferenceException("Comment was null.");
            }
            if (CommentSection == null)
            {
                CommentSection = new QuestionCommentSection(Uid);
            }
            if (CommentSection.Replies == null)
            {
                CommentSection.Replies = new SortedObservableCollection<Comment>(c => c.Created, true);
            }

            CommentSection.Replies.Add(comment);

            user.IncreaseExperience(Experience.Action.AddReply);
            Logs.Add(new QuestionLog(this, user, "added", comment));
        }
        #endregion
    }
}
