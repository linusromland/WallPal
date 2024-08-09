using Interfaces;

namespace SamplePlugin;

public class SamplePlugin : IPlugin
{
    public string Name { get; } = "SamplePlugin";

    public string GetImagePath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Plugins", "sample_image.jpg");
    }

    public bool IsReady()
    {
        string imagePath = GetImagePath();
        return File.Exists(imagePath);
    }

    public Stream GetWallpaperStream()
    {
        string imagePath = GetImagePath();
        return File.OpenRead(imagePath);
    }

}
