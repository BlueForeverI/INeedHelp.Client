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
using Windows.Devices.Geolocation;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace INeedHelp.Client.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string UserPictureUrl { get; set; }
        public string MaxDistance { get; set; }

        public IEnumerable<HelpRequestModel> HelpRequests { get; set; } 

        public AppViewModel()
        {
            CheckIsUserLogged();
        }

        private void CheckIsUserLogged()
        {
            var loggedUser = AccountManager.CurrentUser;

            if (loggedUser != null)
            {
                Username = loggedUser.Username;
                UserPictureUrl = loggedUser.ProfilePictureUrl;

                OnPropertyChanged("Username");
                OnPropertyChanged("UserPictureUrl");

                GetRequests();
            }
            else
            {
                NavigationService.Navigate(ViewType.Login);
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

        private ICommand goToMyRequests;
        public ICommand GoToMyRequests
        {
            get
            {
                if(this.goToMyRequests == null)
                {
                    this.goToMyRequests = new RelayCommand(HandleGoToMyRequests);
                }

                return this.goToMyRequests;
            }
        }

        private ICommand filterRequests;
        public ICommand FilterRequests
        {
            get
            {
                if(this.filterRequests == null)
                {
                    this.filterRequests = new RelayCommand(HandleFilterRequests);
                }

                return this.filterRequests;
            }
        }

        private async void HandleFilterRequests(object obj)
        {
            if(string.IsNullOrEmpty(MaxDistance))
            {
                return;
            }

            int maxDistance;
            if(!int.TryParse(MaxDistance, out maxDistance))
            {
                ErrorMessage = "Invalid distance";
                return;
            }

            ErrorMessage = "";

            try
            {
                var geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
         
                var position = await geolocator.GetGeopositionAsync();
                var coordinates = new CoordinatesModel()
                                      {
                                          Latitude = position.Coordinate.Latitude,
                                          Longitude = position.Coordinate.Longitude
                                      };

                HelpRequests = await HelpRequestsPersister.GetNearRequests(
                    coordinates, maxDistance, AccountManager.CurrentUser.SessionKey);
                OnPropertyChanged("HelpRequests");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Unable to get current location";
            }
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

        private void HandleHomeViewLoaded(object obj)
        {
            CheckIsUserLogged();
        }
    }
}
