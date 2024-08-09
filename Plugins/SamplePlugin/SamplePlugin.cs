using Interfaces;

namespace SamplePlugin;

public class SamplePlugin(IApplicationServices appServices) : IPlugin
{
    private readonly IApplicationServices _appServices = appServices;
    public string Name { get; } = "SamplePlugin";

    private string GetImagePath()
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
        Console.WriteLine("Getting wallpaper stream from SamplePlugin. App directory: " + _appServices.GetAppDirectory());
        string imagePath = GetImagePath();
        return File.OpenRead(imagePath);
    }

}
