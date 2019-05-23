namespace CodeShare.Model
{
    public interface ICommentSection
    {
        SortedObservableCollection<Comment> Replies { get; set; }
    }
}