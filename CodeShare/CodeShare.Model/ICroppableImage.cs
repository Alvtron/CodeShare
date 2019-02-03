namespace CodeShare.Model
{
    public interface ICroppableImage : IWebImage
    {
        Crop Crop { get; set; }
        void CreateCrop(double aspectRatio);
    }
}