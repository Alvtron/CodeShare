﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace CodeShare.Model
{
    public class Comment : Entity, IComment
    {
        public User User { get; set; }

        public Guid UserUid { get; set; }

        public Content Content { get; set; }

        public Guid ContentUid { get; set; }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        public ICollection<CommentLog> Logs { get; set; } = new ObservableCollection<CommentLog>();

        public ObservableCollection<Comment> Replies { get; set; } = new ObservableCollection<Comment>();
        public IEnumerable<Comment> RepliesSorted => Replies.OrderByDescending(c => c.Created);

        public IList<Rating> Ratings { get; set; } = new List<Rating>();

        [NotMapped, JsonIgnore]
        public bool HasRatings => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public bool HasReplies => Replies?.Count > 0;
        [NotMapped, JsonIgnore]
        public bool HasLikes => Ratings?.Count > 0;
        [NotMapped, JsonIgnore]
        public int NumberOfReplies => Replies?.Count ?? 0;
        [NotMapped, JsonIgnore]
        public int NumberOfLikes => Ratings?.Count(x => x.Value) ?? 0;
        [NotMapped, JsonIgnore]
        public int NumberOfDislikes => Ratings?.Count(x => !x.Value) ?? 0;

        public Comment()
        {
        }

        public Comment(User user, Guid contentUid, string text)
        {
            UserUid = user.Uid;
            ContentUid = contentUid;
            Text = text;
            Logs.Add(new CommentLog(this, user, "created this"));
        }

        public void Reply(Comment reply)
        {
            if (reply == null)
                throw new NullReferenceException("Comment was null.");
            if (Replies == null)
                Replies = new ObservableCollection<Comment>();

            Replies.Add(reply);
            Logs.Add(new CommentLog(this, reply.User, "replied", reply));
        }

        public void Like(User user)
        {
            if (Ratings == null || HasLiked(user))
                return;

            Ratings.Add(new Rating(user, true));
            Logs.Add(new CommentLog(this, user, "liked this", null, false));
        }

        public void Dislike(User user)
        {
            if (!HasLikes)
                return;

            Ratings.Remove(Ratings.Single(i => i.User.Equals(user)));
            Logs.Add(new CommentLog(this, user, "disliked this", null, false));
        }

        public bool HasLiked(User user)
        {
            return HasLikes && Ratings.Any(x => x.User.Equals(user));
        }

        public bool HasRated(User user)
        {
            return HasRatings && Ratings.Any(x => x.User.Equals(user));
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
