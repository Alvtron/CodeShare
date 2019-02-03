namespace CodeShare.Model
{
    public interface IWebImage : IWebFile
    {
        double AspectRatio { get; }
        int Height { get; set; }
        int Width { get; set; }
    }
}