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
using Windows.Security.Credentials;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace INeedHelp.Client.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }

        public bool Registering { get; set; }

        public RegisterViewModel()
        {
            ProfilePictureUrl = "http://i.imgur.com/0E6rxzp.png";
            OnPropertyChanged("ProfilePictureUrl");

            Registering = false;
            OnPropertyChanged("Registering");
        }

        private ICommand register;
        public ICommand Register
        {
            get
            {
                if (this.register == null)
                {
                    this.register = new RelayCommand(HandleRegister);
                }

                return this.register;
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
                Registering = true;
                OnPropertyChanged("Registering");

                var url = await ImageUploader.UploadImage(file);
                ProfilePictureUrl = url;
                OnPropertyChanged("ProfilePictureUrl");

                Registering = false;
                OnPropertyChanged("Registering");
            }
        }

        private async void HandleGetPictureFromCamera(object obj)
        {
            var ui = new CameraCaptureUI();
            ui.PhotoSettings.CroppedAspectRatio = new Size(4, 3);

            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (file != null)
            {
                Registering = true;
                OnPropertyChanged("Registering");

                var url = await ImageUploader.UploadImage(file);
                ProfilePictureUrl = url;
                OnPropertyChanged("ProfilePictureUrl");

                Registering = false;
                OnPropertyChanged("Registering");
            }
        }

        private async void HandleRegister(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;

            if(string.IsNullOrEmpty(Username) || string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Enter a username";
                return;
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Enter a password";
                return;
            }

            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrWhiteSpace(FirstName))
            {
                ErrorMessage = "Enter a first name";
                return;
            }

            if (string.IsNullOrEmpty(LastName) || string.IsNullOrWhiteSpace(LastName))
            {
                ErrorMessage = "Enter a last name";
                return;
            }

            Registering = true;
            OnPropertyChanged("Registering");

            var passwordHash = Sha1Encrypter.ConvertToSha1(password);

            var userModel = new UserModel()
                                {
                                    Username = Username,
                                    PasswordHash = passwordHash,
                                    FirstName = FirstName,
                                    LastName = LastName,
                                    ProfilePictureUrl = ProfilePictureUrl
                                };

            var loggedUser = await UsersPersister.Register(userModel);
            if (loggedUser != null)
            {
                AccountManager.CurrentUser = loggedUser;

                NavigationService.Navigate(ViewType.Home);
            }
            else
            {
                Registering = false;
                OnPropertyChanged("Registering");
                ErrorMessage = "Cannot register user";
            }
        }
    }
}
