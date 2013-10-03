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
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage.Pickers;

namespace INeedHelp.Client.ViewModels
{
    public class AddRequestViewModel : BaseViewModel
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }

        public AddRequestViewModel()
        {
            PictureUrl = "http://i.imgur.com/vLNkdqj.jpg";
        }

        private ICommand addRequest;
        public ICommand AddRequest
        {
            get
            {
                if(this.addRequest == null)
                {
                    this.addRequest = new RelayCommand(HandleAddRequest);
                }

                return this.addRequest;
            }
        }

        private ICommand getPictureFromCamera;
        public ICommand GetPictureFromCamera
        {
            get
            {
                if (this.getPictureFromCamera == null)
                {
                    this.getPictureFromCamera = new RelayCommand(HandleGetPictureFromCamera);
                }

                return this.getPictureFromCamera;
            }
        }

        private ICommand getPictureFromFile;
        public ICommand GetPictureFromFile
        {
            get
            {
                if (this.getPictureFromFile == null)
                {
                    this.getPictureFromFile = new RelayCommand(HandleGetPictureFromFile);
                }

                return this.getPictureFromFile;
            }
        }

        public bool IsAddingRequest { get; set; }

        private async void HandleGetPictureFromFile(object obj)
        {
            var openPicker = new FileOpenPicker();

            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary; ;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".bmp");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                IsAddingRequest = true;
                OnPropertyChanged("IsAddingRequest");

                var url = await ImageUploader.UploadImage(file);
                PictureUrl = url;
                OnPropertyChanged("PictureUrl");


                IsAddingRequest = false;
                OnPropertyChanged("IsAddingRequest");
            }
        }

        private async void HandleGetPictureFromCamera(object obj)
        {
            var ui = new CameraCaptureUI();
            ui.PhotoSettings.CroppedAspectRatio = new Size(4, 3);

            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file != null)
            {
                IsAddingRequest = true;
                OnPropertyChanged("IsAddingRequest");

                var url = await ImageUploader.UploadImage(file);
                PictureUrl = url;
                OnPropertyChanged("PictureUrl");

                IsAddingRequest = false;
                OnPropertyChanged("IsAddingRequest");
            }
        }

        private async void HandleAddRequest(object obj)
        {
            
            try
            {
                if(string.IsNullOrEmpty(Title) || string.IsNullOrWhiteSpace(Title))
                {
                    ErrorMessage = "Enter a request title";
                    return;
                }

                if (string.IsNullOrEmpty(Text) || string.IsNullOrWhiteSpace(Text))
                {
                    ErrorMessage = "Enter a request text";
                    return;
                }

                var geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
         
                var position = await geolocator.GetGeopositionAsync();
                var coordinates = new CoordinatesModel()
                                      {
                                          Latitude = position.Coordinate.Latitude,
                                          Longitude = position.Coordinate.Longitude
                                      };

                var request = new HelpRequestModel()
                                  {
                                      Title = Title,
                                      Text = Text,
                                      PictureUrl = PictureUrl,
                                      Coordinates = coordinates
                                  };

                IsAddingRequest = true;
                OnPropertyChanged("IsAddingRequest");

                await HelpRequestsPersister.AddRequest(request, AccountManager.CurrentUser.SessionKey);
                NavigationService.Navigate(ViewType.MyRequests);
                NotificationsManager.ShowNotification("Request added");

            }
            catch (Exception ex)
            {
                ErrorMessage = "Unable to get location";

                IsAddingRequest = false;
                OnPropertyChanged("IsAddingRequest");
            }
        }
    }
}
