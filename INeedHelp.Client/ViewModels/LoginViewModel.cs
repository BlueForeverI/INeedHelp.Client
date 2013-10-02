using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using INeedHelp.Client.Commands;
using INeedHelp.Client.Data;
using INeedHelp.Client.Helpers;
using INeedHelp.Client.Models;
using ParseStarterProject.Services;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Controls;

namespace INeedHelp.Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoggingIn = false;
            OnPropertyChanged("LoggingIn");
        }

        public string Username { get; set; }
        public bool LoggingIn { get; set; }

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

        private ICommand goToRegister;
        public ICommand GoToRegister
        {
            get
            {
                if(this.goToRegister == null)
                {
                    this.goToRegister = new RelayCommand(HandleGoToRegister);
                }

                return this.goToRegister;
            }
        }

        private void HandleGoToRegister(object obj)
        {
            NavigationService.Navigate(ViewType.Register);
        }

        private async void HandleLogin(object obj)
        {
            LoggingIn = true;
            OnPropertyChanged("LoggingIn");

            var passwordBox = obj as PasswordBox;
            var passwordHash = Sha1Encrypter.ConvertToSha1(passwordBox.Password);
            var loggedUser = await UsersPersister.Login(Username, passwordHash);

            if (loggedUser != null)
            {
                AccountManager.CurrentUser = loggedUser;

                NavigationService.Navigate(ViewType.Home);
            }
            else
            {
                ErrorMessage = "Invalid username or password";

                LoggingIn = false;
                OnPropertyChanged("LoggingIn");
            }
        }
    }
}
