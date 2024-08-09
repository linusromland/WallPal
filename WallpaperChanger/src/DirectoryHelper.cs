namespace WallPal
{
    public static class DirectoryHelper
    {

        public static string GetAppDirectory()
        {
            string appDirectory;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                appDirectory = Path.Combine(documentsPath, Constants.AppName);
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                string homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                appDirectory = Path.Combine(homePath, "." + Constants.AppName.ToLower());
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported platform.");
            }

            CreateDirectory(appDirectory);

            return appDirectory;
        }

        public static string GetPluginsDirectory()
        {
            string pluginDirectory = Path.Combine(GetAppDirectory(), Constants.PluginsDirectoryName);
            CreateDirectory(pluginDirectory);

            return pluginDirectory;
        }

        public static string GetConfigFilePath()
        {
            string configPath = Path.Combine(GetAppDirectory(), Constants.ConfigFileName);
            return configPath;
        }

        private static void CreateDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
