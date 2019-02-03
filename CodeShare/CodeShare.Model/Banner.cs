using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Banner : WebImage
    {
        public Content Content { get; set; }
        public Guid ContentUid { get; set; }
        public bool IsPrimary { get; set; }

        public Banner()
        {
        }

        public Banner(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, 20 / 8, fileInBytes, extension)
        {
            
        }
    }
}