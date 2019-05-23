using CodeShare.Extensions;
using CodeShare.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CodeShare.Model
{
    public class Code : Entity, IContent, ILikeable<CodeRating>
    {
        #region Properties

        public virtual User User { get; set; }
        public Guid? UserUid { get; set; }
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
        private string _version;
        public string Version
        {
            get => _version;
            set => SetField(ref _version, value);
        }
        private string _description;
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }
        private string _about;
        public string About
        {
            get => _about;
            set => SetField(ref _about, value);
        }
        public virtual CodeCommentSection CommentSection { get; set; }
        public Guid? CommentSectionUid { get; set; }
        public virtual ObservableCollection<CodeFile> Files { get; set; } = new ObservableCollection<CodeFile>();
        public virtual ObservableCollection<CodeLog> Logs { get; set; } = new ObservableCollection<CodeLog>();
        public virtual ObservableCollection<CodeScreenshot> Screenshots { get; set; } = new ObservableCollection<CodeScreenshot>();
        public virtual ObservableCollection<CodeBanner> Banners { get; set; } = new ObservableCollection<CodeBanner>();
        public virtual ObservableCollection<CodeVideo> Videos { get; set; } = new ObservableCollection<CodeVideo>();
        //public virtual SortedObservableCollection<CodeComment> Replies { get; set; } = new SortedObservableCollection<CodeComment>(c => c.Created, true);
        public virtual ObservableCollection<CodeRating> Ratings { get; set; } = new ObservableCollection<CodeRating>();
        [NotMapped, JsonIgnore] public bool Valid => Uid != Guid.Empty && !string.IsNullOrWhiteSpace(Name);
        [NotMapped, JsonIgnore] public CodeBanner Banner => Banners.FirstOrDefault(p => p.IsPrimary);
        [NotMapped, JsonIgnore] public bool HasVideos => Videos != null && Videos.Any();
        [NotMapped, JsonIgnore] public bool HasImages => Screenshots != null && Screenshots.Any();
        [NotMapped, JsonIgnore] public bool HasBanners => Banners != null && Banners.Any();
        [NotMapped, JsonIgnore] public bool HasRatings => Ratings?.Count > 0;
        [NotMapped, JsonIgnore] public int NumberOfLikes => Ratings?.Count(x => x.Value) ?? 0;
        [NotMapped, JsonIgnore] public int NumberOfDislikes => Ratings?.Count(x => !x.Value) ?? 0;

        #endregion
        #region Constructors

        public Code()
        {
            CommentSection = new CodeCommentSection(Uid);
        }

        public Code(string name, IEnumerable<CodeFile> files, User author)
            : this()
        {
            Name = name;
            Files = new ObservableCollection<CodeFile>(files);
            UserUid = author.Uid;
            Logs.Add(new CodeLog(this, author, "created this"));
        }

        #endregion
        #region Methods

        public bool HasLiked(User user) => Ratings.Any(x => x.Value == true && x.User.Equals(user));
        public bool HasDisliked(User user) => Ratings.Any(x => x.Value == false && x.User.Equals(user));
        public bool HasRated(User user) => Ratings.Any(x => x.User.Equals(user));

        public void ToggleLike(User user)
        {
            if (HasRated(user))
            {
                Like(user);
            }
            else if (HasLiked(user))
            {
                Dislike(user);
            }
            else
            {
                Like(user);
            }
        }

        public void Like(User user)
        {
            if (user == null)
            {
                Logger.WriteLine($"Failed to like comment {Uid}. User is null.");
                return;
            }
            if (HasLiked(user))
            {
                Logger.WriteLine($"Failed to like comment {Uid}. User {user.Uid} has already liked this comment.");
                return;
            }
            if (Ratings == null)
            {
                Ratings = new ObservableCollection<CodeRating>();
            }

            var rating = Ratings.FirstOrDefault(i => i.User.Equals(user));

            if (rating != null)
            {
                rating.Value = true;
            }
            else
            {
                Ratings.Add(new CodeRating(user, true));
            }
        }

        public void Dislike(User user)
        {
            if (user == null)
            {
                Logger.WriteLine($"Failed to dislike comment {Uid}. User is null.");
                return;
            }
            if (HasDisliked(user))
            {
                Logger.WriteLine($"Failed to dislike comment {Uid}. User {user.Uid} has already disliked this comment.");
                return;
            }
            if (Ratings == null)
            {
                Ratings = new ObservableCollection<CodeRating>();
            }

            var rating = Ratings.FirstOrDefault(i => i.User.Equals(user));

            if (rating != null)
            {
                rating.Value = false;
            }
            else
            {
                Ratings.Add(new CodeRating(user, false));
            }
        }

        public override string ToString() => Name;

        public void AddFile(CodeFile file, User user)
        {
            if (file == null)
            {
                throw new ArgumentNullException("File was null.");
            }
            if (user == null)
            {
                throw new ArgumentNullException("User was null.");
            }
            if (Files == null)
            {
                Files = new ObservableCollection<CodeFile>();
            }

            Files.Add(file);
            user.IncreaseExperience(Experience.Action.UploadFile);
            Logs.Add(new CodeLog(this, user, $"added", file));
        }

        public void SetBanner(User user, CodeBanner banner)
        {
            if (banner == null)
            {
                throw new NullReferenceException("Video was null.");
            }
            if (Banners == null)
            {
                Banners = new ObservableCollection<CodeBanner>();
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

            user.IncreaseExperience(Experience.Action.UploadImage);
            Logs.Add(new CodeLog(this, user, "uploaded", banner));
            RefreshBindings();
        }

        public void AddScreenshot(User user, CodeScreenshot screenshot)
        {
            if (screenshot == null)
            {
                throw new NullReferenceException("Screenshot was null.");
            }
            if (Screenshots == null)
            {
                Screenshots = new ObservableCollection<CodeScreenshot>();
            }

            Screenshots.Add(screenshot);

            user.IncreaseExperience(Experience.Action.UploadImage);
            Logs.Add(new CodeLog(this, user, "added", screenshot));
            RefreshBindings();
        }

        public void AddVideo(User user, CodeVideo video)
        {
            if (video == null || video.Empty)
            {
                throw new NullReferenceException("Video was null.");
            }
            if (Videos == null)
            {
                Videos = new ObservableCollection<CodeVideo>();
            }

            Videos.Add(video);

            user.IncreaseExperience(Experience.Action.UploadVideo);
            Logs.Add(new CodeLog(this, user, "added", video));
        }

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
