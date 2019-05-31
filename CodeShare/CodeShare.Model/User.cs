// ***********************************************************************
// Assembly         : CodeShare.Model
// Author           : Thomas Angeland
// Created          : 01-23-2019
//
// Last Modified By : Thomas Angeland
// Last Modified On : 05-31-2019
// ***********************************************************************
// <copyright file="User.cs" company="CodeShare.Model">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CodeShare.Utilities;

namespace CodeShare.Model
{
    /// <summary>
    /// Class User. This class cannot be inherited.
    /// Implements the <see cref="CodeShare.Model.Entity" />
    /// Implements the <see cref="CodeShare.Model.IAccount" />
    /// Implements the <see cref="CodeShare.Model.IContent" />
    /// </summary>
    /// <seealso cref="CodeShare.Model.Entity" />
    /// <seealso cref="CodeShare.Model.IAccount" />
    /// <seealso cref="CodeShare.Model.IContent" />
    public sealed class User : Entity, IAccount, IContent
    {
        #region Properties
        /// <summary>
        /// The email
        /// </summary>
        private Email _email;
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public Email Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }
        /// <summary>
        /// The password
        /// </summary>
        private Password _password = new Password();
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public Password Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }
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
        /// The first name
        /// </summary>
        private string _firstName;
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get => _firstName;
            set => SetField(ref _firstName, value);
        }
        /// <summary>
        /// The last name
        /// </summary>
        private string _lastName;
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get => _lastName;
            set => SetField(ref _lastName, value);
        }
        /// <summary>
        /// The birthday
        /// </summary>
        private DateTime? _birthday;
        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        /// <value>The birthday.</value>
        public DateTime? Birthday
        {
            get => _birthday;
            set => SetField(ref _birthday, value);
        }
        /// <summary>
        /// The gender
        /// </summary>
        private string _gender;
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public string Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
        }
        /// <summary>
        /// The country
        /// </summary>
        private string _country;
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country
        {
            get => _country;
            set => SetField(ref _country, value);
        }
        /// <summary>
        /// The bio
        /// </summary>
        private string _bio;
        /// <summary>
        /// Gets or sets the bio.
        /// </summary>
        /// <value>The bio.</value>
        public string Bio
        {
            get => _bio;
            set => SetField(ref _bio, value);
        }
        /// <summary>
        /// The subscribed
        /// </summary>
        private bool _subscribed;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="User"/> is subscribed.
        /// </summary>
        /// <value><c>true</c> if subscribed; otherwise, <c>false</c>.</value>
        public bool Subscribed
        {
            get => _subscribed;
            set => SetField(ref _subscribed, value);
        }
        /// <summary>
        /// The website
        /// </summary>
        private string _website;
        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>The website.</value>
        public string Website
        {
            get => _website;
            set => SetField(ref _website, value);
        }
        /// <summary>
        /// The signed in
        /// </summary>
        private DateTime? _signedIn = DateTime.Now;
        /// <summary>
        /// Gets or sets the signed in.
        /// </summary>
        /// <value>The signed in.</value>
        public DateTime? SignedIn
        {
            get => _signedIn;
            set => SetField(ref _signedIn, value);
        }
        /// <summary>
        /// The signed out
        /// </summary>
        private DateTime? _signedOut;
        /// <summary>
        /// Gets or sets the signed out.
        /// </summary>
        /// <value>The signed out.</value>
        public DateTime? SignedOut
        {
            get => _signedOut;
            set => SetField(ref _signedOut, value);
        }
        /// <summary>
        /// The color
        /// </summary>
        private Color _color = new Color();
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get => _color;
            set => SetField(ref _color, value);
        }
        /// <summary>
        /// The experience
        /// </summary>
        private int _experience;
        /// <summary>
        /// Gets or sets the experience.
        /// </summary>
        /// <value>The experience.</value>
        public int Experience
        {
            get => _experience;
            set => SetField(ref _experience, value);
        }
        /// <summary>
        /// Gets or sets the banner.
        /// </summary>
        /// <value>The banner.</value>
        public UserBanner Banner { get; set; }
        /// <summary>
        /// Gets or sets the banner uid.
        /// </summary>
        /// <value>The banner uid.</value>
        public Guid? BannerUid { get; set; }
        /// <summary>
        /// Gets or sets the banners.
        /// </summary>
        /// <value>The banners.</value>
        public SortedObservableCollection<UserBanner> Banners { get; set; } = new SortedObservableCollection<UserBanner>(f => f.Created, true);
        /// <summary>
        /// Gets or sets the avatar.
        /// </summary>
        /// <value>The avatar.</value>
        public UserAvatar Avatar { get; set; }
        /// <summary>
        /// Gets or sets the avatar uid.
        /// </summary>
        /// <value>The avatar uid.</value>
        public Guid? AvatarUid { get; set; }
        /// <summary>
        /// Gets or sets the avatars.
        /// </summary>
        /// <value>The avatars.</value>
        public SortedObservableCollection<UserAvatar> Avatars { get; set; } = new SortedObservableCollection<UserAvatar>(f => f.Created, true);
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public ICollection<Comment> Comments { get; set; }
        /// <summary>
        /// Gets or sets the codes.
        /// </summary>
        /// <value>The codes.</value>
        public ICollection<Code> Codes { get; set; }
        /// <summary>
        /// Gets or sets the questions.
        /// </summary>
        /// <value>The questions.</value>
        public ICollection<Question> Questions { get; set; }
        /// <summary>
        /// Gets or sets the ratings.
        /// </summary>
        /// <value>The ratings.</value>
        public ICollection<Rating> Ratings { get; set; }
        /// <summary>
        /// Gets or sets the logs.
        /// </summary>
        /// <value>The logs.</value>
        public ICollection<UserLog> Logs { get; set; } = new ObservableCollection<UserLog>();
        /// <summary>
        /// Gets or sets the sent friend requests.
        /// </summary>
        /// <value>The sent friend requests.</value>
        public ObservableCollection<Friendship> SentFriendRequests { get; set; } = new ObservableCollection<Friendship>();
        /// <summary>
        /// Gets or sets the received friend requests.
        /// </summary>
        /// <value>The received friend requests.</value>
        public ObservableCollection<Friendship> ReceivedFriendRequests { get; set; } = new ObservableCollection<Friendship>();
        /// <summary>
        /// Gets the friends.
        /// </summary>
        /// <value>The friends.</value>
        [NotMapped, JsonIgnore] public IEnumerable<User> Friends
        {
            get
            {
                return SentFriendRequests.Where(x => x.Confirmed != null).Select(f => f.Confirmer)
                    .Concat(ReceivedFriendRequests.Where(x => x.Confirmed != null).Select(f => f.Requester));
            }
        }
        /// <summary>
        /// Gets the website URI.
        /// </summary>
        /// <value>The website URI.</value>
        [NotMapped, JsonIgnore] public Uri WebsiteUri => Uri.TryCreate(Website, UriKind.Absolute, out var uri) ? uri : null;
        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        [NotMapped, JsonIgnore] public bool IsValid => Validate.UserName(Name) == ValidationResponse.Valid && Validate.Email(Email.Address) == ValidationResponse.Valid && Password != null;
        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        [NotMapped, JsonIgnore] public string FullName => $"{FirstName} {LastName}";
        /// <summary>
        /// Gets the initials.
        /// </summary>
        /// <value>The initials.</value>
        [NotMapped, JsonIgnore] public string Initials => !string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName) ? FirstName?.Substring(0, 1) + LastName?.Substring(0, 1) : "";
        /// <summary>
        /// Gets the current level.
        /// </summary>
        /// <value>The current level.</value>
        [NotMapped, JsonIgnore] public int CurrentLevel => Model.Experience.ExpToLevel(Experience);
        /// <summary>
        /// Gets the next level.
        /// </summary>
        /// <value>The next level.</value>
        [NotMapped, JsonIgnore] public int NextLevel => CurrentLevel + 1;
        /// <summary>
        /// Gets the next exp.
        /// </summary>
        /// <value>The next exp.</value>
        [NotMapped, JsonIgnore] public int NextExp => NextLevel * Model.Experience.LevelUpExp;
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
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ArgumentException">
        /// Username is invalid. - username
        /// or
        /// Email is invalid. - email
        /// or
        /// Password is invalid. - password
        /// </exception>
        public User(string username, string email, string password)
            : this()
        {
            if (Validate.UserName(username) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Username is invalid.", nameof(username));
            }
            if (Validate.Email(email) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Email is invalid.", nameof(email));
            }
            if (Validate.Password(password) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Password is invalid.", nameof(password));
            }

            Name = username;
            Email = new Email(email);
            Password = new Password(password);
            Created = DateTime.Now;
        }

        #endregion
        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Determines whether [is friends with] [the specified friend].
        /// </summary>
        /// <param name="friend">The friend.</param>
        /// <returns><c>true</c> if [is friends with] [the specified friend]; otherwise, <c>false</c>.</returns>
        public bool IsFriendsWith(User friend) => Friends.Any(f => f.Equals(friend));

        /// <summary>
        /// Determines whether [is pending friend with] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if [is pending friend with] [the specified user]; otherwise, <c>false</c>.</returns>
        public bool IsPendingFriendWith(User user) => 
            ReceivedFriendRequests.Any(f => f.RequesterUid.Equals(user?.Uid)) ||
            SentFriendRequests.Any(f => f.ConfirmerUid.Equals(user?.Uid));

        /// <summary>
        /// Increases the experience.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>System.Int32.</returns>
        public int IncreaseExperience(Experience.Action action) => Experience += (int)action;

        /// <summary>
        /// Decreases the experience.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns>System.Int32.</returns>
        public int DecreaseExperience(int amount) => Experience -= amount < 0 ? -1 : amount;

        /// <summary>
        /// Signs the in.
        /// </summary>
        public void SignIn()
        {
            SignedIn = DateTime.Now;
            IncreaseExperience(Model.Experience.Action.SignIn);
            Logs.Add(new UserLog(this, this, "signed in", null, false));
        }

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        public void SignOut()
        {
            SignedOut = DateTime.Now;
            Logs.Add(new UserLog(this, this, "signed out", null, false));
        }

        /// <summary>
        /// Sets the profile picture.
        /// </summary>
        /// <param name="avatar">The avatar.</param>
        /// <exception cref="ArgumentNullException">avatar</exception>
        public void SetProfilePicture(UserAvatar avatar)
        {
            Avatar = avatar ?? throw new ArgumentNullException(nameof(avatar));
            AvatarUid = avatar.Uid;
            Avatars.Add(avatar);

            IncreaseExperience(Model.Experience.Action.UploadImage);
            Logs.Add(new UserLog(this, this, "uploaded", avatar));
            RefreshBindings();
        }

        /// <summary>
        /// Sets the banner.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="banner">The banner.</param>
        /// <exception cref="ArgumentNullException">banner</exception>
        public void SetBanner(User user, UserBanner banner)
        {
            Banner = banner ?? throw new ArgumentNullException(nameof(banner));
            BannerUid = banner.Uid;
            Banners.Add(banner);

            IncreaseExperience(Model.Experience.Action.UploadImage);
            Logs.Add(new UserLog(this, user, "uploaded", banner));
            RefreshBindings();
        }

        /// <summary>
        /// Sends the friend request.
        /// </summary>
        /// <param name="friend">The friend.</param>
        /// <returns>Friendship.</returns>
        public Friendship SendFriendRequest(User friend)
        {
            if (friend == null)
            {
                Logger.WriteLine("Failed to send friend request with an uninitialized user object.");
                return null;
            }
            if (Uid == friend.Uid)
            {
                Logger.WriteLine($"Failed to send friend request from {Name} to {friend.Name} as they are the same user.");
                return null;
            }
            if (IsFriendsWith(friend))
            {
                Logger.WriteLine($"Failed to send friend request from {Name} to {friend.Name} as they are already friends.");
                return null;
            }
            var friendship = new Friendship(this, friend);
            SentFriendRequests.Add(friendship);
            Logs.Add(new UserLog(this, this, "sent friend request to", friend, false));
            friend.Logs.Add(new UserLog(friend, this, "sent friend request to", friend, false));
            return friendship;
        }

        /// <summary>
        /// Accepts the friend request.
        /// </summary>
        /// <param name="friend">The friend.</param>
        /// <returns>Friendship.</returns>
        public Friendship AcceptFriendRequest(User friend)
        {
            if (friend == null)
            {
                Logger.WriteLine($"Failed accept friend request from an uninitialized user object.");
                return null;
            }
            if (Uid == friend.Uid)
            {
                Logger.WriteLine($"Failed to accept friend request from {friend.Name} as they are the same user.");
                return null;
            }
            if (IsFriendsWith(friend))
            {
                Logger.WriteLine($"Failed to accept friend request from {friend.Name} as they are already friends.");
                return null;
            }
            var friendship = ReceivedFriendRequests.FirstOrDefault(f => f.RequesterUid.Equals(friend.Uid));
            if (friendship == null)
            {
                Logger.WriteLine($"Failed to accept friend request from {friend.Name} as the request could not be found.");
                return null;
            }
            friendship.Accept();
            IncreaseExperience(Model.Experience.Action.Befriend);
            friend.IncreaseExperience(Model.Experience.Action.Befriend);
            Logs.Add(new UserLog(this, this, "accepted friend request from", friend, false));
            friend.Logs.Add(new UserLog(friend, this, "accepted friend request from", friend, false));
            return friendship;
        }

        /// <summary>
        /// Gets the friendship.
        /// </summary>
        /// <param name="friend">The friend.</param>
        /// <returns>Friendship.</returns>
        public Friendship GetFriendship(User friend)
        {
            return
                ReceivedFriendRequests.FirstOrDefault(f => f.RequesterUid.Equals(friend.Uid)) ??
                SentFriendRequests.FirstOrDefault(f => f.ConfirmerUid.Equals(friend.Uid)) ??
                friend.ReceivedFriendRequests.FirstOrDefault(f => f.RequesterUid.Equals(Uid)) ??
                friend.SentFriendRequests.FirstOrDefault(f => f.ConfirmerUid.Equals(Uid));
        }

        /// <summary>
        /// Removes the friendship.
        /// </summary>
        /// <param name="friend">The friend.</param>
        /// <returns>Friendship.</returns>
        public Friendship RemoveFriendship(User friend)
        {
            if (friend == null)
            {
                Logger.WriteLine($"Failed to unfriend {Name} with an uninitialized user object.");
                return null;
            }
            if (Uid == friend.Uid)
            {
                Logger.WriteLine($"Failed to unfriend {Name} and {friend.Name} as they are the same user.");
                return null;
            }

            var friendship = GetFriendship(friend);

            if (friendship == null)
            {
                Logger.WriteLine($"Failed to unfriend {Name} and {friend.Name} as they do not have a friendship.");
                return null;
            }

            RemoveSentFriendRequest(friendship);
            RemoveReceivedFriendRequest(friendship);
            friend.RemoveSentFriendRequest(friendship);
            friend.RemoveReceivedFriendRequest(friendship);
            Logs.Add(new UserLog(this, this, "is no longer friends with", friend));
            friend.Logs.Add(new UserLog(friend, this, "is no longer friends with", friend));
            return friendship;
        }

        /// <summary>
        /// Removes the sent friend request.
        /// </summary>
        /// <param name="friendship">The friendship.</param>
        private void RemoveSentFriendRequest(Friendship friendship)
        {
            friendship = SentFriendRequests.FirstOrDefault(f => f.Equals(friendship));

            if (friendship == null)
            {
                return;
            }

            SentFriendRequests.Remove(friendship);
        }

        /// <summary>
        /// Removes the received friend request.
        /// </summary>
        /// <param name="friendship">The friendship.</param>
        private void RemoveReceivedFriendRequest(Friendship friendship)
        {
            friendship = ReceivedFriendRequests.FirstOrDefault(f => f.Equals(friendship));

            if (friendship == null)
            {
                return;
            }

            ReceivedFriendRequests.Remove(friendship);
        }

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        /// <param name="a">a.</param>
        public void SetColor(byte r, byte g, byte b, byte a)
        {
            SetColor(new Color(r, g, b, a));
        }

        /// <summary>
        /// Sets the color.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetColor(Color color)
        {
            if (color == null) return;

            Color = color;
            IncreaseExperience(Model.Experience.Action.ChangedSettings);
            Logs.Add(new UserLog(this, this, $"changed his color to {color}"));
        }

        #endregion
    }
}