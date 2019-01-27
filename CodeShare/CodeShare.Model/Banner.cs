using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Banner : WebImage, ICroppableImage
    {
        public Content Content { get; set; }
        public Guid ContentUid { get; set; }
        public bool IsPrimary { get; set; }
        private Crop _crop;
        public Crop Crop
        {
            get => _crop;
            set => SetField(ref _crop, value);
        }

        public Banner()
        {
        }

        public Banner(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, fileInBytes, extension)
        {
            CreateNewCrop();
        }

        public void CreateNewCrop()
        {
            var aspectRatio_X = 20;
            var aspectRatio_Y = 8;
            var ratio = aspectRatio_X / aspectRatio_Y;

            var croppedWidth = Width * 0.9;
            var croppedHeight = Height * 0.9;

            var suggestedHeight = croppedWidth / ratio;
            var suggestedWidth = croppedHeight * ratio;

            if (suggestedHeight <= croppedHeight)
            {
                Crop = new Crop(Width / 2, Height / 2, (int)croppedWidth, (int)suggestedHeight, ratio);
            }
            else
            {
                Crop = new Crop(Width / 2, Height / 2, (int)suggestedWidth, (int)croppedHeight, ratio);
            }
        }
    }
}