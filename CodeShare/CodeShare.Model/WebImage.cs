using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CodeShare.Model
{
    public class WebImage : WebFile, IWebImage, ICroppableImage
    {
        public int Width { get; set; }
        public int Height { get; set; }
        private Crop _crop;
        public Crop Crop
        {
            get => _crop;
            set => SetField(ref _crop, value);
        }

        [NotMapped, JsonIgnore]
        public double AspectRatio => Width / Height;

        public WebImage()
        {
        }

        public WebImage(int width, int height, byte[] fileInBytes, string extension)
            : this(width, height, width / height, fileInBytes, extension)
        {
        }

        public WebImage(int width, int height, double cropAspectRatio, byte[] fileInBytes, string extension)
            : base(fileInBytes, extension)
        {
            Width = width;
            Height = height;

            CreateCrop(cropAspectRatio);
        }

        public void CreateCrop(double aspectRatio)
        {
            var croppedWidth = Width * 0.9;
            var croppedHeight = Height * 0.9;

            var suggestedHeight = croppedWidth / aspectRatio;
            var suggestedWidth = croppedHeight * aspectRatio;

            if (suggestedHeight <= croppedHeight)
            {
                Crop = new Crop(Width / 2, Height / 2, (int)croppedWidth, (int)suggestedHeight, aspectRatio);
            }
            else
            {
                Crop = new Crop(Width / 2, Height / 2, (int)suggestedWidth, (int)croppedHeight, aspectRatio);
            }
        }
    }
}