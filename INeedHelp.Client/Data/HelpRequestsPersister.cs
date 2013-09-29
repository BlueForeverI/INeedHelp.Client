using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Models;

namespace INeedHelp.Client.Data
{
    public class HelpRequestsPersister
    {
        private static string baseUrl = "http://ineedhelp.apphb.com/api/requests/";
        private static string sessionHeaderName = "X-sessionKey";
        private static Dictionary<string, string> headers = new Dictionary<string, string>();

        static HelpRequestsPersister()
        {
            headers.Add(sessionHeaderName, "");
        }

        public static async Task<IEnumerable<HelpRequestModel>> GetAllRequests(string sessionKey)
        {
            headers[sessionHeaderName] = sessionKey;
            return await HttpRequester.Get<IEnumerable<HelpRequestModel>>(baseUrl + "all", headers);
        }

        public static async Task AddRequest(HelpRequestModel request, string sessionKey)
        {
            headers[sessionHeaderName] = sessionKey;
            await HttpRequester.Post(baseUrl + "add", request, headers);
        }

        public static async Task AddComment(int id, CommentModel comment, string sessionKey)
        {
            headers[sessionHeaderName] = sessionKey;
            await HttpRequester.Post(baseUrl + "comment/" + id.ToString(), comment, headers);
        }

        public static async Task AddHelper(int requestId, int id, string sessionKey)
        {
            string url = baseUrl + requestId.ToString() + "/addhelper/" + id.ToString();
            headers[sessionHeaderName] = sessionKey;
            await HttpRequester.Get(url, headers);
        }

        public static async Task<IEnumerable<HelpRequestModel>> GetRequestsByUser(string sessionKey)
        {
            headers[sessionHeaderName] = sessionKey;
            return await HttpRequester.Get<IEnumerable<HelpRequestModel>>(baseUrl + "byuser", headers);
        }

        public static async Task MarkRequestSolved(int id, string sessionKey)
        {
            headers[sessionHeaderName] = sessionKey;
            await HttpRequester.Get(baseUrl + "solve/" + id.ToString(), headers);
        }

        public static async Task<HelpRequestModel> GetRequestById(int id, string sessinoKey)
        {
            headers[sessionHeaderName] = sessinoKey;
            return await HttpRequester.Get<HelpRequestModel>(baseUrl + "byid/" + id.ToString(), headers);
        }

        public static async Task EditRequest(HelpRequestModel request, string sessionKey)
        {
            headers[sessionHeaderName] = sessionKey;
            await HttpRequester.Post(baseUrl + "edit", request, headers);
        }

        public static async Task<IEnumerable<HelpRequestModel>> GetNearRequests(
            CoordinatesModel coordinates, int maxDistance, string sessionKey)
        {
            headers[sessionHeaderName] = sessionKey;
            string url = baseUrl + "near/" + maxDistance.ToString();
            return await HttpRequester.Post<IEnumerable<HelpRequestModel>>(url, coordinates, headers);
        }
    }
}
