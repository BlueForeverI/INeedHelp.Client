using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using INeedHelp.Client.Commands;
using INeedHelp.Client.Data;
using INeedHelp.Client.Helpers;
using ParseStarterProject.Services;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace INeedHelp.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private NavigationService navigationService;

        public LoginViewModel()
        {
            this.navigationService = new NavigationService();
        }

        public string Username { get; set; }

        private ICommand login;
        public ICommand Login
        {
            get
            {
                if(this.login == null)
                {
                    this.login = new RelayCommand(HandleLogin);
                }

                return this.login;
            }
        }

        private async void HandleLogin(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var loggedUser = await UsersPersister.Login(Username, passwordBox.Password);

            if (loggedUser != null)
            {
                var vault = new PasswordVault();
                var credential = new PasswordCredential("USER_CREDENTIALS", loggedUser.Username, loggedUser.SessionKey);
                vault.Add(credential);
                navigationService.Navigate(ViewType.Home);
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }
    }
}
