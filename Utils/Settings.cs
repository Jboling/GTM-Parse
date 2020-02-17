using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Utils
{
    public static class UserSettings
    {
        private static Settings _settings;
        public static Settings GetSettings()
        {
            if (_settings == null)
                ReadSettings();

            return _settings;
        }

        private static void ReadSettings()
        {
            if (File.Exists("settings.json"))
            {
                using (var reader = new StreamReader("settings.json"))
                {
                    string json = reader.ReadToEnd();
                    _settings = JsonConvert.DeserializeObject<Settings>(json);
                }
            }
            else
            {
                throw new Exception("Could not find settings.json");
            }
        }
    }

    public class Settings
    {
        public string GTMDirectory;
    }
}