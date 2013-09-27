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
    public class RegisterViewModel : BaseViewModel
    {
        private NavigationService navigationService;
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public RegisterViewModel()
        {
            this.navigationService = new NavigationService();
        }

        private ICommand register;
        public ICommand Register
        {
            get
            {
                if(this.register == null)
                {
                    this.register = new RelayCommand(HandleRegister);
                }

                return this.register;
            }
        }

        private async void HandleRegister(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;
            var passwordHash = Sha1Encrypter.ConvertToSha1(password);

            var userModel = new UserModel()
                                {
                                    Username = Username,
                                    PasswordHash = passwordHash,
                                    FirstName = FirstName,
                                    LastName = LastName
                                };

            var loggedUser = await UsersPersister.Register(userModel);
            if(loggedUser != null)
            {
                AccountManager.CurrentUser = new LoggedUser()
                {
                    Username = loggedUser.Username, 
                    SessionKey = loggedUser.SessionKey
                };
                navigationService.Navigate(ViewType.Home);
            }
            else
            {
                ErrorMessage = "Cannot register user";
            }
        }
    }
}
