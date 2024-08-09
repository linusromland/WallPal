namespace Interfaces;

public interface IPlugin
{
    string Name { get; }
    bool IsReady();
    Stream GetWallpaperStream();
}
