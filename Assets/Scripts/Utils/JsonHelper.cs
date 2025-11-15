using System;
using System.Text;
using Newtonsoft.Json;

namespace Utils
{
    public static class JsonHelper
    {
        private const Formatting Format
        #if UNITY_EDITOR
            = Formatting.Indented;
        #else
            = Formatting.None;
        #endif

        private static readonly JsonSerializerSettings JsonSettings = new ()
        {
            Formatting = Format,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
        
        public static bool SaveRaw<T>(string path, T data)
        {
            try
            {
                string json = Serialize(data);
                FileUtils.SaveText(path, json);
                return true;
            }
            catch (Exception ex)
            {
                GameLogger.Error($"Could not save file at {path}: {ex.Message}", nameof(JsonHelper));
                return false;
            }
        }

        public static T LoadRaw<T>(string path)
        {
            string json = FileUtils.Load(path);
            return Deserialize<T>(json);
        }
        
        public static bool TryLoadRaw<T>(string path, out T result, Encoding encoding = null)
        {
            result = default;

            string json = FileUtils.Load(path, encoding);
            if (string.IsNullOrWhiteSpace(json))
            {
                GameLogger.Warn($"JSON boş veya dosya bulunamadı: {path}",nameof(JsonHelper));
                return false;
            }

            try
            {
                result = Deserialize<T>(json);
                return true;
            }
            catch (Exception ex)
            {
                GameLogger.Error($"JSON deserialize error ({typeof(T).Name}) → {path}\n{ex}", nameof(JsonHelper));
                return false;
            }
        }

        public static string Serialize<T>(T data)
        {
            try
            {
                return JsonConvert.SerializeObject(data, JsonSettings);
            }
            catch (Exception e)
            {
                GameLogger.Error($"Failed to serialize object of type {typeof(T)} \n{e}", nameof(JsonHelper));
                throw;
            }
            
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSettings);
        }

        public static T[] DeserializeArray<T>(string json)
        {
            return Deserialize<T[]>(json);
        }

        public static bool IsArray(string json)
        {
            var trimmed = json.TrimStart().TrimEnd();
            return trimmed.StartsWith("[") && trimmed.EndsWith("]");
        }
    }

}