using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace testingAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            // ExecuteSync();
            ExecuteAsync().Wait();
        }

        private static void ExecuteSync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            RunDownloadSync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("Total execution time: " + elapsedMs);
        }

        private static async Task ExecuteAsync()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // await RunDownloadAsync();
            await RunDownloadParallelAsync();

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

        private static async Task RunDownloadAsync()
        {
            List<string> websites = PrepData();

            foreach (string site in websites)
            {
                WebsiteDataModel results = await Task.Run(() => DownloadWebsite(site));
                ReportWebsiteInfo(results);
            }
        }

        private static async Task RunDownloadParallelAsync()
        {
            List<string> websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();

            foreach (string site in websites)
            {
                //tasks.Add(Task.Run(() => DownloadWebsite(site))); // Use Task.Run(lamda) if no control over method
                tasks.Add(DownloadWebsiteAsync(site));  // Use this if control over method is had
            }

            var results = await Task.WhenAll(tasks);

            foreach (var item in results)
            {
                ReportWebsiteInfo(item);
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

        private static async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteURL = websiteURL;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL); // Built-in

            return output;
        }

        private static void ReportWebsiteInfo(WebsiteDataModel data)
        {
            Console.WriteLine(data.WebsiteURL + " downloaded: " + data.WebsiteData.Length + " characters long.");
        }
    }
}