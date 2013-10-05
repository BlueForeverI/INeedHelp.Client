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
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace INeedHelp.Client.ViewModels
{
    public class ProfileSettingsViewModel : BaseViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }

        public ProfileSettingsViewModel()
        {
            if (AccountManager.CurrentUser != null)
            {
                LoadProperties();
            }
        }

        public void LoadProperties()
        {
            FirstName = AccountManager.CurrentUser.FirstName;
            LastName = AccountManager.CurrentUser.LastName;
            ProfilePictureUrl = AccountManager.CurrentUser.ProfilePictureUrl;

            OnPropertyChanged("FirstName");
            OnPropertyChanged("LastName");
            OnPropertyChanged("ProfilePictureUrl");
        }

        public event EventHandler PictureReceived;
        private void RaisePictureReceived()
        {
            if(this.PictureReceived != null)
            {
                this.PictureReceived(this, EventArgs.Empty);
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

        private ICommand saveProfile;
        public ICommand SaveProfile
        {
            get
            {
                if(this.saveProfile == null)
                {
                    this.saveProfile = new RelayCommand(HandleSaveProfile);
                }

                return this.saveProfile;
            }
        }

        private async void HandleSaveProfile(object obj)
        {
            var stackPanel = obj as StackPanel;
            var oldPassword = (stackPanel.Children[1] as PasswordBox).Password;
            var newPassword = (stackPanel.Children[3] as PasswordBox).Password;

            var user = new UserEditModel()
                           {
                               FirstName = FirstName,
                               LastName = LastName,
                               ProfilePictureUrl = ProfilePictureUrl,
                               OldPasswordHash = (!string.IsNullOrEmpty(oldPassword)) 
                                    ? Sha1Encrypter.ConvertToSha1(oldPassword) 
                                    : null,
                               NewPasswordHash = (!string.IsNullOrEmpty(newPassword)) 
                                    ? Sha1Encrypter.ConvertToSha1(newPassword) 
                                    : null,
                           };

            await UsersPersister.Edit(user, AccountManager.CurrentUser.SessionKey);
            var newUser = await UsersPersister.GetUserBySessionKey(AccountManager.CurrentUser.SessionKey);
            AccountManager.CurrentUser = newUser;
            NotificationsManager.ShowToastNotification("Profile saved successfully");
        }

        private async void HandleGetPictureFromFile(object obj)
        {
            var openPicker = new FileOpenPicker();

            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary; ;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".bmp");

            var file = await openPicker.PickSingleFileAsync();
            RaisePictureReceived();

            if (file != null)
            {
                var url = await ImageUploader.UploadImage(file);
                ProfilePictureUrl = url;
                OnPropertyChanged("ProfilePictureUrl");
            }
        }

        private async void HandleGetPictureFromCamera(object obj)
        {
            var ui = new CameraCaptureUI();
            ui.PhotoSettings.CroppedAspectRatio = new Size(4, 3);

            var file = await ui.CaptureFileAsync(CameraCaptureUIMode.Photo);
            RaisePictureReceived();

            if (file != null)
            {
                var url = await ImageUploader.UploadImage(file);
                ProfilePictureUrl = url;
                OnPropertyChanged("ProfilePictureUrl");
            }
        }
    }
}
