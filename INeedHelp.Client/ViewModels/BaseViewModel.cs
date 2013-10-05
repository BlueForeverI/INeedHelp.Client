using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using INeedHelp.Client.Commands;
using INeedHelp.Client.Helpers;
using ParseStarterProject.Services;

namespace INeedHelp.Client.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string Username
        {
            get { return (AccountManager.CurrentUser != null) ? AccountManager.CurrentUser.Username : ""; }
        }

        public string UserPictureUrl
        {
            get { return (AccountManager.CurrentUser != null) ? AccountManager.CurrentUser.ProfilePictureUrl : ""; }
        }

        public int Reputation
        {
            get { return (AccountManager.CurrentUser != null) ? AccountManager.CurrentUser.Reputation : 0; }
        }

        private string successMessage;
        private string errorMessage;

        public string SuccessMessage
        {
            get { return this.successMessage; }
            set { this.successMessage = value; OnPropertyChanged("SuccessMessage"); }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand logout;
        public ICommand Logout
        {
            get
            {
                if (this.logout == null)
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
                if (this.goToAddRequest == null)
                {
                    this.goToAddRequest = new RelayCommand(HandleGoToAddRequest);
                }

                return this.goToAddRequest;
            }
        }

        private ICommand goToMyRequests;
        public ICommand GoToMyRequests
        {
            get
            {
                if (this.goToMyRequests == null)
                {
                    this.goToMyRequests = new RelayCommand(HandleGoToMyRequests);
                }

                return this.goToMyRequests;
            }
        }

        private ICommand goToHome;
        public ICommand GoToHome
        {
            get
            {
                if (this.goToHome == null)
                {
                    this.goToHome = new RelayCommand(HandleGoToHome);
                }

                return this.goToHome;
            }
        }

        private void HandleGoToHome(object obj)
        {
            NavigationService.Navigate(ViewType.Home);
        }

        private void HandleGoToMyRequests(object obj)
        {
            NavigationService.Navigate(ViewType.MyRequests);
        }

        private void HandleGoToAddRequest(object obj)
        {
            NavigationService.Navigate(ViewType.AddRequest);
        }

        private async void HandleLogout(object obj)
        {
            await AccountManager.ClearCurrentUser();
            NavigationService.Navigate(ViewType.Login);
        }
    }
}
