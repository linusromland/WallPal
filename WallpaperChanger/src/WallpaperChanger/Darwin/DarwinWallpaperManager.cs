using System;
using System.Diagnostics;
using System.IO;

namespace WallPal.Darwin
{
    public class DarwinWallpaperManager : IWallpaperManager
    {
        public void SetWallpaper(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Image path cannot be null or empty.");

            Console.WriteLine($"Setting macOS wallpaper to: {imagePath}");

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

                Console.WriteLine(output);
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