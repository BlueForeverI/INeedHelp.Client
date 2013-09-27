﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Models;
using Newtonsoft.Json;
using Xaml.Chat.Client.Data;

namespace INeedHelp.Client.Data
{
    public class UsersPersister
    {
        private static string baseUrl = "http://ineedhelp.apphb.com/api/users/";

        public async static Task<UserModel> Login(string username, string passwordHash)
        {
            var user = new UserModel() {Username = username, PasswordHash = passwordHash};
            var response = await HttpRequester.Post<UserModel>(baseUrl + "login", user);

            return response;
        }

        public async static Task<UserModel> Register(UserModel model)
        {
            var response = await HttpRequester.Post<UserModel>(baseUrl + "register", model);
            return response;
        }
    }
}