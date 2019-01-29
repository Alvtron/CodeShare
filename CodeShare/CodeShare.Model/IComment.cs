using System;
using System.Collections.ObjectModel;

namespace CodeShare.Model
{
    public interface IComment
    {
        Content Content { get; set; }
        Guid ContentUid { get; set; }
        bool HasLikes { get; }
        bool HasRatings { get; }
        bool HasReplies { get; }
        ObservableCollection<CommentLog> Logs { get; set; }
        int NumberOfDislikes { get; }
        int NumberOfLikes { get; }
        int NumberOfReplies { get; }
        ObservableCollection<Rating> Ratings { get; set; }
        ObservableCollection<Reply> Replies { get; set; }
        string Text { get; set; }
        User User { get; set; }
        Guid UserUid { get; set; }

        void Dislike(User user);
        bool HasLiked(User user);
        bool HasRated(User user);
        void Like(User user);
        void Reply(Reply reply);
        string ToString();
    }
}