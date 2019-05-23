using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class QuestionCommentSection : CommentSection
    {
        public Question Question { get; set; }
        public Guid QuestionUid { get; set; }
        public QuestionCommentSection()
        {

        }
        public QuestionCommentSection(Guid questionUid)
        {
            QuestionUid = questionUid;
        }
    }
}
