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
    public class Reply : Comment
    {
        public Content Content { get; set; }
        public Guid? ContentUid { get; set; }

        public ObservableCollection<Reply> Replies { get; set; } = new ObservableCollection<Reply>();

        public Reply()
        {
        }

        public Reply(User user, string text)
        {
            UserUid = user.Uid;
            Text = text;

            Logs.Add(new CommentLog(this, user, "created this"));
        }

        public Reply(Guid contentUid, User user, string text)
            : this(user, text)
        {
            ContentUid = contentUid;
        }

        public void AddReply(Reply reply)
        {
            if (reply == null)
            {
                throw new NullReferenceException("Comment was null.");
            }
            if (Replies == null)
            {
                Replies = new ObservableCollection<Reply>();
            }

            Replies.Add(reply);
            Logs.Add(new CommentLog(this, reply.User, "replied", reply));
        }
    }
}
