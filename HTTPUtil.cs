using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WVCore.Server
{
    public class HTTPUtil
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static readonly HttpClient AppHttpClient = new(new HttpClientHandler
        {
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.All,
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        })
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        public static async Task<byte[]> PostDataAsync(string URL, Dictionary<string, string> headers, byte[] postData)
        {
            logger.Debug($"Post to: {URL}");
            logger.Debug($"Post data: {Util.BytesToHex(postData, " ")}");
            ByteArrayContent content = new ByteArrayContent(postData);
            if (headers.TryGetValue("Content-Type", out var contentType))
            {
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
            }
            HttpResponseMessage response = await PostAsync(URL, headers, content);
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();
            logger.Debug($"Recv data: {Util.BytesToHex(bytes, " ")}");
            return bytes;
        }

        private static async Task<HttpResponseMessage> PostAsync(string URL, Dictionary<string, string> headers, HttpContent content)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(URL),
                Method = HttpMethod.Post,
                Content = content
            };

            if (headers != null)
                foreach (KeyValuePair<string, string> header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);

            logger.Debug(request.Headers.ToString());

            return await SendAsync(request);
        }

        static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await AppHttpClient.SendAsync(request);
        }
    }
}
