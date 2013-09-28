using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace INeedHelp.Client.Helpers
{
    public class ImageUploader
    {
        public static async Task<string> UploadImage(StorageFile file)
        {
            var stream = await file.OpenReadAsync();
            using (var dataReader = new DataReader(stream))
            {
                var bytes = new byte[stream.Size];
                await dataReader.LoadAsync((uint)stream.Size);
                dataReader.ReadBytes(bytes);

                HttpClient client = new HttpClient();

                MultipartFormDataContent contentToAdd = new MultipartFormDataContent();
                contentToAdd.Add(new StringContent("6528448c258cff474ca9701c5bab6927"), "key");
                contentToAdd.Add(new StringContent(Convert.ToBase64String(bytes)), "image");

                var response = await client.PostAsync("http://api.imgur.com/2/upload", contentToAdd);
                var responseBytes = await response.Content.ReadAsByteArrayAsync();

                var doc = XDocument.Load(new MemoryStream(responseBytes));
                var url = doc.Element("upload").Element("links").Element("original").Value;
                return url;
            }
        }
    }
}
