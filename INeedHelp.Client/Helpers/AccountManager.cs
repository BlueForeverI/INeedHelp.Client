using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Data;
using INeedHelp.Client.Models;
using Windows.Security.Credentials;
using Windows.Storage;

namespace INeedHelp.Client.Helpers
{
    public class AccountManager
    {
        private const string UserCredentialsToken = "USER_CREDENTIALS";
        private static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static UserModel CurrentUser
        {
            get
            {
                PasswordVault vault = new PasswordVault();
                try
                {
                    var foundCredentials = vault.FindAllByResource(UserCredentialsToken).FirstOrDefault();
                    if (foundCredentials != null)
                    {
                        var username = foundCredentials.UserName;
                        var sessionKey = vault.Retrieve(UserCredentialsToken, username).Password;
                        return new UserModel()
                                   {
                                       Username = username, 
                                       SessionKey = sessionKey,
                                       ProfilePictureUrl = localSettings.Values["ProfilePictureUrl"].ToString(),
                                       FirstName = localSettings.Values["FirstName"].ToString(),
                                       LastName = localSettings.Values["LastName"].ToString(),
                                       Reputation = (int)localSettings.Values["Reputation"]
                                   };
                    }
                   
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            set
            {
                var vault = new PasswordVault();
                var credential = new PasswordCredential(UserCredentialsToken, 
                    value.Username, value.SessionKey);
                vault.Add(credential);

                localSettings.Values["ProfilePictureUrl"] = value.ProfilePictureUrl;
                localSettings.Values["FirstName"] = value.FirstName;
                localSettings.Values["LastName"] = value.LastName;
                localSettings.Values["Reputation"] = value.Reputation;
            }
        }

        public static async Task ClearCurrentUser()
        {
            string sessionKey = CurrentUser.SessionKey;
            await UsersPersister.Logout(sessionKey);
            var vault = new PasswordVault();
            vault.Remove(vault.Retrieve(UserCredentialsToken, CurrentUser.Username));


            localSettings.Values["ProfilePictureUrl"] = "";
            localSettings.Values["FirstName"] = "";
            localSettings.Values["LastName"] = "";
            localSettings.Values["Reputation"] = "";
        }
    }
}
