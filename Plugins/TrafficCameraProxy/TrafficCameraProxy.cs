using Interfaces;
using Newtonsoft.Json.Linq;

namespace TrafficCameraProxy
{
    public struct ConfigStruct
    {
        public string serverURL;
        public string cameraId;
    }

    public class TrafficCameraProxy(IApplicationServices appServices) : IPlugin
    {
        private readonly IApplicationServices _appServices = appServices;
        public string Name { get; } = "TrafficCameraProxy";


        public bool IsReady()
        {
            string? serverURL = GetURL();
            if (serverURL == null)
            {
                return false;
            }

            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = client.GetAsync(serverURL).Result;

                return response.IsSuccessStatusCode;
            }
            catch
            {
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
            return response.Content.ReadAsStream();
        }

    }
}
