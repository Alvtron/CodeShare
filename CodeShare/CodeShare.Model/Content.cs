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

        private ObservableCollection<Screenshot> _screenshots = new ObservableCollection<Screenshot>();
        public ObservableCollection<Screenshot> Screenshots
        {
            get => _screenshots;
            set => SetField(ref _screenshots, value);
        }

        private ObservableCollection<Banner> _banners = new ObservableCollection<Banner>();
        public ObservableCollection<Banner> Banners
        {
            get => _banners;
            set => SetField(ref _banners, value);
        }

        private ObservableCollection<Video> _videos = new ObservableCollection<Video>();
        public ObservableCollection<Video> Videos
        {
            get => _videos;
            set => SetField(ref _videos, value);
        }

        private ObservableCollection<Reply> _replies = new ObservableCollection<Reply>();
        public ObservableCollection<Reply> Replies
        {
            get => _replies;
            set => SetField(ref _replies, value);
        }

        private ObservableCollection<ContentRating> _ratings = new ObservableCollection<ContentRating>();
        public ObservableCollection<ContentRating> Ratings
        {
            get => _ratings;
            set => SetField(ref _ratings, value);
        }

        [NotMapped, JsonIgnore]
        public Banner Banner => Banners.FirstOrDefault(p => p.IsPrimary);
        [NotMapped, JsonIgnore]
        public bool HasVideos => Videos != null && Videos.Any();
        [NotMapped, JsonIgnore]
        public bool HasImages => Screenshots != null && Screenshots.Any();
        [NotMapped, JsonIgnore]
        public bool HasBanners => Banners != null && Banners.Any();
        [NotMapped, JsonIgnore]
        public bool HasReplies => Replies?.Count > 0;
        [NotMapped, JsonIgnore]
        public bool HasRatings => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public int NumberOfReplies => Replies?.Count ?? 0;
        [NotMapped, JsonIgnore]
        public bool HasLikes => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public int NumberOfLikes => Ratings?.Count(x => x.Value) ?? 0;
        [NotMapped, JsonIgnore]
        public int NumberOfDislikes => Ratings?.Count(x => !x.Value) ?? 0;

        #endregion

        #region Methods

        public abstract void SetBanner(User user, Banner banner);
        protected void SetBanner(Banner banner)
        {
            if (banner == null)
                throw new NullReferenceException("Video was null.");
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

        public abstract void AddScreenshot(User user, Screenshot screenshot);
        protected void AddScreenshot(Screenshot screenshot)
        {
            if (screenshot == null)
                throw new NullReferenceException("Screenshot was null.");
            if (Screenshots == null)
                Screenshots = new ObservableCollection<Screenshot>();

            Screenshots.Add(screenshot);
        }

        public abstract void AddVideo(User user, Video video);
        protected void AddVideo(Video video)
        {
            if (video == null || video.Empty)
                throw new NullReferenceException("Video was null.");
            if (Videos == null)
                Videos = new ObservableCollection<Video>();

            Videos.Add(video);
        }

        public abstract void Reply(User user, Reply comment);
        protected void Reply(Reply comment)
        {
            if (comment == null)
                throw new NullReferenceException("Comment was null.");
            if (Replies == null)
                Replies = new ObservableCollection<Reply>();

            Replies.Add(comment);

            Replies = new ObservableCollection<Reply>(Replies.OrderByDescending(c => c.Created));
        }

        public void Like(User user)
        {
            if (Ratings == null || HasLiked(user))
                return;

            Ratings.Add(new ContentRating(user, true));
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

        #endregion
    }
}
