using Newtonsoft.Json.Linq;

namespace Interfaces;

public interface IApplicationServices
{
    string GetAppDirectory();

    JObject GetConfig(JObject defaultConfig);
}