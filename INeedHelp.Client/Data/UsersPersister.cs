using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Models;
using Newtonsoft.Json;

namespace INeedHelp.Client.Data
{
    public class UsersPersister
    {
        private static string baseUrl = "http://ineedhelp.apphb.com/api/users/";

        public static async Task<UserModel> Login(string username, string passwordHash)
        {
            var user = new UserModel() {Username = username, PasswordHash = passwordHash};
            var response = await HttpRequester.Post<UserModel>(baseUrl + "login", user);

            return response;
        }

        public static async Task<UserModel> Register(UserModel model)
        {
            var response = await HttpRequester.Post<UserModel>(baseUrl + "register", model);
            return response;
        }

        public static async Task Logout(string sessionKey)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("X-sessionKey", sessionKey);

            await HttpRequester.Get(baseUrl + "logout", headers);
        }
    }
}
