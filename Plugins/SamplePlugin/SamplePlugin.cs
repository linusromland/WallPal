using Interfaces;
using Newtonsoft.Json.Linq;

namespace SamplePlugin;

public struct ConfigStruct
{
    public string ImagePath;
}

public class SamplePlugin(IApplicationServices appServices) : IPlugin
{
    private readonly IApplicationServices _appServices = appServices;
    public string Name { get; } = "SamplePlugin";

    private string GetImagePath()
    {
        JObject config = _appServices.GetConfig(GetDefaultConfig());

        string? imagePath = config?["ImagePath"]?.ToString();
        if (imagePath != null)
        {
            return imagePath;
        }

        return "";

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

    public JObject GetDefaultConfig()
    {
        ConfigStruct config = new ConfigStruct
        {
            ImagePath = ""
        };

        return JObject.FromObject(config);
    }

}
