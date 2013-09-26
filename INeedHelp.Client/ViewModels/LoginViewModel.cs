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
            var result = await UsersPersister.Login(Username, passwordBox.Password);
            navigationService.Navigate(ViewType.Home);
        }
    }
}
