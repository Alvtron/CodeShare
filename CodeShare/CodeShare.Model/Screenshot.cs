using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class Screenshot : WebImage, IContentImage, ICroppableImage
    {
        public Content Content { get; set; }
        public Guid ContentUid { get; set; }
        public Crop Crop { get; set; }

        public Screenshot()
        {
        }

        public Screenshot(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, fileInBytes, extension)
        {
            CreateNewCrop();
        }

        public void CreateNewCrop()
        {
            var aspectRatio_X = 16;
            var aspectRatio_Y = 9;
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