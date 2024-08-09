using System;
using System.Runtime.InteropServices;

namespace WallPal.Windows
{
    public class WindowsWallpaperManager : IWallpaperManager
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        public void SetWallpaper(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Image path cannot be null or empty.");

            Console.WriteLine($"Setting Windows wallpaper to: {imagePath}");
            int result = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

            if (result == 0)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
