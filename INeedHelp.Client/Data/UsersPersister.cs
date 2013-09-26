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

        public async static Task<UserModel> Login(string username, string passwordHash)
        {
            var user = new UserModel() {Username = username, PasswordHash = passwordHash};
            var response = await HttpRequest.Post(baseUrl + "login", user);
            
            if(response.StatusCode == HttpStatusCode.OK)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var loggedUser = JsonConvert.DeserializeObject<UserModel>(contentString);
                return loggedUser;
            }

            return null;
        }
    }
}
