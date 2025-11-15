using System.IO;
using UnityEngine;

namespace Constants.Paths
{
    public static class LogPaths
    {
        public static readonly string LogsFolder = Path.Combine(Application.persistentDataPath, "logs");
        public static readonly string LogsFile = Path.Combine(LogsFolder, "logs.txt");
    }
}