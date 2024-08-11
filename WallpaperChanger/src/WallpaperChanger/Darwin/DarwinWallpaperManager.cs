using System;
using System.Diagnostics;
using System.IO;
using NLog;

namespace WallPal.Darwin
{
    public class DarwinWallpaperManager : IWallpaperManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void SetWallpaper(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Image path cannot be null or empty.");

            _logger.Info($"Setting wallpaper to: {imagePath}");

            string script = $"tell application \"System Events\" to set picture of every desktop to \"{imagePath}\"";

            RunAppleScript(script);
        }

        private void RunAppleScript(string script)
        {
            string tempScriptFile = Path.GetTempFileName();
            File.WriteAllText(tempScriptFile, script);

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "osascript",
                        Arguments = $"\"{tempScriptFile}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception($"Error setting wallpaper: {error}");
                }
            }
            finally
            {
                if (File.Exists(tempScriptFile))
                {
                    File.Delete(tempScriptFile);
                }
            }
        }
    }
}