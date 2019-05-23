using CodeShare.Extensions;
using CodeShare.Utilities;
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
    public class Comment : Entity, IComment, IComment<Comment>, ILikeable<CommentRating>
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }
        public virtual User User { get; set; }
        public Guid? UserUid { get; set; }
        public Guid? CommentSectionUid { get; set; }
        public virtual Comment Parent { get; set; }
        public Guid? ParentUid { get; set; }
        public virtual SortedObservableCollection<Comment> Replies { get; set; } = new SortedObservableCollection<Comment>(c => c.Created, true);
        public virtual ObservableCollection<CommentLog> Logs { get; set; } = new ObservableCollection<CommentLog>();
        public virtual ObservableCollection<CommentRating> Ratings { get; set; } = new ObservableCollection<CommentRating>();
        [NotMapped, JsonIgnore] public bool HasRatings => Ratings?.Count > 0;
        [NotMapped, JsonIgnore] public int NumberOfLikes => Ratings?.Count(x => x.Value) ?? 0;
        [NotMapped, JsonIgnore] public int NumberOfDislikes => Ratings?.Count(x => !x.Value) ?? 0;

        public Comment()
        {
        }

        public Comment(User user, string text)
        {
            UserUid = user.Uid;
            Text = text;

            Logs.Add(new CommentLog(this, user, "created this"));
        }
        public Comment(CommentSection parent, User user, string text)
            : this(user, text)
        {
            CommentSectionUid = parent.Uid;
        }
        public Comment(Comment parent, User user, string text)
            : this(user, text)
        {
            ParentUid = parent.Uid;
        }

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
                Ratings = new ObservableCollection<CommentRating>();
            }

            var rating = Ratings.FirstOrDefault(i => i.User.Equals(user));

            if (rating != null)
            {
                rating.Value = true;
            }
            else
            {
                Ratings.Add(new CommentRating(user, true));
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
                Ratings = new ObservableCollection<CommentRating>();
            }

            var rating = Ratings.FirstOrDefault(i => i.User.Equals(user));

            if (rating != null)
            {
                rating.Value = false;
            }
            else
            {
                Ratings.Add(new CommentRating(user, false));
            }
        }

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
