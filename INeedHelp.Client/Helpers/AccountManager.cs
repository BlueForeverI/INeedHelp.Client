using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INeedHelp.Client.Data;
using Windows.Security.Credentials;

namespace INeedHelp.Client.Helpers
{
    public class AccountManager
    {
        private const string UserCredentialsToken = "USER_CREDENTIALS";

        public static LoggedUser CurrentUser
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
                        return new LoggedUser(){Username = username, SessionKey = sessionKey};
                    }
                   
                    return null;
                }
                catch (Exception)
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
            }
        }

        public static async Task ClearCurrentUser()
        {
            string sessionKey = CurrentUser.SessionKey;
            await UsersPersister.Logout(sessionKey);
            var vault = new PasswordVault();
            vault.Remove(vault.Retrieve(UserCredentialsToken, CurrentUser.Username));
        }
    }
}
