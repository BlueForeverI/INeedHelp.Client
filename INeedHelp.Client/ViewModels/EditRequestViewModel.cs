using System;
using System.Collections;
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
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage.Pickers;

namespace INeedHelp.Client.ViewModels
{
    public class EditRequestViewModel : BaseViewModel
    {
        public HelpRequestModel Request { get; set; }
        
        public int CommentsCount
        {
            get
            {
                return (Request != null && Request.Comments != null) 
                    ? Request.Comments.Count() : 0;
            }
        }

        public EditRequestViewModel()
        {
            RegisterForShare();
        }

        private ICommand editRequestLoaded;
        public ICommand EditRequestLoaded
        {
            get
            {
                if (this.editRequestLoaded == null)
                {
                    this.editRequestLoaded = new RelayCommand(HandleEditRequestLoaded);
                }

                return this.editRequestLoaded;
            }
        }

        private ICommand addHelper;
        public ICommand AddHelper
        {
            get
            {
                if(this.addHelper == null)
                {
                    this.addHelper = new RelayCommand(HandleAddHelper);
                }

                return this.addHelper;
            }
        }

        private ICommand saveChanges;
        public ICommand SaveChanges
        {
            get
            {
                if(this.saveChanges == null)
                {
                    this.saveChanges = new RelayCommand(HandleSaveChanges);
                }

                return this.saveChanges;
            }
        }

        private ICommand exportRequest;
        private static TypedEventHandler<DataTransferManager, DataRequestedEventArgs> _handler;

        public ICommand ExportRequest
        {
            get
            {
                if(this.exportRequest == null)
                {
                    this.exportRequest = new RelayCommand(HandleExportRequest);
                }

                return this.exportRequest;
            }
        }

        private ICommand addTile;
        public ICommand AddTile
        {
            get
            {
                if(this.addTile == null)
                {
                    this.addTile = new RelayCommand(HandleAddTile);
                }

                return this.addTile;
            }
        }

        private async void HandleAddTile(object obj)
        {
            var tileID = "SecondaryTile." + Request.Id.ToString();
            var shortName = Request.Title;
            var displayName = Request.Title;
            var secondaryTileArg = Request.Id.ToString();
            var squareLogo = new System.Uri("ms-appx:///Assets/StoreLogo.png");
            var wideLogo = new System.Uri("ms-appx:///Assets/WideLogo.png");
            var tileOptions = Windows.UI.StartScreen.TileOptions.ShowNameOnWideLogo | 
                Windows.UI.StartScreen.TileOptions.ShowNameOnLogo;

            var secondaryTile = new Windows.UI.StartScreen.SecondaryTile(
                tileID, shortName, displayName, secondaryTileArg, tileOptions, squareLogo, wideLogo);
            await secondaryTile.RequestCreateAsync();

            NotificationsManager.ShowToastNotification("Secondary tile added successfully!");
        }

        private async void HandleExportRequest(object obj)
        {
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("HTML Page", new List<string>(){".html"});
            picker.SuggestedFileName = Request.Title + ".html";
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var file = await picker.PickSaveFileAsync();

            if(file != null)
            {
                await RequestExporter.ExportToHtml(Request, file);
                NotificationsManager.ShowToastNotification("Request exported successfully");
            }
        }

        private async void HandleSaveChanges(object obj)
        {

            if(string.IsNullOrEmpty(Request.Title) || string.IsNullOrWhiteSpace(Request.Title))
            {
                ErrorMessage = "Enter a request title";
                return;
            }

            if (string.IsNullOrEmpty(Request.Text) || string.IsNullOrWhiteSpace(Request.Text))
            {
                ErrorMessage = "Enter a request text";
                return;
            }

            IsSavingRequest = true;
            OnPropertyChanged("IsSavingRequest");

            await HelpRequestsPersister.EditRequest(Request, AccountManager.CurrentUser.SessionKey);
            NotificationsManager.ShowToastNotification("Request details saved");
            NavigationService.Navigate(ViewType.MyRequests);
        }

        private async void HandleAddHelper(object obj)
        {
            int id = (int) obj;
            await HelpRequestsPersister.AddHelper(Request.Id, id, AccountManager.CurrentUser.SessionKey);
            LoadRequest(Request.Id);
        }

        public int HelpersCount
        {
            get { return (Request != null && Request.Comments != null) ? Request.Helpers.Count() : 0; }
        }

        public IEnumerable<UserModel> SuggestedHelpers
        {
            get
            {

                return (Request != null && Request.Comments != null) ? 
                    Request.Comments.Select(c => c.User)
                    .Where(u => !Request.Helpers.Any(h => h.Id == u.Id)).Distinct(new UserModelComparer()) : 
                    new List<UserModel>();
            }
        }

        public bool IsSavingRequest { get; set; }


        private void HandleEditRequestLoaded(object obj)
        {
            var request = obj as HelpRequestModel;
            LoadRequest(request.Id);
        }

        private async void LoadRequest(int requestId)
        {
            var fullRequest = await HelpRequestsPersister.GetRequestById(requestId, AccountManager.CurrentUser.SessionKey);
            this.Request = fullRequest;

            OnPropertyChanged("Request");
            OnPropertyChanged("CommentsCount");
            OnPropertyChanged("HelpersCount");
            OnPropertyChanged("SuggestedHelpers");
        }

        private void RegisterForShare()
        {
            if (_handler != null)
                DataTransferManager.GetForCurrentView().DataRequested -= _handler;


            _handler = ShareTextHandler;
            DataTransferManager.GetForCurrentView().DataRequested += _handler;
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = Request.Title;
            request.Data.Properties.Description = "Help Request";
            request.Data.SetText(Request.Text);
        }
    }
}
