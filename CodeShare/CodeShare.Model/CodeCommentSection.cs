using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public class CodeCommentSection : CommentSection
    {
        public Code Code { get; set; }
        public Guid CodeUid { get; set; }
        public CodeCommentSection()
        {

        }
        public CodeCommentSection(Guid codeUid)
        {
            CodeUid = codeUid;
        }
    }
}
