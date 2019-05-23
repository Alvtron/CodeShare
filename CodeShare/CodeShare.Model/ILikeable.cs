using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeShare.Model
{
    public interface ILikeable<T> : IEntity where T : IRating, new()
    {
        ObservableCollection<T> Ratings { get; set; }
        bool HasRatings { get; }
        int NumberOfLikes { get; }
        int NumberOfDislikes { get; }
        bool HasLiked(User user);
        bool HasDisliked(User user);
        bool HasRated(User user);
        void ToggleLike(User user);
        void Like(User user);
        void Dislike(User user);
    }
}