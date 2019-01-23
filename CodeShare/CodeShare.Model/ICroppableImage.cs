namespace CodeShare.Model
{
    public interface ICroppableImage
    {
        Crop Crop { get; set; }
        void CreateNewCrop();
    }
}