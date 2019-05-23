using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CodeShare.Services;
using CodeShare.Utilities;
using System.Collections;

namespace CodeShare.Model
{
    public class User : Entity, IAccount, IContent
    {
        #region Properties
        private Email _email;
        public virtual Email Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }
        private Password _password = new Password();
        public virtual Password Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        private int _views;
        public int Views
        {
            get => _views;
            set => SetField(ref _views, value);
        }
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetField(ref _firstName, value);
        }
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetField(ref _lastName, value);
        }
        private DateTime? _birthday;
        public DateTime? Birthday
        {
            get => _birthday;
            set => SetField(ref _birthday, value);
        }
        private string _gender;
        public string Gender
        {
            get => _gender;
            set => SetField(ref _gender, value);
        }
        private string _country;
        public string Country
        {
            get => _country;
            set => SetField(ref _country, value);
        }
        private string _bio;
        public string Bio
        {
            get => _bio;
            set => SetField(ref _bio, value);
        }
        private bool _subscribed;
        public bool Subscribed
        {
            get => _subscribed;
            set => SetField(ref _subscribed, value);
        }
        private string _website;
        public string Website
        {
            get => _website;
            set => SetField(ref _website, value);
        }
        private DateTime? _signedIn = DateTime.Now;
        public DateTime? SignedIn
        {
            get => _signedIn;
            set => SetField(ref _signedIn, value);
        }
        private DateTime? _signedOut;
        public DateTime? SignedOut
        {
            get => _signedOut;
            set => SetField(ref _signedOut, value);
        }
        private Color _color = new Color();
        public virtual Color Color
        {
            get => _color;
            set => SetField(ref _color, value);
        }
        private int _experience;
        public int Experience
        {
            get => _experience;
            set => SetField(ref _experience, value);
        }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ObservableCollection<Code> Codes { get; set; } = new ObservableCollection<Code>();
        public virtual ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        public virtual ObservableCollection<ProfilePicture> ProfilePictures { get; set; } = new ObservableCollection<ProfilePicture>();
        public virtual ObservableCollection<UserBanner> Banners { get; set; } = new ObservableCollection<UserBanner>();
        public virtual ICollection<UserLog> Logs { get; set; } = new ObservableCollection<UserLog>();
        public virtual ObservableCollection<Friendship> SentFriendRequests { get; set; } = new ObservableCollection<Friendship>();
        public virtual ObservableCollection<Friendship> ReceievedFriendRequests { get; set; } = new ObservableCollection<Friendship>();
        [NotMapped, JsonIgnore] public IEnumerable<User> Friends
        {
            get
            {
                return SentFriendRequests.Where(x => x.Confirmed != null).Select(f => f.Confirmer)
                    .Concat(ReceievedFriendRequests.Where(x => x.Confirmed != null).Select(f => f.Requester));
            }
        }
        [NotMapped, JsonIgnore] public Uri WebsiteUri => Uri.TryCreate(Website, UriKind.Absolute, out Uri uri) ? uri : null;
        [NotMapped, JsonIgnore] public ProfilePicture ProfilePicture => ProfilePictures.FirstOrDefault(p => p.IsPrimary);
        [NotMapped, JsonIgnore] public UserBanner Banner => Banners.FirstOrDefault(p => p.IsPrimary);
        [NotMapped, JsonIgnore] public bool HasBanners => Banners != null && Banners.Any();
        [NotMapped, JsonIgnore] public bool IsValid => Validate.UserName(Name) == ValidationResponse.Valid && Validate.Email(Email.Address) == ValidationResponse.Valid && Password != null;
        [NotMapped, JsonIgnore] public string FullName => $"{FirstName} {LastName}";
        [NotMapped, JsonIgnore] public string Initials => (!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName)) ? FirstName?.Substring(0, 1) + LastName?.Substring(0, 1) : "";
        [NotMapped, JsonIgnore] public int CurrentLevel => Model.Experience.ExpToLevel(Experience);
        [NotMapped, JsonIgnore] public int NextLevel => CurrentLevel + 1;
        [NotMapped, JsonIgnore] public int NextExp => NextLevel * Model.Experience.LevelUpExp;

        #endregion
        #region Constructors

        public User()
        {
        }

        public User(string username, string email, string password)
        {
            if (Validate.UserName(username) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Username is invalid.", "username");
            }
            if (Validate.Email(email) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Email is invalid.", "email");
            }
            if (Validate.Password(password) != ValidationResponse.Valid)
            {
                throw new ArgumentException("Password is invalid.", "password");
            }

            Name = username;
            Email = new Email(email);
            Password = new Password(password);
            Created = DateTime.Now;
        }

        #endregion
        #region Methods

        public override string ToString() => Name;

        public bool IsFriendsWith(User friend) => Friends.Any(f => f.Equals(friend));

        public int IncreaseExperience(Experience.Action action) => Experience += (int)action;

        public int DecreaseExperience(int amount) => Experience -= amount < 0 ? -1 : amount;

        public void SignIn()
        {
            SignedIn = DateTime.Now;
            IncreaseExperience(Model.Experience.Action.SignIn);
            Logs.Add(new UserLog(this, this, "signed in", null, false));
        }

        public void SignOut()
        {
            SignedOut = DateTime.Now;
            Logs.Add(new UserLog(this, this, "signed out", null, false));
        }

        public void SetProfilePicture(ProfilePicture profilePicture)
        {
            if (profilePicture == null)
            {
                throw new InvalidOperationException("Profile Picture is null.");
            }

            var existingProfilePicture = ProfilePictures.FirstOrDefault(i => i.Uid.Equals(profilePicture.Uid));
            if (existingProfilePicture == null)
            {
                ProfilePictures.Add(profilePicture);
            }
            else
            {
                ProfilePictures.Remove(existingProfilePicture);
                ProfilePictures.Add(profilePicture);
            }

            for (var i = 0; i < ProfilePictures.Count; i++)
            {
                ProfilePictures[i].IsPrimary = profilePicture.Uid.Equals(ProfilePictures[i].Uid);
            }

            IncreaseExperience(Model.Experience.Action.UploadImage);
            Logs.Add(new UserLog(this, this, "uploaded", profilePicture));
            RefreshBindings();
        }

        public void SetBanner(User user, UserBanner banner)
        {
            if (banner == null)
            {
                throw new NullReferenceException("Video was null.");
            }
            if (Banners == null)
            {
                Banners = new ObservableCollection<UserBanner>();
            }

            var existingBanner = Banners.FirstOrDefault(i => i.Uid.Equals(banner.Uid));
            if (existingBanner == null)
            {
                Banners.Add(banner);
            }
            else
            {
                Banners.Remove(existingBanner);
                Banners.Add(banner);
            }

            for (var i = 0; i < Banners.Count; i++)
            {
                Banners[i].IsPrimary = banner.Uid.Equals(Banners[i].Uid);
            }

            IncreaseExperience(Model.Experience.Action.UploadImage);
            Logs.Add(new UserLog(this, user, "uploaded", banner));
            RefreshBindings();
        }

        public void SendFriendRequest(User friend)
        {
            if (friend == null)
            {
                Logger.WriteLine($"Failed to send friend request with an uninitialized user object.");
                return;
            }
            if (Uid == friend.Uid)
            {
                Logger.WriteLine($"Failed to send friend request from {Name} to {friend.Name} as they are the same user.");
                return;
            }
            if (IsFriendsWith(friend))
            {
                Logger.WriteLine($"Failed to send friend request from {Name} to {friend.Name} as they are already friends.");
                return;
            }
            var friendship = new Friendship(this, friend);
            SentFriendRequests.Add(friendship);
            Logs.Add(new UserLog(this, this, "sent friend request to", friend, false));
        }

        public void AcceptFriendRequest(User friend)
        {
            if (friend == null)
            {
                Logger.WriteLine($"Failed accept friend request from an uninitialized user object.");
                return;
            }
            if (Uid == friend.Uid)
            {
                Logger.WriteLine($"Failed to accept friend request from {friend.Name} as they are the same user.");
                return;
            }
            if (IsFriendsWith(friend))
            {
                Logger.WriteLine($"Failed to accept friend request from {friend.Name} as they are already friends.");
                return;
            }
            var friendship = ReceievedFriendRequests.FirstOrDefault(f => f.Requester.Equals(friend));
            friendship.Confirmed = DateTime.Now;
            IncreaseExperience(Model.Experience.Action.Befriend);
            friend.IncreaseExperience(Model.Experience.Action.Befriend);
        }

        public void RemoveFriend(User friend)
        {
            if (friend == null)
            {
                Logger.WriteLine($"Failed to unfriend {Name} with an uninitialized user object.");
                return;
            }
            if (Uid == friend.Uid)
            {
                Logger.WriteLine($"Failed to unfriend {Name} and {friend.Name} as they are the same user.");
                return;
            }
            var friendship = ReceievedFriendRequests.FirstOrDefault(f => f.Requester.Equals(friend));

            if (friendship == null)
            {
                Logger.WriteLine($"Failed to unfriend {Name} and {friend.Name} as they are not friends.");
                return;
            }
            RemoveSentFriendRequest(friend);
            RemoveReceivedFriendRequest(friend);
            Logs.Add(new UserLog(this, this, "is no longer friends with", friend));
        }

        private void RemoveSentFriendRequest(User friend)
        {
            var friendship = SentFriendRequests.FirstOrDefault(f => f.Confirmer.Equals(friend));

            if (friendship == null)
            {
                Logger.WriteLine($"Failed to remove sent friend request from {Name} to {friend.Name}. Not request could be found.");
                return;
            }

            SentFriendRequests.Remove(friendship);
        }

        private void RemoveReceivedFriendRequest(User friend)
        {
            var friendship = ReceievedFriendRequests.FirstOrDefault(f => f.Requester.Equals(friend));

            if (friendship == null)
            {
                Logger.WriteLine($"Failed to remove received friend request from {Name} to {friend.Name}. Not request could be found.");
                return;
            }

            ReceievedFriendRequests.Remove(friendship);
        }

        public void SetColor(byte r, byte g, byte b, byte a)
        {
            SetColor(new Color(r, g, b, a));
        }

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