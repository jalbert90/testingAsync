using System;
using System.Collections.Generic;
using System.Net;

namespace testingAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteSync();
        }

        private static void ExecuteSync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            RunDownloadSync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("Total execution time: " + elapsedMs);
        }

        private static List<string> PrepData()
        {
            List<string> output = new List<string>();

            output.Add("https://www.google.com");
            output.Add("https://www.yahoo.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");

            return output;
        }

        private static void RunDownloadSync()
        {
            List<string> websites = PrepData();

            foreach (string site in websites)
            {
                WebsiteDataModel results = DownloadWebsite(site);
                ReportWebsiteInfo(results);
            }
        }

        private static WebsiteDataModel DownloadWebsite(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteURL = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);

            return output;
        }

        private static void ReportWebsiteInfo(WebsiteDataModel data)
        {
            Console.WriteLine(data.WebsiteURL + " downloaded: " + data.WebsiteData.Length + "characters long.");
        }
    }
}
