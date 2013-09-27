using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using INeedHelp.Client.Commands;
using INeedHelp.Client.Helpers;
using ParseStarterProject.Services;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace INeedHelp.Client.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        private NavigationService navigationService;

        public AppViewModel()
        {
            this.navigationService = new NavigationService();

            
        }

        private ICommand goToLogin;
        public ICommand GoToLogin
        {
            get
            {
                if(this.goToLogin == null)
                {
                    this.goToLogin = new RelayCommand(HandleGoToLogin);
                }

                return this.goToLogin;
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

        private ICommand homeViewLoaded;
        public ICommand HomeViewLoaded
        {
            get
            {
                if(this.homeViewLoaded == null)
                {
                    this.homeViewLoaded = new RelayCommand(HandleHomeViewLoaded);
                }

                return this.homeViewLoaded;
            }
        }

        private void HandleHomeViewLoaded(object obj)
        {
            var vault = new PasswordVault();

            try
            {
                var foundCredentials = vault.FindAllByResource("USER_CREDENTIALS").FirstOrDefault();
                if (foundCredentials != null)
                {
                    var username = foundCredentials.UserName;
                    SuccessMessage = "Welcome, " + username;
                }
                else
                {
                    navigationService.Navigate(ViewType.Login);
                }
            }
            catch (Exception)
            {
                navigationService.Navigate(ViewType.Login);
            }
        }

        private void HandleGoToRegister(object obj)
        {
            navigationService.Navigate(ViewType.Register);
        }

        private void HandleGoToLogin(object obj)
        {
            navigationService.Navigate(ViewType.Login);
        }
    }
}
