using System;
using System.Collections.ObjectModel;

namespace CodeShare.Model
{
    public interface IComment : IEntity
    {
        string Text { get; set; }
        User User { get; set; }
        Guid? UserUid { get; set; }
    }

    public interface IComment<T> : IEntity where T : IComment
    {
        string Text { get; set; }
        User User { get; set; }
        Guid? UserUid { get; set; }
        T Parent { get; set; }
        Guid? ParentUid { get; set; }
        SortedObservableCollection<T> Replies { get; set; }
    }
}