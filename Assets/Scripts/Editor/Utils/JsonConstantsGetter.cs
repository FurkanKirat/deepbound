using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Editor.Utils
{
    public static class JsonFieldExtractor
    {
        public static Dictionary<string, string> ExtractConstantsFromJson(string[] folders, string[] fields)
        {
            var result = new Dictionary<string, string>();

            foreach (var folder in folders)
            {
                if (!Directory.Exists(folder)) continue;

                var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var text = File.ReadAllText(file);
                    var json = JObject.Parse(text);

                    foreach (var field in fields)
                    {
                        if (json.TryGetValue(field, out var token))
                        {
                            if (token.Type == JTokenType.Array)
                            {
                                foreach (var el in token)
                                    TryAdd(result, el.ToString());
                            }
                            else if (token.Type == JTokenType.String)
                            {
                                TryAdd(result, token.ToString());
                            }
                        }
                    }
                }
            }

            return result;
        }
        
        public static Dictionary<string, string> ExtractSettingKeys(string path)
        {
            var result = new Dictionary<string, string>();
            
            var text = File.ReadAllText(path);
            var json = JObject.Parse(text);

            // panels -> settings -> key
            if (json["panels"] is not JArray panels) return result;

            foreach (var panel in panels)
            {
                if (panel["settings"] is not JArray settings) continue;

                foreach (var setting in settings)
                {
                    var key = setting["key"]?.ToString();
                    if (string.IsNullOrEmpty(key)) continue;

                    result.TryAdd(key, key);
                }
            }
            
            return result;
        }

        public static Dictionary<string, string> ExtractAllFields(string path)
        {
            var result = new Dictionary<string, string>();
            
            var text = File.ReadAllText(path);
            var json = JObject.Parse(text);
            foreach (var field in json)
                result.TryAdd(field.Key, field.Value.ToString());
            return result;
        }


        private static void TryAdd(Dictionary<string, string> dict, string value)
        {
            var cleaned = value.Replace(":", ".").Replace("-", "_"); // Optionally handle other symbols
            var key = EditorUtils.ToPascalCase(cleaned);

            dict.TryAdd(key, value);
        }
    }
}