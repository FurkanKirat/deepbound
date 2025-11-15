using System;
using System.IO;
using System.Text;

namespace Utils
{
    public static class FileUtils
    {
        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }
        

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            CreateDirectoryIfNotExists(path);
            File.WriteAllBytes(path, bytes);
        }

        public static void CreateDirectoryIfNotExists(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        

        public static void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static void DeleteDirectoryIfExists(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public static string GetSafePath(string path)
        {
            return Path.GetFullPath(path).Replace('\\', '/');
        }

        public static string CreateUniqueFilename(string baseDirectory, string baseName, string extension)
        {
            return CreateUnique(baseDirectory, baseName, false, extension);
        }

        public static string CreateUniqueDirectoryName(string baseDirectory, string directoryName)
        {
            return CreateUnique(baseDirectory, directoryName, true);
        }

        private static string CreateUnique(string baseDirectory, string name, bool isDirectory, string extension = null)
        {
            EnsureDirectoryExists(baseDirectory);
            int counter = 0;
            string path;
            string newName;
            do
            {
                string suffix = counter == 0 ? "" : $"_{counter}";
                newName = $"{name}{suffix}";
                if (isDirectory)
                {
                    path = Path.Combine(baseDirectory, newName);
                }
                else
                {
                    path = Path.Combine(baseDirectory, extension == null ? newName : $"{newName}.{extension}");
                }
                
                counter++;
            } while (isDirectory ? Directory.Exists(path) : File.Exists(path));
            return newName;
        }

        public static string CombinePath(params string[] parts)
        {
            return Path.Combine(parts).Replace('\\', '/');
        }
        
        public static void SaveText(string path, string content, Encoding encoding = null)
        {
            try
            {
                EnsureDirectoryExists(Path.GetDirectoryName(path));
                File.WriteAllText(path, content, encoding ?? Encoding.UTF8);
                GameLogger.Log($"Saved: {path}",nameof(FileUtils));
            }
            catch (Exception ex)
            {
                GameLogger.Error($"Failed to save: {path}\n{ex}",nameof(FileUtils));
            }
            
        }
        
        public static string Load(string path, Encoding encoding = null)
        {
            try
            {
                if (!File.Exists(path))
                {
                    GameLogger.Warn($"File not found: {path}", nameof(FileUtils));
                    return null;
                }

                var text = File.ReadAllText(path, encoding ?? Encoding.UTF8);
                GameLogger.Log($"Loaded: {path}", nameof(FileUtils));
                return text;
            }
            catch (Exception ex)
            {
                GameLogger.Error($"Failed to load: {path}\n{ex}", nameof(FileUtils));
                return null;
            }
        }

        public static (string extension, string fileName, string directory) ParseFilename(string pathWithExtension)
        {
            var extension = Path.GetExtension(pathWithExtension);
            var fileName = Path.GetFileNameWithoutExtension(pathWithExtension);
            var directory = Path.GetDirectoryName(pathWithExtension);
            return (extension, fileName, directory);
        }

        public static void CopyDirectory(string source, string destination, bool overwrite)
        {
            EnsureDirectoryExists(destination);

            foreach (var file in Directory.GetFiles(source))
            {
                string destinationFile = Path.Combine(destination, file);
                File.Copy(file, destinationFile, overwrite);
            }

            foreach (var directory in Directory.GetDirectories(source))
            {
                string destinationDirectory = Path.Combine(destination, directory);
                CopyDirectory(directory, destinationDirectory, overwrite);
            }
        }

        public static void CutDirectory(string source, string destination, bool overwrite)
        {
            CopyDirectory(source, destination, overwrite);
            Directory.Delete(source, true);
        }
    }
}