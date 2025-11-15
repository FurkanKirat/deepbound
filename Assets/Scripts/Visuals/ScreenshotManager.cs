using System;
using UnityEngine;
using Utils;
using Utils.Extensions;

namespace Visuals
{
    public static class ScreenshotManager
    {
        public static void TakeScreenshot()
        {
            var fileName = DateTime.UtcNow.ToFileTimeString();
            var path = ScreenshotUtils.GetScreenshotFilePath(fileName);
            var info = FileUtils.ParseFilename(path);
            FileUtils.EnsureDirectoryExists(info.directory);
            ScreenCapture.CaptureScreenshot(path);
            GameLogger.Log($"Screenshot saved to {path}", "ScreenshotManager");
        }
    }
}