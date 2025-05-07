// 拡張子が.iniのファイルを読み込む

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace LoadTestTool
{
    public class Config
    {
        public string FilePath { get; set; }
        // セクションとその設定値を格納するディクショナリ
        private Dictionary<string, Dictionary<string, string>> Sections { get; set; }

        public Config(string filePath)
        {
            FilePath = filePath;
            Sections = new Dictionary<string, Dictionary<string, string>>();
            // デフォルトセクションを追加
            Sections[""] = new Dictionary<string, string>();
            Load(filePath);
        }
        
        // ファイルを読み込む
        public void Load(string filePath)
        {
            Console.WriteLine("Load:" + filePath);
            try
            {
                FilePath = filePath;
                if (!File.Exists(FilePath))
                {
                    throw new FileNotFoundException($"設定ファイルが見つかりません: {FilePath}");
                }

                Console.WriteLine("ReadAllLines:");
                var lines = File.ReadAllLines(FilePath, Encoding.UTF8);
                string currentSection = "";
                Sections.Clear();
                Sections[""] = new Dictionary<string, string>();

                foreach (var line in lines)
                {
                    Console.WriteLine("line:" + line);
                    // コメント行は無視する
                    if (line.StartsWith(";")) continue;

                    // 空行は無視する
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // セクション行を処理する
                    var sectionMatch = Regex.Match(line, @"^\[(.*?)\]$");
                    if (sectionMatch.Success)
                    {
                        currentSection = sectionMatch.Groups[1].Value;
                        if (!Sections.ContainsKey(currentSection))
                        {
                            Sections[currentSection] = new Dictionary<string, string>();
                        }
                        continue;
                    }

                    // キーと値を取得する
                    var parts = line.Split('=');
                    if (parts.Length != 2)
                    {
                        Console.WriteLine($"警告: 不正な行をスキップします: {line}");
                        continue;
                    }

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    // セクションに設定値を格納する
                    Sections[currentSection][key] = value;
                    Console.WriteLine($"セクション: {currentSection}, key: {key}, value: {value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"設定ファイルの読み込み中にエラーが発生しました: {ex.Message}");
                throw;
            }
        }

        // 設定値を取得する（文字列）
        public string Get(string key, string section = "")
        {
            if (!Sections.ContainsKey(section))
            {
                throw new KeyNotFoundException($"セクションが見つかりません: {section}");
            }
            if (!Sections[section].ContainsKey(key))
            {
                throw new KeyNotFoundException($"設定キーが見つかりません: {key} (セクション: {section})");
            }
            return Sections[section][key];
        }

        // 設定値を取得する（整数）
        public int GetInt(string key, string section = "")
        {
            if (!int.TryParse(Get(key, section), out int result))
            {
                throw new FormatException($"設定値 {key} を整数に変換できません (セクション: {section})");
            }
            return result;
        }

        // 設定値を取得する（真偽値）
        public bool GetBool(string key, string section = "")
        {
            var value = Get(key, section).ToLower();
            return value == "true" || value == "1" || value == "yes";
        }

        // 設定値が存在するかチェックする
        public bool HasKey(string key, string section = "")
        {
            return Sections.ContainsKey(section) && Sections[section].ContainsKey(key);
        }

        // 設定値を保存する
        public void Save()
        {
            try
            {
                var lines = new List<string>();
                foreach (var section in Sections)
                {
                    // セクション名が空でない場合は、セクション行を追加
                    if (!string.IsNullOrEmpty(section.Key))
                    {
                        lines.Add($"[{section.Key}]");
                    }

                    // セクション内の設定値を追加
                    foreach (var setting in section.Value)
                    {
                        lines.Add($"{setting.Key}={setting.Value}");
                    }
                }

                File.WriteAllLines(FilePath, lines, Encoding.UTF8);
                Console.WriteLine($"設定を保存しました: {FilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"設定の保存中にエラーが発生しました: {ex.Message}");
                throw;
            }
        }

        // 設定値を更新する
        public void Set(string key, string value, string section = "")
        {
            if (!Sections.ContainsKey(section))
            {
                Sections[section] = new Dictionary<string, string>();
            }
            Sections[section][key] = value;
        }

        // セクションが存在するかチェックする
        public bool HasSection(string section)
        {
            return Sections.ContainsKey(section);
        }

        // セクション内のすべてのキーを取得する
        public string[] GetKeys(string section = "")
        {
            if (!Sections.ContainsKey(section))
            {
                throw new KeyNotFoundException($"セクションが見つかりません: {section}");
            }
            return Sections[section].Keys.ToArray();
        }

        // すべてのセクション名を取得する
        public string[] GetSections()
        {
            return Sections.Keys.Where(k => !string.IsNullOrEmpty(k)).ToArray();
        }
    }
}
