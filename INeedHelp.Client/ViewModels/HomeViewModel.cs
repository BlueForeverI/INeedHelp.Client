using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using INeedHelp.Client.Commands;
using INeedHelp.Client.Data;
using INeedHelp.Client.Helpers;
using INeedHelp.Client.Models;
using ParseStarterProject.Services;
using Windows.Devices.Geolocation;

namespace INeedHelp.Client.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public string MaxDistance { get; set; }

        public IEnumerable<HelpRequestModel> HelpRequests { get; set; } 

        public HomeViewModel()
        {
            CheckIsUserLogged();
        }

        private void CheckIsUserLogged()
        {
            var loggedUser = AccountManager.CurrentUser;

            if (loggedUser != null)
            {
                OnPropertyChanged("Username");
                OnPropertyChanged("UserPictureUrl");

                RequestsLoading = true;
                OnPropertyChanged("RequestsLoading");
                OnPropertyChanged("RequestsVisible");
                GetRequests();
            }
            else
            {
                NavigationService.Navigate(ViewType.Login);
            }
        }

        private async void GetRequests()
        {
            try
            {
                int oldCount = (HelpRequests != null) ? HelpRequests.Count() : 0;
                HelpRequests = await HelpRequestsPersister.GetAllRequests(
                    AccountManager.CurrentUser.SessionKey);

                RequestsLoading = false;
                OnPropertyChanged("RequestsLoading");
                OnPropertyChanged("RequestsVisible");

                OnPropertyChanged("HelpRequests");

                NotificationsManager.ShowTileNotification("Help Requests");
                NotificationsManager.ShowBadgeNotification(HelpRequests.Count());
            }
            catch (Exception)
            {
                ErrorMessage = "Error while getting help requests. Check your internet connection";
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

        public bool RequestsLoading { get; set; }
        public bool RequestsVisible { get { return !RequestsLoading; } }

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

        private void HandleHomeViewLoaded(object obj)
        {
            CheckIsUserLogged();
        }
    }
}
