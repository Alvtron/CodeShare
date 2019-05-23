using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class UserBanner : WebImage
    {
        public virtual User User { get; set; }
        public Guid? UserUid { get; set; }
        public bool IsPrimary { get; set; }

        public UserBanner()
        {
        }

        public UserBanner(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 20 / 8, fileInBytes, extension)
        {
            
        }
    }
}