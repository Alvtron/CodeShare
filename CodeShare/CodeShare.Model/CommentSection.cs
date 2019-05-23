using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public abstract class CommentSection : Entity, ICommentSection
    {
        public virtual SortedObservableCollection<Comment> Replies { get; set; } = new SortedObservableCollection<Comment>(c => c.Created, true);
    }
}
