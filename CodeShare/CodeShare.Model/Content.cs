using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CodeShare.Model
{
    public abstract class Content : Entity
    {
        #region Properties

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

        public ObservableCollection<Screenshot> Screenshots { get; set; } = new ObservableCollection<Screenshot>();
        public IEnumerable<Screenshot> ScreenshotsSorted => Screenshots.OrderByDescending(c => c.Created);

        public ObservableCollection<Banner> Banners { get; set; } = new ObservableCollection<Banner>();
        public IEnumerable<Banner> BannersSorted => Banners?.OrderByDescending(c => c.Created);

        public ObservableCollection<Video> Videos { get; set; } = new ObservableCollection<Video>();
        public IEnumerable<Video> VideosSorted => Videos.OrderByDescending(c => c.Created);

        public ObservableCollection<Comment> Comments { get; set; } = new ObservableCollection<Comment>();
        public IEnumerable<Comment> CommentsSorted => Comments.OrderByDescending(c => c.Created);

        public IList<Rating> Ratings { get; set; } = new List<Rating>();

        [NotMapped, JsonIgnore]
        public Banner Banner => Banners.FirstOrDefault(p => p.IsPrimary);

        [NotMapped, JsonIgnore]
        public bool HasVideos => Videos != null && Videos.Any();
        [NotMapped, JsonIgnore]
        public bool HasImages => Screenshots != null && Screenshots.Any();
        [NotMapped, JsonIgnore]
        public bool HasBanners => Banners != null && Banners.Any();
        [NotMapped, JsonIgnore]
        public bool HasReplies => Comments?.Count > 0;
        [NotMapped, JsonIgnore]
        public bool HasRatings => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public int NumberOfReplies => Comments?.Count ?? 0;
        [NotMapped, JsonIgnore]
        public bool HasLikes => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public int NumberOfLikes => Ratings?.Count(x => x.Value) ?? 0;
        [NotMapped, JsonIgnore]
        public int NumberOfDislikes => Ratings?.Count(x => !x.Value) ?? 0;

        #endregion

        #region Methods

        public void SetBanner(User user, Banner banner)
        {
            if (banner == null)
                throw new NullReferenceException("Video was null.");
            if (user == null)
                throw new NullReferenceException("User was null.");
            if (Videos == null)
                Videos = new ObservableCollection<Video>();

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

            RefreshBindings();
        }

        public void AddVideo(User user, Video video)
        {
            if (video == null || video.Empty)
                throw new NullReferenceException("Video was null.");
            if (user == null)
                throw new NullReferenceException("User was null.");
            if (Videos == null)
                Videos = new ObservableCollection<Video>();

            Videos.Insert(0, video);
        }

        public void Reply(Comment comment)
        {
            if (comment == null)
                throw new NullReferenceException("Comment was null.");
            if (Comments == null)
                Comments = new ObservableCollection<Comment>();

            Comments.Add(comment);
        }

        public void Like(User user)
        {
            if (Ratings == null || HasLiked(user))
                return;

            Ratings.Add(new Rating(user, true));
        }

        public void Dislike(User user)
        {
            if (!HasLikes)
                return;

            Ratings.Remove(Ratings.Single(i => i.User.Equals(user)));
        }

        public bool HasLiked(User user)
        {
            return HasLikes && Ratings.Any(x => x.User.Equals(user));
        }

        public bool HasRated(User user)
        {
            return HasRatings && Ratings.Any(x => x.User.Equals(user));
        }

        public void AddScreenshot(Screenshot screenshot, User user)
        {
            if (screenshot == null)
                throw new NullReferenceException("Screenshot was null.");
            if (user == null)
                throw new NullReferenceException("User was null.");
            if (Screenshots == null)
                Screenshots = new ObservableCollection<Screenshot>();

            Screenshots.Add(screenshot);
        }

        #endregion
    }
}
