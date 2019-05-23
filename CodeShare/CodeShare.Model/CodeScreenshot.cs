using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class CodeScreenshot : WebImage
    {
        public virtual Code Code { get; set; }
        public Guid? CodeUid { get; set; }

        public CodeScreenshot()
        {
        }

        public CodeScreenshot(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 16 / 9, fileInBytes, extension)
        {
        }
    }
}