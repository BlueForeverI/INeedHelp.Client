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
                var url = await ImageUploader.UploadImage(file);
                PictureUrl = url;
                OnPropertyChanged("PictureUrl");
            }
        }

        private async void HandleGetPictureFromCamera(object obj)
        {
            var ui = new CameraCaptureUI();
            ui.PhotoSettings.CroppedAspectRatio = new Size(4, 3);

            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file != null)
            {
                var url = await ImageUploader.UploadImage(file);
                PictureUrl = url;
                OnPropertyChanged("PictureUrl");
            }
        }

        private async void HandleAddRequest(object obj)
        {
            var request = new HelpRequestModel()
                              {
                                  Title = Title,
                                  Text = Text,
                                  PictureUrl = PictureUrl
                              };

            await HelpRequestsPersister.AddRequest(request, AccountManager.CurrentUser.SessionKey);
            NavigationService.Navigate(ViewType.MyRequests);
            NotificationsManager.ShowNotification("Request added");
        }
    }
}
