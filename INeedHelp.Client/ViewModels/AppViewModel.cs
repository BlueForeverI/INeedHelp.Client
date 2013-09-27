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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace INeedHelp.Client.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        private NavigationService navigationService;
        public string Username { get; set; }

        public IEnumerable<HelpRequestModel> HelpRequests { get; set; } 

        public AppViewModel()
        {
            this.navigationService = new NavigationService();
            CheckIsUserLogged();
        }

        private void CheckIsUserLogged()
        {
            var loggedUser = AccountManager.CurrentUser;

            if (loggedUser != null)
            {
                Username = loggedUser.Username;
                OnPropertyChanged("Username");

                GetRequests();
            }
            else
            {
                navigationService.Navigate(ViewType.Login);
            }
        }

        private async void GetRequests()
        {
            HelpRequests = await HelpRequestsPersister.GetAllRequests(
                AccountManager.CurrentUser.SessionKey);
            OnPropertyChanged("HelpRequests");
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

        private ICommand logout;
        public ICommand Logout
        {
            get
            {
                if(this.logout == null)
                {
                    this.logout = new RelayCommand(HandleLogout);
                }

                return this.logout;
            }
        }

        private ICommand goToAddRequest;
        public ICommand GoToAddRequest
        {
            get
            {
                if(this.goToAddRequest == null)
                {
                    this.goToAddRequest = new RelayCommand(HandleGoToAddRequest);
                }

                return this.goToAddRequest;
            }
        }

        private void HandleGoToAddRequest(object obj)
        {
            navigationService.Navigate(ViewType.AddRequest);
        }

        private async void HandleLogout(object obj)
        {
            await AccountManager.ClearCurrentUser();
            navigationService.Navigate(ViewType.Login);
        }

        private void HandleHomeViewLoaded(object obj)
        {
            CheckIsUserLogged();
        }
    }
}
