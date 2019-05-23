using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class CodeBanner : WebImage
    {
        public virtual Code Code { get; set; }
        public Guid? CodeUid { get; set; }
        public bool IsPrimary { get; set; }

        public CodeBanner()
        {
        }

        public CodeBanner(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 20 / 8, fileInBytes, extension)
        {
            
        }
    }
}