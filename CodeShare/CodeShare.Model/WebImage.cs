using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CodeShare.Model
{
    public class WebImage : WebFile, IWebImage
    {
        public int Width { get; set; }
        public int Height { get; set; }

        [NotMapped, JsonIgnore]
        public double AspectRatio => Width / Height;

        public WebImage()
        {
        }

        public WebImage(int width, int height, byte[] fileInBytes, string extension)
            : base(fileInBytes, extension)
        {
            Width = width;
            Height = height;
        }
    }
}