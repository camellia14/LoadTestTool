// See https://aka.ms/new-console-template for more information
using LoadTestTool;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

Console.WriteLine("Hello, World!");

string res = await LoadTestTool.Web.Get("https://www.bing.com/");

var settings = new LoadTestTool.Settings(args);

Console.WriteLine(res);
