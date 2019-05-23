using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class QuestionScreenshot : WebImage
    {
        public virtual Question Question { get; set; }
        public Guid? QuestionUid { get; set; }

        public QuestionScreenshot()
        {
        }

        public QuestionScreenshot(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 16 / 9, fileInBytes, extension)
        {
        }
    }
}