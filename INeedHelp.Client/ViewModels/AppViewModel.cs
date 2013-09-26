using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using INeedHelp.Client.Commands;
using INeedHelp.Client.Helpers;
using ParseStarterProject.Services;
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

        private void HandleGoToLogin(object obj)
        {
            navigationService.Navigate(ViewType.Login);
        }
    }
}
