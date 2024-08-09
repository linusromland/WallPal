using System.Runtime.InteropServices;

namespace WallPal
{
    public class WallpaperChanger
    {
        private readonly IWallpaperManager _wallpaperManager;

        public WallpaperChanger()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _wallpaperManager = new Windows.WindowsWallpaperManager();
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not implemented yet.");
            }
        }

        public void ChangeWallpaper(Stream imageStream)
        {
            string tempFilePath = Path.GetTempFileName() + Path.GetExtension(Path.GetTempFileName());
            using (var fileStream = File.Create(tempFilePath))
            {
                imageStream.CopyTo(fileStream);
            }

            _wallpaperManager.SetWallpaper(tempFilePath);

            File.Delete(tempFilePath);
        }
    }
}
