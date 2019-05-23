using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class ProfilePicture : WebImage, ICroppableImage
    {
        public virtual User User { get; set; }
        public Guid? UserUid { get; set; }
        public bool IsPrimary { get; set; }

        public ProfilePicture()
        {
        }

        public ProfilePicture(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 1 / 1, fileInBytes, extension)
        {
        }
    }
}