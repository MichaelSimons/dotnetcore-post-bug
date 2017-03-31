using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DotnetPost
{
    public class Program
    {

    const string uri = "http://127.0.0.1:8000/post";
    const int numberOfRequests = 1000;
    public static void Main(string[] args)
        {
            var p = new Program();
            p.Run().Wait();
        }
        public async Task Run()
        {
            Console.Out.WriteLine($"POST to {uri} with {numberOfRequests} requests ...");

            var time = 0L;
            for (var i = 0; i < numberOfRequests + 1; i++)
            {
                var cts = new System.Threading.CancellationTokenSource();
                var sender = new HttpMessageSender();
                var sw = Stopwatch.StartNew();

                var response = await sender.Send(new Uri("http://127.0.0.1:8000/post"),
                    "\"test\"",
                    new Dictionary<string, string> { },
                    "text/plain",
                    cts.Token);
                    sw.Stop();
                
                // Don't time first request
                if (i > 1)
                {
                    time += sw.ElapsedMilliseconds;
                    if (i % (numberOfRequests/4) == 0)
                    {
                        Console.Out.WriteLine($"{i/(numberOfRequests/100)}% Time: {time}ms");
                    }
                }
                
            }
            Console.Out.WriteLine($"100% Time:{time}");
            System.Threading.Thread.Sleep(5000);
        }
    }


    public class HttpMessageSender
    {
        private static HttpClient _httpClient;
        public HttpMessageSender()
        {
            // Create _httpClient once only
            if (_httpClient != null)
            {
                return;
            }
            _httpClient = new HttpClient(new HttpClientHandler
            {
                UseProxy = true
            })
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }
        public async Task<Stream> Send(Uri uri, string message, IDictionary<string, string> headers, string contentType, CancellationToken cancellationToken)
        {
            var stringContent = new StringContent(message);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            foreach (var header in headers)
            {
                stringContent.Headers.Add(header.Key, header.Value);
            }
            var result = await _httpClient.PostAsync(uri, stringContent, cancellationToken);
            var resultStreamTask = result.Content.ReadAsStreamAsync();
            var resultStream = await resultStreamTask;
            return resultStream;
        }
    }
}