using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace CodeShare.Model
{
    public abstract class Comment : Entity
    {
        public User User { get; set; }
        public Guid? UserUid { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public ObservableCollection<CommentLog> Logs { get; set; } = new ObservableCollection<CommentLog>();
        public ObservableCollection<CommentRating> Ratings { get; set; } = new ObservableCollection<CommentRating>();

        [NotMapped, JsonIgnore]
        public bool HasRatings => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public bool HasLikes => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public int NumberOfLikes => Ratings?.Count(x => x.Value) ?? 0;
        [NotMapped, JsonIgnore]
        public int NumberOfDislikes => Ratings?.Count(x => !x.Value) ?? 0;

        public void Like(User user)
        {
            if (Ratings == null || HasLiked(user))
                return;

            Ratings.Add(new CommentRating(user, true));
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
    }
}
