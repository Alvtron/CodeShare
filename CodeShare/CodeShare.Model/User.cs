using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using CodeShare.Model.Services;

namespace CodeShare.Model
{
    public class User : Content, IAccount
    {
        [Required]
        private Email _email;
        public Email Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        [Required]
        private Password _password = new Password();
        public Password Password
        {
            get => _password;
            set => SetField(ref _password, value);
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
        [NotMapped, JsonIgnore]
        public Uri WebsiteUri => Uri.TryCreate(Website, UriKind.Absolute, out Uri uri) ? uri : null;

        [Required]
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
        public Color Color
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

        public ObservableCollection<User> Friends { get; set; } = new ObservableCollection<User>();
        public IEnumerable<User> FriendsSorted => Friends?.OrderBy(c => c.Name);

        public ObservableCollection<Code> Codes { get; set; } = new ObservableCollection<Code>();
        public IEnumerable<Code> CodesSorted => Codes?.OrderBy(c => c.Name);

        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        public IEnumerable<Question> QuestionsSorted => Questions?.OrderByDescending(c => c.Created);

        public ObservableCollection<ProfilePicture> ProfilePictures { get; set; } = new ObservableCollection<ProfilePicture>();
        public IEnumerable<ProfilePicture> ProfilePicturesSorted => ProfilePictures.OrderByDescending(c => c.Created);

        public ICollection<UserLog> Logs { get; set; } = new ObservableCollection<UserLog>();

        [NotMapped, JsonIgnore]
        public ProfilePicture ProfilePicture => ProfilePictures.FirstOrDefault(p => p.IsPrimary);


        // CONSTRUCTORS

        public User()
        {
        }

        public User(string username, string email, string password)
        {
            if (ValidationService.ValidateUserName(username) != ValidationResponse.Valid) return;
            if (ValidationService.ValidatePassword(password) != ValidationResponse.Valid) return;

            Name = username;
            Email = new Email(email);
            Password = new Password(password);
            Created = DateTime.Now;
        }

        // FUNCTIONS

        public bool Equals(User user) => Uid == user?.Uid || Name == user?.Name || Email == user?.Email;

        public int IncreaseExperience(Experience.Action action) => Experience += (int)action;

        public int DecreaseExperience(int amount) => (amount < 0) ? -1 : Experience -= amount;

        public void SignIn()
        {
            SignedIn = DateTime.Now;
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

            Logs.Add(new UserLog(this, this, "uploaded", profilePicture));
            RefreshBindings();
        }

        public new void SetBanner(User user, Banner banner)
        {
            base.SetBanner(user, banner);
            Logs.Add(new UserLog(this, user, "uploaded", banner));
        }

        public void AddFriend(User friend)
        {
            if (friend == null)
            {
                Debug.WriteLine($"Can't befriend {Name} with an uninitialized user object.");
                return;
            }
            if (Uid == friend.Uid)
            {
                Debug.WriteLine($"Can't befriend {Name} and {friend.Name} as they are the same user.");
                return;
            }
            if (IsFriendsWith(friend))
            {
                Debug.WriteLine($"Can't befriend {Name} and {friend.Name} as they are already friends.");
                return;
            }

            Friends.Add(friend);
            Logs.Add(new UserLog(this, this, "is now friends with", friend));
        }

        public void RemoveFriend(User friend)
        {
            if (friend == null)
            {
                Debug.WriteLine($"Can't unfriend {Name} with an uninitialized user object.");
                return;
            }
            if (Uid == friend.Uid)
            {
                Debug.WriteLine($"Can't unfriend {Name} and {friend.Name} as they are the same user.");
                return;
            }
            if (!IsFriendsWith(friend))
            {
                Debug.WriteLine($"Can't unfriend {Name} and {friend.Name} as they are not friends.");
                return;
            }

            Friends.Remove(friend);

            Logs.Add(new UserLog(this, this, "is no longer friends with", friend));
        }

        public void AddCode(Code code)
        {
            if (!Codes.Contains(code))
            {
                Codes.Add(code);
                Logs.Add(new UserLog(this, this, "added", code));
            }
        }

        public void RemoveCode(Code code)
        {
            if (Codes.Contains(code))
            {
                Codes.Remove(code);
                Logs.Add(new UserLog(this, this, "removed", code));
            }
        }

        public void SetColor(byte r, byte g, byte b, byte a)
        {
            SetColor(new Color(r, g, b, a));
        }

        public void SetColor(Color color)
        {
            if (color == null) return;

            Color = color;
            Logs.Add(new UserLog(this, this, $"changed his color to {color}"));
        }

        public bool IsFriendsWith(User friend) => Friends.Any(f => f.Uid == friend.Uid);

        // NOT MAPPED

        [NotMapped, JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped, JsonIgnore]
        public string Initials => (!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName)) ? FirstName?.Substring(0, 1) + LastName?.Substring(0, 1) : "";

        [NotMapped, JsonIgnore]
        public bool Valid => !string.IsNullOrWhiteSpace(Name) && Email != null && Password != null;

        [NotMapped, JsonIgnore]
        public int CurrentLevel => Model.Experience.ExpToLevel(Experience);

        [NotMapped, JsonIgnore]
        public int NextLevel => CurrentLevel + 1;

        [NotMapped, JsonIgnore]
        public int NextExp => NextLevel * Model.Experience.LevelUpExp;

        // OVERRIDES

        public override string ToString() => Name;

        public override bool Equals(object obj) =>  (obj is User user) ? Equals(user) : false;

        public override int GetHashCode() => base.GetHashCode();
    }
}