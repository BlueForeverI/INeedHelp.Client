using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace INeedHelp.Client.Data
{
    public class HttpRequest
    {
        public async static Task<HttpResponseMessage> Post(string url, object data, string sessionKey = "", 
            string mediaValueType="application/json")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            //request.Headers.Add("Content-Type", mediaValueType);

            var jsonData = JsonConvert.SerializeObject(data);
            request.Content = new StringContent(jsonData);

            var client = new HttpClient();
            if(sessionKey != "")
            {
                client.DefaultRequestHeaders.Add("X-sessionKey", sessionKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return await client.SendAsync(request);
        }

        public async static Task<T> Get<T>(string url, string sessionKey = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);        
            var client = new HttpClient();
            if (sessionKey != "")
            {
                client.DefaultRequestHeaders.Add("X-sessionKey", sessionKey);
            }

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<T>(content);
            return responseData;
        }
    }
}
