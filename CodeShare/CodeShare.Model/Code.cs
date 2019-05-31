// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-30-2019
// ***********************************************************************
// <copyright file="Code.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    /// <summary>
    /// Class Code. This class cannot be inherited.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.IContent" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.IContent" />
    public sealed class Code : Entity, IContent
    {
        #region Properties

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
        /// The version
        /// </summary>
        private string _version;
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get => _version;
            set => SetField(ref _version, value);
        }
        /// <summary>
        /// The description
        /// </summary>
        private string _description;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }
        /// <summary>
        /// The about
        /// </summary>
        private string _about;
        /// <summary>
        /// Gets or sets the about.
        /// </summary>
        /// <value>The about.</value>
        public string About
        {
            get => _about;
            set => SetField(ref _about, value);
        }
        /// <summary>
        /// Gets or sets the comment section.
        /// </summary>
        /// <value>The comment section.</value>
        public CodeCommentSection CommentSection { get; set; }
        /// <summary>
        /// Gets or sets the comment section uid.
        /// </summary>
        /// <value>The comment section uid.</value>
        public Guid? CommentSectionUid { get; set; }
        /// <summary>
        /// Gets or sets the rating collection.
        /// </summary>
        /// <value>The rating collection.</value>
        public CodeRatingCollection RatingCollection { get; set; }
        /// <summary>
        /// Gets or sets the rating collection uid.
        /// </summary>
        /// <value>The rating collection uid.</value>
        public Guid? RatingCollectionUid { get; set; }
        /// <summary>
        /// Gets or sets the banner.
        /// </summary>
        /// <value>The banner.</value>
        public CodeBanner Banner { get; set; }
        /// <summary>
        /// Gets or sets the banner uid.
        /// </summary>
        /// <value>The banner uid.</value>
        public Guid? BannerUid { get; set; }
        /// <summary>
        /// Gets or sets the banners.
        /// </summary>
        /// <value>The banners.</value>
        public SortedObservableCollection<CodeBanner> Banners { get; set; } = new SortedObservableCollection<CodeBanner>(f => f.Created, true);
        /// <summary>
        /// Gets or sets the screenshots.
        /// </summary>
        /// <value>The screenshots.</value>
        public SortedObservableCollection<CodeScreenshot> Screenshots { get; set; } = new SortedObservableCollection<CodeScreenshot>(f => f.Created, true);
        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>The files.</value>
        public SortedObservableCollection<CodeFile> Files { get; set; } = new SortedObservableCollection<CodeFile>(f => f.FullName);
        /// <summary>
        /// Gets or sets the logs.
        /// </summary>
        /// <value>The logs.</value>
        public ObservableCollection<CodeLog> Logs { get; set; } = new ObservableCollection<CodeLog>();
        /// <summary>
        /// Gets or sets the videos.
        /// </summary>
        /// <value>The videos.</value>
        public ObservableCollection<CodeVideo> Videos { get; set; } = new ObservableCollection<CodeVideo>();
        /// <summary>
        /// Gets a value indicating whether this <see cref="Code"/> is valid.
        /// </summary>
        /// <value><c>true</c> if valid; otherwise, <c>false</c>.</value>
        [NotMapped, JsonIgnore] public bool Valid => Uid != Guid.Empty && !string.IsNullOrWhiteSpace(Name);
        /// <summary>
        /// Gets a value indicating whether this instance has banner.
        /// </summary>
        /// <value><c>true</c> if this instance has banner; otherwise, <c>false</c>.</value>
        [NotMapped, JsonIgnore] public bool HasBanner => Banner != null;
        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        [NotMapped, JsonIgnore] public string Type => GetType().Name;

        #endregion
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Code"/> class.
        /// </summary>
        public Code()
        {
            CommentSection = new CodeCommentSection(Uid);
            RatingCollection = new CodeRatingCollection(Uid);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Code"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="files">The files.</param>
        /// <param name="author">The author.</param>
        public Code(string name, IEnumerable<CodeFile> files, User author)
            : this()
        {
            Name = name;
            Files = new SortedObservableCollection<CodeFile>(f => f.FullName);
            foreach (var file in files)
            {
                Files.Add(file);
            }
            UserUid = author.Uid;
            Logs.Add(new CodeLog(this, author, "created this"));
        }

        #endregion
        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="user">The user.</param>
        /// <exception cref="ArgumentNullException">
        /// file
        /// or
        /// user
        /// </exception>
        public void AddFile(CodeFile file, User user)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (Files == null)
            {
                Files = new SortedObservableCollection<CodeFile>(f => f.FullName);
            }

            Files.Add(file);
            user.IncreaseExperience(Experience.Action.UploadFile);
            Logs.Add(new CodeLog(this, user, "added", file));
        }

        /// <summary>
        /// Sets the banner.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="banner">The banner.</param>
        /// <exception cref="ArgumentNullException">banner</exception>
        public void SetBanner(User user, CodeBanner banner)
        {
            Banner = banner ?? throw new ArgumentNullException(nameof(banner));
            BannerUid = banner.Uid;
            Banners.Add(banner);

            user.IncreaseExperience(Experience.Action.UploadImage);
            Logs.Add(new CodeLog(this, user, "uploaded", banner));
            RefreshBindings();
        }

        /// <summary>
        /// Adds the screenshot.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="screenshot">The screenshot.</param>
        /// <exception cref="ArgumentNullException">screenshot</exception>
        public void AddScreenshot(User user, CodeScreenshot screenshot)
        {
            Screenshots.Add(screenshot ?? throw new ArgumentNullException(nameof(screenshot)));

            user.IncreaseExperience(Experience.Action.UploadImage);
            Logs.Add(new CodeLog(this, user, "added", screenshot));
            RefreshBindings();
        }

        /// <summary>
        /// Adds the video.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="video">The video.</param>
        /// <exception cref="ArgumentNullException">video</exception>
        public void AddVideo(User user, CodeVideo video)
        {
            if (video == null || video.Empty)
            {
                throw new ArgumentNullException(nameof(video));
            }

            Videos.Add(video);

            user.IncreaseExperience(Experience.Action.UploadVideo);
            Logs.Add(new CodeLog(this, user, "added", video));
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
                CommentSection = new CodeCommentSection(Uid);
            }
            if (CommentSection.Replies == null)
            {
                CommentSection.Replies = new SortedObservableCollection<Comment>(c => c.Created, true);
            }

            CommentSection.Replies.Add(comment);

            user.IncreaseExperience(Experience.Action.AddReply);
            Logs.Add(new CodeLog(this, user, "added", comment));
        }

        #endregion
    }
}
