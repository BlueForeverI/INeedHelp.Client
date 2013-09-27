using System.Threading.Tasks;

namespace Xaml.Chat.Client.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Newtonsoft.Json;

    class HttpRequester
    {
        public const string sessionKeyHeaderName = "X-sessionKey";
        public async static Task<T> Get<T>(string resourceUrl, IDictionary<string, string> headers = null)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "GET";

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            var response = await request.GetResponseAsync();
            string responseString;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }
            var responseData = JsonConvert.DeserializeObject<T>(responseString);
            return responseData;
        }

        public async static Task<WebResponse> Get(string resourceUrl, IDictionary<string, string> headers = null)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "GET";
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            return await request.GetResponseAsync();
        }

        public async static Task<WebResponse> Post(string resourceUrl, object data, IDictionary<string, string> headers = null)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            var jsonData = JsonConvert.SerializeObject(data);

            using (StreamWriter writer =
                new StreamWriter(await request.GetRequestStreamAsync()))
            {
                writer.Write(jsonData);
            }

            return await request.GetResponseAsync();
        }

        public async static Task<T> Post<T>(string resourceUrl,
            object data,
            IDictionary<string, string> headers = null)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            var jsonData = JsonConvert.SerializeObject(data);

            using (StreamWriter writer = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                writer.Write(jsonData);
            }

            try
            {
                var response = await request.GetResponseAsync();
                string responseString;
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }

                var responseData = JsonConvert.DeserializeObject<T>(responseString);
                return responseData;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        //public static void Delete(string resourceUrl)
        //{
        //    var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
        //    request.ContentType = "application/json";
        //    request.Method = "DELETE";
        //    request.GetResponse();
        //}

        //internal static bool Put(string resourceUrl,
        //    IDictionary<string, string> headers = null)
        //{
        //    var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
        //    request.ContentType = "application/json";
        //    request.Method = "PUT";

        //    if (headers != null)
        //    {
        //        foreach (var header in headers)
        //        {
        //            request.Headers.Add(header.Key, header.Value);
        //        }
        //    }

        //    //var jsonData = JsonConvert.SerializeObject(data);

        //    using (StreamWriter writer =
        //        new StreamWriter(request.GetRequestStream()))
        //    {
        //        writer.Write("");
        //    }

        //    try
        //    {
        //        var response = request.GetResponse();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    
    }
}
