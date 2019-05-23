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
    public class Question : Entity, IContent, ILikeable<QuestionRating>
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
        private string _text;
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }
        public virtual CodeLanguage CodeLanguage { get; set; }
        public Guid? CodeLanguageUid { get; set; }
        public virtual User User { get; set; }
        public Guid? UserUid { get; set; }
        public virtual Comment Solution { get; set; }
        public Guid? SolutionUid { get; set; }
        public DateTime? DateTimeSolved { get; set; }
        public virtual QuestionCommentSection CommentSection { get; set; }
        public Guid? CommentSectionUid { get; set; }
        public virtual ObservableCollection<QuestionLog> Logs { get; set; } = new ObservableCollection<QuestionLog>();
        public virtual ObservableCollection<QuestionScreenshot> Screenshots { get; set; } = new ObservableCollection<QuestionScreenshot>();
        public virtual ObservableCollection<QuestionVideo> Videos { get; set; } = new ObservableCollection<QuestionVideo>();
        public virtual ObservableCollection<QuestionRating> Ratings { get; set; } = new ObservableCollection<QuestionRating>();
        [NotMapped, JsonIgnore] public bool Valid => Uid != Guid.Empty && !string.IsNullOrWhiteSpace(Name);
        [NotMapped, JsonIgnore] public bool HasVideos => Videos != null && Videos.Any();
        [NotMapped, JsonIgnore] public bool HasImages => Screenshots != null && Screenshots.Any();
        [NotMapped, JsonIgnore] public bool HasRatings => Ratings?.Count > 0;
        [NotMapped, JsonIgnore] public int NumberOfLikes => Ratings?.Count(x => x.Value) ?? 0;
        [NotMapped, JsonIgnore] public int NumberOfDislikes => Ratings?.Count(x => !x.Value) ?? 0;
        [NotMapped, JsonIgnore] public bool IsValid => UserUid != Guid.Empty && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Text) && CodeLanguageUid != null && CodeLanguageUid != Guid.Empty;
        [NotMapped, JsonIgnore] public bool IsSolved => Solution != null;

        #endregion
        #region Constructors

        public Question()
        {
            CommentSection = new QuestionCommentSection(Uid);
        }

        public Question(User user, string title, string text, CodeLanguage codeLanguage)
            : this()
        {
            UserUid = user.Uid;
            Name = title;
            Text = text;
            CodeLanguageUid = codeLanguage.Uid;
            Logs.Add(new QuestionLog(this, user, "created this"));
        }
        #endregion
        #region Methods
        public override string ToString() => Name;

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
                Ratings = new ObservableCollection<QuestionRating>();
            }

            var rating = Ratings.FirstOrDefault(i => i.User.Equals(user));

            if (rating != null)
            {
                rating.Value = true;
            }
            else
            {
                Ratings.Add(new QuestionRating(user, true));
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
                Ratings = new ObservableCollection<QuestionRating>();
            }

            var rating = Ratings.FirstOrDefault(i => i.User.Equals(user));

            if (rating != null)
            {
                rating.Value = false;
            }
            else
            {
                Ratings.Add(new QuestionRating(user, false));
            }
        }

        public void AddScreenshot(User user, QuestionScreenshot screenshot)
        {
            if (screenshot == null)
            {
                throw new NullReferenceException("Screenshot was null.");
            }
            if (Screenshots == null)
            {
                Screenshots = new ObservableCollection<QuestionScreenshot>();
            }

            Screenshots.Add(screenshot);

            user.IncreaseExperience(Experience.Action.UploadImage);
            Logs.Add(new QuestionLog(this, user, "added", screenshot));
            RefreshBindings();
        }

        public void AddVideo(User user, QuestionVideo video)
        {
            if (video == null || video.Empty)
            {
                throw new NullReferenceException("Video was null.");
            }
            if (Videos == null)
            {
                Videos = new ObservableCollection<QuestionVideo>();
            }

            Videos.Add(video);

            user.IncreaseExperience(Experience.Action.UploadVideo);
            Logs.Add(new QuestionLog(this, user, "added", video));
        }

        public void Reply(User user, Comment comment)
        {
            if (comment == null)
            {
                throw new NullReferenceException("Comment was null.");
            }
            if (CommentSection == null)
            {
                CommentSection = new QuestionCommentSection(Uid);
            }
            if (CommentSection.Replies == null)
            {
                CommentSection.Replies = new SortedObservableCollection<Comment>(c => c.Created, true);
            }

            CommentSection.Replies.Add(comment);

            user.IncreaseExperience(Experience.Action.AddReply);
            Logs.Add(new QuestionLog(this, user, "added", comment));
        }
        #endregion
    }
}
