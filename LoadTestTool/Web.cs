using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTestTool
{
    internal class Web
    {
        private static FormUrlEncodedContent? Convert(string data)
        {
            var dic = new Dictionary<string, string>();
            var list = data.Split('&');
            foreach (var pair_str in list)
            {
                var pair = pair_str.Split('=');
                if (pair.Length != 2) continue;
                dic.Add(pair[0], pair[1]);
            }
            if (dic.Count > 0)
            {
                return new FormUrlEncodedContent(dic);
            }
            return null;
        }
        public static async Task<string> Post(string url, string data = "")
        {
            var contents = Convert(data);
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage res = await client.PostAsync(url, contents);
                    res.EnsureSuccessStatusCode();
                    return await res.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Network Error Message: " + e.Message);
                }
            }
            return string.Empty;
        }
        public static async Task<string> Get(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage res = await client.GetAsync(url);
                    res.EnsureSuccessStatusCode();
                    return await res.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Network Error Message: " + e.Message);
                }
            }
            return string.Empty;
        }
    }
}
