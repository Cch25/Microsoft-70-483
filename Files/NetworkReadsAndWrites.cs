using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Files
{
    public class NetworkReadsAndWrites
    {
        public async Task WebRequest() => await new WebRequests().WebRequestFromURI();
        public async Task WebClient() => await new WebRequests().WebClientFromURI();
        public async Task HttpClient() => await new WebRequests().HttpClientFromURI();


    }

    public class WebRequests
    {
        public async Task WebRequestFromURI()
        {
            WebRequest request = WebRequest.Create("https://www.microsoft.com");
            WebResponse response = await request.GetResponseAsync();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                Console.WriteLine(sr.ReadToEnd());
            }
        }
        public async Task WebClientFromURI()
        {
            WebClient webClient = new WebClient();
            Uri uri = new Uri("https://www.microsoft.com");
            string text = await webClient.DownloadStringTaskAsync(uri);
            Console.WriteLine(text);
        }
        public async Task<string> HttpClientFromURI()
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetStringAsync("https://microsoft.com");
        }

    }
}
