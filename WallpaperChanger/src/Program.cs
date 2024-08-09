using System;

namespace WallpaperChangerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            WallpaperChanger.WallpaperChanger wallpaperChanger = new WallpaperChanger.WallpaperChanger();
            wallpaperChanger.ChangeWallpaper("C:\\Users\\hello\\Pictures\\Screenshots\\Screenshot 2024-07-13 195347.png");
        }
    }
}
