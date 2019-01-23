namespace CodeShare.Model
{
    public interface IWebImage
    {
        double AspectRatio { get; }
        int Height { get; set; }
        int Width { get; set; }
    }
}