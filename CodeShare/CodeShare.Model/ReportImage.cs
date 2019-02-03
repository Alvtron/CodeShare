using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class ReportImage : WebImage, ICroppableImage
    {
        public Report Report { get; set; }
        public Guid ReportUid { get; set; }

        public ReportImage()
        {
        }

        public ReportImage(int width, int height, byte[] fileInBytes, string extension)
            : base(width, height, fileInBytes, extension)
        {
        }
    }
}