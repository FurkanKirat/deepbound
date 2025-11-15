using System.IO;
using Constants.Paths;

namespace Utils
{
    public static class ScreenshotUtils
    {
        public static string GetScreenshotFilePath(string screenshotName)
            => Path.Combine(SaveConstants.ScreenshotsFolder, screenshotName + ".png");
    }

}