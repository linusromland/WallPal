using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace WallPal
{
    public class WallpaperChanger
    {
        private string lastHash = string.Empty;
        private readonly IWallpaperManager wallpaperManager;

        public WallpaperChanger()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                wallpaperManager = new Windows.WindowsWallpaperManager();
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not implemented yet.");
            }
        }

        public void ChangeWallpaper(Stream imageStream)
        {
            string hash = ComputeHashFromStream(imageStream);
            if (hash == lastHash && !string.IsNullOrEmpty(lastHash))
            {
                Console.WriteLine("Image has not changed. Skipping wallpaper change.");
                return;
            }

            lastHash = hash;

            string tempFilePath = Path.GetTempFileName() + Path.GetExtension(Path.GetTempFileName());
            using (var fileStream = File.Create(tempFilePath))
            {
                imageStream.CopyTo(fileStream);
            }

            wallpaperManager.SetWallpaper(tempFilePath);
        }

        static string ComputeHashFromStream(Stream stream)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(stream);

                // Reset the stream position to the beginning
                stream.Position = 0;

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

    }
}
