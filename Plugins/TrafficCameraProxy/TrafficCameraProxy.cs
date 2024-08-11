using Interfaces;
using Newtonsoft.Json.Linq;
using NLog;

namespace TrafficCameraProxy
{
    public struct ConfigStruct
    {
        public string serverURL;
        public string cameraId;
    }

    public class TrafficCameraProxy : IPlugin
    {
        private readonly Logger _logger;
        private readonly IApplicationServices _appServices;
        public string Name { get; } = "TrafficCameraProxy";

        public TrafficCameraProxy(IApplicationServices appServices)
        {
            _logger = appServices.GetLogger();
            _appServices = appServices;
        }


        public bool IsReady()
        {
            string? serverURL = GetURL();
            if (serverURL == null)
            {
                _logger.Error("Missing serverURL or cameraId in config");
                return false;
            }

            using HttpClient client = new();
            try
            {
                HttpResponseMessage response = client.GetAsync(serverURL).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
                _logger.Error("Failed to connect to server");
                return false;
            }
        }

        public JObject GetDefaultConfig()
        {
            ConfigStruct config = new()
            {
                serverURL = "https://trafficcamera.markus.romland.dev/",
                cameraId = "SE_STA_CAMERA_Orion_4500003"
            };

            return JObject.FromObject(config);
        }

        private string? GetURL()
        {
            JObject config = _appServices.GetConfig(GetDefaultConfig());
            string? serverURL = config?["serverURL"]?.ToString();
            string? cameraId = config?["cameraId"]?.ToString();

            if (serverURL == null || cameraId == null)
            {
                return null;
            }

            if (serverURL.EndsWith("/"))
            {
                serverURL = serverURL.Substring(0, serverURL.Length - 1);
            }

            return $"{serverURL}/{cameraId}/latest";
        }



        public Stream GetWallpaperStream()
        {
            string? url = GetURL() ?? throw new Exception("Missing serverURL or cameraId in config");

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            _logger.Info($"Got image from {url}");
            return response.Content.ReadAsStream();
        }

    }
}
