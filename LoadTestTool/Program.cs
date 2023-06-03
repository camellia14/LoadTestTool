// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string res = await LoadTestTool.Web.Get("https://www.bing.com/");

Console.WriteLine(res);
