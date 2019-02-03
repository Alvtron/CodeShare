using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Screenshot : WebImage, ICroppableImage
    {
        public Content Content { get; set; }
        public Guid? ContentUid { get; set; }

        public Screenshot()
        {
        }

        public Screenshot(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 16 / 9, fileInBytes, extension)
        {
        }
    }
}