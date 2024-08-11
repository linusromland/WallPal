using Interfaces;
using Newtonsoft.Json.Linq;
using NLog;

namespace SamplePlugin;

public struct ConfigStruct
{
    public string imagePath;
}

public class SamplePlugin : IPlugin
{
    private readonly Logger _logger;
    private readonly IApplicationServices _appServices;


    public string Name { get; } = "SamplePlugin";

    public SamplePlugin(IApplicationServices appServices)
    {
        _logger = appServices.GetLogger();
        _appServices = appServices;
    }


    private string GetImagePath()
    {
        JObject config = _appServices.GetConfig(GetDefaultConfig());

        string? imagePath = config?["imagePath"]?.ToString();
        if (imagePath != null)
        {
            return imagePath;
        }

        _logger.Error("Missing imagePath in config");
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
        ConfigStruct config = new()
        {
            imagePath = ""
        };

        return JObject.FromObject(config);
    }

}
