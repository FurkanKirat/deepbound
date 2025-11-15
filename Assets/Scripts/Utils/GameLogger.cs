using System;
using System.IO;
using Constants.Paths;
using Utils.Extensions;

namespace Utils
{
    using UnityEngine;

    public static class GameLogger
    {
        
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public static bool EnableInfoLogs = true;
#else
    public static bool EnableInfoLogs = false;
#endif
        public static bool EnableWarnings = true;
        public static bool EnableErrors = true;
        public static void Log(string message, string tag = "Default")
        {
            if (!EnableInfoLogs)
                return;
            string entry = $"[{DateTime.Now.ToLogString()}] [INFO] [{tag}] {message}";

#if UNITY_EDITOR
            Debug.Log(entry);
#endif

            WriteToFile(entry);
        }

        public static void Warn(string message, string tag = "Default")
        {
            if (!EnableWarnings)
                return;
            string entry = $"[{DateTime.Now.ToLogString()}] [WARNING] [{tag}] {message}";

#if UNITY_EDITOR
                Debug.LogWarning(entry);
#endif

            WriteToFile(entry);
        }

        public static void Error(string message, string tag = "Default")
        {
            if (!EnableErrors)
                return;
            string entry = $"[{DateTime.Now.ToLogString()}] [ERROR] [{tag}] {message}";

#if UNITY_EDITOR
                Debug.LogError(entry);
#endif

            WriteToFile(entry);
        }
        
        private static readonly object FileLock = new ();
        private static void WriteToFile(string entry)
        {
            lock (FileLock)
            {
                FileUtils.EnsureDirectoryExists(LogPaths.LogsFolder);
                File.AppendAllText(LogPaths.LogsFile, entry + "\n");
            }
        }

        
    }

}