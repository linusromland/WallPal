using Newtonsoft.Json.Linq;

namespace Interfaces;

public interface IPlugin
{
    string Name { get; }
    bool IsReady();
    Stream GetWallpaperStream();

    JObject GetDefaultConfig();
}
