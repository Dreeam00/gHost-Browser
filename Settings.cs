using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace IFoxer
{
    public class Bookmark
    {
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string Icon { get; set; } = "";
    }

    public class Settings
    {
        public string ThemeColor { get; set; } = "#007ACC";
        public string SearchEngine { get; set; } = "https://www.google.com/search?q=";
        public string HomePage { get; set; } = "https://www.google.com";
        public bool EnableDarkMode { get; set; } = false;
        public bool EnableAdBlock { get; set; } = true;
        public bool EnableJavaScript { get; set; } = true;
        public bool EnableImages { get; set; } = true;
        public int DefaultZoomLevel { get; set; } = 100;
        public bool EnableAutoComplete { get; set; } = true;
        public bool EnableHistory { get; set; } = true;
        public bool EnableCookies { get; set; } = true;
        public bool VerticalTabs { get; set; } = false;
        public double Opacity { get; set; } = 1.0;
        public List<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public string DownloadPath { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
        public bool EnableGhostMode { get; set; } = false;
        public double GhostOpacityInactive { get; set; } = 0.7;
        public double GhostOpacityActive { get; set; } = 1.0;
        // 常に前面モード
        public bool AlwaysOnTop { get; set; } = false;
        // シークレットモード
        public bool EnableSecretMode { get; set; } = false;
        // シークレットモードアイコンパス
        public string SecretIconPath { get; set; } = null;

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "IFoxer",
            "settings.json"
        );

        public static Settings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
            }
            catch (Exception)
            {
                // エラーが発生した場合はデフォルト設定を返す
            }
            return new Settings();
        }

        public void Save()
        {
            try
            {
                string directory = Path.GetDirectoryName(SettingsPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception)
            {
                // 保存に失敗した場合のエラー処理
            }
        }
    }
} 