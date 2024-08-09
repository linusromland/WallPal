using Interfaces;

namespace WallpaperChanger
{
    public class ApplicationServices : IApplicationServices
    {
        public string GetAppDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
