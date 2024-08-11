using Newtonsoft.Json.Linq;
using NLog;

namespace Interfaces;

public interface IApplicationServices
{
    string GetAppDirectory();

    JObject GetConfig(JObject defaultConfig);

    Logger GetLogger(string name);
    Logger GetLogger();
}