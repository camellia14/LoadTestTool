// See https://aka.ms/new-console-template for more information
using LoadTestTool;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

Console.WriteLine("負荷テストツールを開始します");

// 設定ファイルの読み込み
var config = new Config("LoadTestTool/config.ini");

int botCount = config.GetInt("BotCount", "General");
int interval = config.GetInt("Interval", "General");

Console.WriteLine($"ボット数: {botCount}");
Console.WriteLine($"インターバル: {interval}ms");

var bots = new List<Task>();

// ボットを起動
for (int i = 0; i < botCount; i++)
{
    var scenario = new Scenario();
    bots.Add(scenario.SampleScenario());
    await Task.Delay(interval);
}

// すべてのボットの完了を待機
await Task.WhenAll(bots);

Console.WriteLine("すべてのシナリオが完了しました");










