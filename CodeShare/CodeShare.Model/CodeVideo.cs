using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class CodeVideo : Video
    {
        public virtual Code Code { get; set; }
        public Guid? CodeUid { get; set; }

        public CodeVideo() { }

        public CodeVideo(string youTubeID) : base(youTubeID)
        {
        }
    }
}
