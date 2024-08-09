using System.Runtime.InteropServices;

namespace WallpaperChanger
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

        public void ChangeWallpaper(string imagePath)
        {
            _wallpaperManager.SetWallpaper(imagePath);
        }
    }
}
